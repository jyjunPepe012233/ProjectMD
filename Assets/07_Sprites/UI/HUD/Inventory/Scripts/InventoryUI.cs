using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    // Inspector에서 설정 가능한 슬롯 프리팹, 스크롤, 카테고리 관련 UI 및 슬롯 목록들
    public GameObject slotPrefab;
    public ScrollRect scrollRect;
    public List<Transform> categoryPolygon; 
    public List<Transform> categoryPanels;
    private List<List<InventorySlot>> categorySlots;

    private int selectedSlotIndex = 0; // 현재 선택된 슬롯의 인덱스
    private int inventoryWidth = 5;    // 인벤토리의 가로 길이
    private PlayerInventoryHandler playerInventory; // 플레이어의 인벤토리 핸들러

    private int currentCategoryIndex = 0; // 현재 선택된 카테고리 인덱스

    public GameObject inventoryPanel; // 전체 인벤토리 패널
    private bool isInventoryActive = false; // 인벤토리 활성화 여부

    void Start()
    {
        // 초기화: 플레이어 인벤토리 찾고, 각 카테고리에 슬롯 생성
        playerInventory = FindObjectOfType<Player>().inventory;
        categorySlots = new List<List<InventorySlot>>();

        foreach (var panel in categoryPanels)
        {
            List<InventorySlot> slots = CreateSlots(panel, 25);
            categorySlots.Add(slots);
        }

        UpdateCategory();
        UpdateInventoryUI();
        UpdateSelectionImage();
        inventoryPanel.SetActive(false); // 시작할 때 인벤토리 비활성화
    }

    void Update()
    {
        HandleInput(); // 입력 처리 함수 호출
    }

    void HandleInput()
    {
        // 인벤토리 열기/닫기 토글
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory(); 
        }

        if (!isInventoryActive) return; // 인벤토리 비활성화 시 조작 무시

        // 카테고리 변경, 슬롯 선택 이동, 새 슬롯 추가에 따른 입력 처리
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
        else if (Input.GetKeyDown(KeyCode.E))
        {
            AddSlot(currentCategoryIndex);
        }
    }

    // 인벤토리 열기/닫기 토글
    void ToggleInventory()
    {
        isInventoryActive = !isInventoryActive; 
        inventoryPanel.SetActive(isInventoryActive);
        if (isInventoryActive)
        {
            UpdateCategory(); // 현재 카테고리 갱신
        }
    }

    // 카테고리 변경
    void ChangeCategory(int direction)
    {
        currentCategoryIndex += direction;
        if (currentCategoryIndex < 0)
        {
            currentCategoryIndex = categoryPanels.Count - 1;
        }
        else if (currentCategoryIndex >= categoryPanels.Count)
        {
            currentCategoryIndex = 0;
        }

        UpdateCategory(); // 카테고리 갱신
    }

    // 카테고리 UI 갱신 및 활성화
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
        UpdateSelectionImage();
        ScrollToSelectedSlot();
    }

    // 카테고리 UI 폴리곤 상태 갱신
    void UpdateCategoryPolygon()
    {
        for (int i = 0; i < categoryPolygon.Count; i++)
        {
            categoryPolygon[i].gameObject.SetActive(i == currentCategoryIndex);
        }
    }

    // 슬롯 선택 이동
    void MoveSelection(int direction)
    {
        int newSelectedIndex = selectedSlotIndex + direction;

        // 인덱스가 유효한 경우에만 이동
        if (newSelectedIndex < 0 || newSelectedIndex >= categorySlots[currentCategoryIndex].Count)
        {
            return; 
        }

        if (direction == 1 && (selectedSlotIndex + 1) % inventoryWidth == 0)
        {
            return; // 오른쪽 끝에서 더 이동하지 않음
        }
        else if (direction == -1 && selectedSlotIndex % inventoryWidth == 0)
        {
            return; // 왼쪽 끝에서 더 이동하지 않음
        }

        selectedSlotIndex = newSelectedIndex;
        UpdateSelectionImage();
    }

    // 슬롯 선택 이미지 갱신
    void UpdateSelectionImage()
    {
        var slots = categorySlots[currentCategoryIndex];
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetSelected(i == selectedSlotIndex);
        }
    }

    // 스크롤을 아래로 이동
    void ScrollDown()
    {
        Vector2 newPosition = scrollRect.content.anchoredPosition;
        newPosition.y += 134; 
        scrollRect.content.anchoredPosition = newPosition;
    }

    // 스크롤을 위로 이동
    void ScrollUp()
    {
        Vector2 newPosition = scrollRect.content.anchoredPosition;
        newPosition.y -= 134; 
        scrollRect.content.anchoredPosition = newPosition;
    }

    // 현재 선택된 슬롯으로 스크롤 이동
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

    // 슬롯을 생성하여 패널에 추가
    List<InventorySlot> CreateSlots(Transform panel, int slotCount)
    {
        List<InventorySlot> slots = new List<InventorySlot>();

        for (int i = 0; i < slotCount; i++)
        {
            slots.Add(AddSlot(panel));
        }

        return slots;
    }

    // 슬롯 추가 함수
    InventorySlot AddSlot(Transform panel)
    {
        GameObject newSlotObject = Instantiate(slotPrefab, panel);
        InventorySlot slot = newSlotObject.GetComponent<InventorySlot>();

        Button button = newSlotObject.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(slot.OnClick);
        }

        return slot; 
    }

    // 새 슬롯을 현재 카테고리에 추가
    void AddSlot(int categoryIndex)
    {
        InventorySlot newSlot = AddSlot(categoryPanels[categoryIndex]);
        if (newSlot != null)
        {
            categorySlots[categoryIndex].Add(newSlot);
            UpdateSelectionImage();
            ScrollToSelectedSlot();
        }
    }

    // 슬롯 클릭 이벤트 처리
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

    // 플레이어 인벤토리와 슬롯 UI 업데이트
    public void UpdateInventoryUI()
    {
        Item[] playerItems = playerInventory.playerItemList;
        int slotIndex = 0;

        var slots = categorySlots[currentCategoryIndex];

        for (int i = 0; i < playerItems.Length; i++)
        {
            if (playerItems[i] != null && playerItems[i].itemCount > 0)
            {
                if (slotIndex < slots.Count)
                {
                    slots[slotIndex].SetItem(playerItems[i]);
                    slotIndex++;
                }
            }
        }

        for (int i = slotIndex; i < slots.Count; i++)
        {
            slots[i].ClearSlot();
        }
    }
}
