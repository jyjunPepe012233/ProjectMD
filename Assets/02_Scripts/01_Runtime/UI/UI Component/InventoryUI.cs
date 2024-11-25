using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MinD.Runtime.Entity;
using MinD.SO.Item;

namespace MinD.Runtime.UI {

public class InventoryUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public ScrollRect scrollRect;
    public List<Transform> categoryPolygon;
    public List<Transform> categoryPanels;
    private List<List<InventorySlot>> categorySlots;

    private int selectedSlotIndex = 0;
    private int inventoryWidth = 5;
    private PlayerInventoryHandler playerInventory;
    private ItemSoList itemSoList;

    private int currentCategoryIndex = 0;

    public GameObject inventoryPanel;
    private bool isInventoryActive = false;

    void Start()
    {
        playerInventory = FindObjectOfType<Player>().inventory;
        categorySlots = new List<List<InventorySlot>>();

        // 각 카테고리 패널에 슬롯 생성
        for (int i = 0; i < categoryPanels.Count; i++)
        {
            List<InventorySlot> slots = CreateSlots(categoryPanels[i], 25, i); // i를 통해 categoryId 설정
            categorySlots.Add(slots);
        }

        UpdateCategory();
        UpdateInventoryUI();
        UpdateSelectionImage();
        inventoryPanel.SetActive(false);
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }

        if (!isInventoryActive) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            ChangeCategory(-1);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            ChangeCategory(1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveSelection(1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveSelection(-1);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ScrollUp();
            MoveSelection(-inventoryWidth);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ScrollDown();
            MoveSelection(inventoryWidth);
        }
    }

    void ToggleInventory()
    {
        isInventoryActive = !isInventoryActive;
        inventoryPanel.SetActive(isInventoryActive);

        if (isInventoryActive)
        {
            UpdateCategory();
        }
    }

    void ChangeCategory(int direction)
    {
        currentCategoryIndex = (currentCategoryIndex + direction + categoryPanels.Count) % categoryPanels.Count;
        UpdateCategory();
    
        scrollRect.content.anchoredPosition = new Vector2(scrollRect.content.anchoredPosition.y, 0);
    }


    void UpdateCategory()
    {
        foreach (var panel in categoryPanels)
        {
            panel.gameObject.SetActive(false);
        }

        Transform currentPanel = categoryPanels[currentCategoryIndex];
        currentPanel.gameObject.SetActive(true);

        UpdateCategoryPolygon();

        scrollRect.content = currentPanel.GetComponent<RectTransform>();

        selectedSlotIndex = 0;
        UpdateInventoryUI();
        UpdateSelectionImage();
        ScrollToSelectedSlot();
        scrollRect.normalizedPosition = new Vector2(scrollRect.normalizedPosition.y, 0);
    }

    void UpdateCategoryPolygon()
    {
        for (int i = 0; i < categoryPolygon.Count; i++)
        {
            categoryPolygon[i].gameObject.SetActive(i == currentCategoryIndex);
        }
    }

    void MoveSelection(int direction)
    {
        int newSelectedIndex = selectedSlotIndex + direction;

        if (newSelectedIndex < 0 || newSelectedIndex >= categorySlots[currentCategoryIndex].Count)
        {
            return;
        }

        if (direction == 1 && (selectedSlotIndex + 1) % inventoryWidth == 0)
        {
            return;
        }
        else if (direction == -1 && selectedSlotIndex % inventoryWidth == 0)
        {
            return;
        }

        selectedSlotIndex = newSelectedIndex;
        UpdateSelectionImage();
    }

    void UpdateSelectionImage()
    {
        var slots = categorySlots[currentCategoryIndex];
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetSelected(i == selectedSlotIndex);
        }
    }

    void ScrollDown()
    {
        if (scrollRect.content.anchoredPosition.y >= 65 * GetCurrentCategorySlotRange()) 
            return;
        
        Vector2 newPosition = scrollRect.content.anchoredPosition;
        newPosition.y += 134;
        scrollRect.content.anchoredPosition = newPosition;
    }

    void ScrollUp()
    {
        if (scrollRect.content.anchoredPosition.y <= -65 * GetCurrentCategorySlotRange())
            return;
        
        Vector2 newPosition = scrollRect.content.anchoredPosition;
        newPosition.y -= 134;
        scrollRect.content.anchoredPosition = newPosition;
    }

    void ScrollToSelectedSlot()
    {
        if (scrollRect == null || categorySlots[currentCategoryIndex].Count == 0) return;

        RectTransform slotRect = categorySlots[currentCategoryIndex][selectedSlotIndex].GetComponent<RectTransform>();

        Vector2 viewportLocalPosition = (Vector2)scrollRect.viewport.InverseTransformPoint(slotRect.position);
        Vector2 contentLocalPosition = (Vector2)scrollRect.content.InverseTransformPoint(slotRect.position);

        float contentHeight = scrollRect.content.rect.height;
        float viewportHeight = scrollRect.viewport.rect.height;

        float targetY = contentLocalPosition.y - viewportLocalPosition.y;
        float clampedY = Mathf.Clamp(targetY, 0, contentHeight - viewportHeight);

        Vector2 newContentPosition = new Vector2(scrollRect.content.anchoredPosition.x, -clampedY);
        scrollRect.content.anchoredPosition = newContentPosition;
    }

    List<InventorySlot> CreateSlots(Transform panel, int slotCount, int categoryId)
    {
        List<InventorySlot> slots = new List<InventorySlot>();

        for (int i = 0; i < slotCount; i++)
        {
            InventorySlot newSlot = AddSlot(panel, categoryId);
            slots.Add(newSlot);
        }

        return slots;
    }

    InventorySlot AddSlot(Transform panel, int categoryId)
    {
        GameObject newSlotObject = Instantiate(slotPrefab, panel);
        InventorySlot slot = newSlotObject.GetComponent<InventorySlot>();

        slot.categoryId = categoryId; // 슬롯에 카테고리 ID 설정

        Button button = newSlotObject.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(slot.OnClick);
        }

        return slot;
    }

    public void OnSlotClicked(InventorySlot clickedSlot)
    {
        int clickedIndex = categorySlots[currentCategoryIndex].IndexOf(clickedSlot);
        if (clickedIndex >= 0)
        {
            selectedSlotIndex = clickedIndex;
            UpdateSelectionImage();
            ScrollToSelectedSlot();
        }
    }

    public void UpdateInventoryUI()
    {
        Item[] playerItems = playerInventory.playerItemList;
        var slots = categorySlots[currentCategoryIndex];
        int slotIndex = 0;

        for (int i = 0; i < playerItems.Length; i++)
        {
            if (playerItems[i] != null && playerItems[i].itemCount > 0 && playerItems[i].categoryId == currentCategoryIndex)
            {
                if (slotIndex < slots.Count)
                {
                    slots[slotIndex].SetItem(playerItems[i], currentCategoryIndex);
                    slotIndex++;
                }
            }
        }

        for (int i = slotIndex; i < slots.Count; i++)
        {
            slots[i].ClearSlot();
        }
    }

    int GetCurrentCategorySlotRange()
    {
        int slotCount = categorySlots[currentCategoryIndex].Count;
        int rangeSize = 5;
        int baseCount = 25;

        if (slotCount < baseCount)
            return -1;
        

        return (slotCount - baseCount) / rangeSize;
    }
}

}