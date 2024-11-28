using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MinD.Runtime.Entity;
using MinD.SO.Item;
using UnityEngine.EventSystems;

namespace MinD.Runtime.UI {

    public class InventoryUI : MonoBehaviour
    {
        public ItemActionPanel itemActionPanel; // 아이템 액션 패널

        public Text itemNameText; // 아이템 이름 표시
        public Text itemDescriptionText; // 아이템 설명 표시

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
                List<InventorySlot> slots = CreateSlots(categoryPanels[i], 25, i);
                categorySlots.Add(slots);
            }

            UpdateCategory();
            UpdateInventoryUI();
            UpdateSelectionImage();

            inventoryPanel.SetActive(false);
            UpdateItemDetails(); // 초기 상태에서도 아이템 정보 표시
        }

        void MaintainFocus()
        {
            if (isInventoryActive && EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(categorySlots[currentCategoryIndex][selectedSlotIndex].gameObject);
            }
        }
        void Update()
        {
            HandleInput();
            MaintainFocus();
            // Q 키를 눌렀을 때 패널을 숨김
        }

        void ToggleInventory()
        {
            isInventoryActive = !isInventoryActive;
            inventoryPanel.SetActive(isInventoryActive);

            if (isInventoryActive)
            {
                selectedSlotIndex = 0; // 인벤토리를 열 때 0번 슬롯 선택
                UpdateCategory();      // 현재 카테고리 활성화
                UpdateSelectionImage(); // 선택 이미지 업데이트
                UpdateItemDetails();   // 아이템 이름과 설명 업데이트
            }
            else
            {
                itemNameText.text = ""; // 인벤토리를 닫을 때 이름 초기화
                itemDescriptionText.text = ""; // 설명 초기화

                // 액션 패널 비활성화
                itemActionPanel.HidePanel();
            }
        }


        void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ToggleInventory();
            }

            if (!isInventoryActive) return; // 인벤토리가 활성화되지 않으면 아무것도 하지 않음

            // 인벤토리 액션 패널이 열려있지 않을 때만 입력 처리
            if (!itemActionPanel.IsActive())
            {
                // Q 키를 눌렀을 때 패널을 숨김
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    itemActionPanel.HidePanel(); // 패널 숨기기
                    return; // 패널을 숨기면 추가 입력 처리 중단
                }

                // 슬롯 이동 관련 입력 처리
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

                // Enter 키를 눌렀을 때 패널을 띄움
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    var selectedSlot = categorySlots[currentCategoryIndex][selectedSlotIndex];
                    var item = selectedSlot.GetCurrentItem();
                    if (item != null && item.itemCount > 0) // 아이템이 있을 때만 패널 표시
                    {
                        itemActionPanel.ShowPanel(item); // 아이템과 슬롯 인덱스를 전달
                    }
                }
            }
        }
        void ChangeCategory(int direction)
        {
            currentCategoryIndex = (currentCategoryIndex + direction + categoryPanels.Count) % categoryPanels.Count;

            UpdateCategory();

            // 스크롤 초기화
            scrollRect.content.anchoredPosition = new Vector2(scrollRect.content.anchoredPosition.y, 0);

            // 선택된 슬롯 정보 업데이트
            UpdateItemDetails();

            // EventSystem으로 선택된 슬롯 포커싱
            FocusSelectedSlot();
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
            UpdateSelectionImage(); // 카테고리 변경 시 선택 이미지 업데이트
            ScrollToSelectedSlot();
            scrollRect.normalizedPosition = new Vector2(scrollRect.normalizedPosition.y, 0);
        }

        // OnSlotSelected 메서드 수정
        void OnSlotSelected(InventorySlot selectedSlot)
        {
            int selectedIndex = categorySlots[currentCategoryIndex].IndexOf(selectedSlot);
            if (selectedIndex >= 0)
            {
                itemActionPanel.ShowPanel(selectedSlot.GetCurrentItem()); // 슬롯 인덱스 전달
            }
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

            // 선택 이미지 업데이트
            UpdateSelectionImage();
            // 아이템 정보 업데이트
            UpdateItemDetails();
        }

        void UpdateSelectionImage()
        {
            var slots = categorySlots[currentCategoryIndex];
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].SetSelected(i == selectedSlotIndex);
            }

            // EventSystem으로 선택된 슬롯 포커싱
            FocusSelectedSlot();
        }

        void UpdateItemDetails()
        {
            var selectedSlot = categorySlots[currentCategoryIndex][selectedSlotIndex];
            var item = selectedSlot.GetCurrentItem();

            if (item != null)
            {
                itemNameText.text = item.itemName; // 아이템 이름 업데이트
                itemDescriptionText.text = item.itemDescription; // 아이템 설명 업데이트
            }
            else
            {
                itemNameText.text = ""; // 아이템이 없을 때 이름 초기화
                itemDescriptionText.text = ""; // 아이템이 없을 때 설명 초기화
            }
        }

        void ScrollDown()
        {
            if (scrollRect.content.anchoredPosition.y >= 65 * GetCurrentCategorySlotRange()-1) 
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

        public List<InventorySlot> CreateSlots(Transform panel, int slotCount, int categoryId)
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

            return slot;
        }

        public void OnSlotClicked(InventorySlot clickedSlot)
        {
            int clickedIndex = categorySlots[currentCategoryIndex].IndexOf(clickedSlot);
            if (clickedIndex >= 0)
            {
                selectedSlotIndex = clickedIndex;
                UpdateSelectionImage();  // 클릭 시 선택 이미지 업데이트
                UpdateItemDetails();     // 아이템 정보 업데이트
            }
        }

        public int GetSlotIndex(InventorySlot slot)
        {
            return categorySlots[currentCategoryIndex].IndexOf(slot);
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
        void FocusSelectedSlot()
        {
            if (categorySlots[currentCategoryIndex].Count > selectedSlotIndex)
            {
                GameObject selectedSlotObject = categorySlots[currentCategoryIndex][selectedSlotIndex].gameObject;
                EventSystem.current.SetSelectedGameObject(selectedSlotObject);
            }
        }
    }
}
