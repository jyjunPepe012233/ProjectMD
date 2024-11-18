using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MinD.Runtime.Entity;
using MinD.SO.Item;
using UnityEngine.EventSystems;

namespace MinD.Runtime.UI {

    public class InventoryUI : MonoBehaviour
    {
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

        void DisableScrollbarNavigation()
        {
            if (scrollRect.verticalScrollbar != null)
            {
                var nav = scrollRect.verticalScrollbar.navigation;
                nav.mode = Navigation.Mode.None; // 네비게이션 비활성화
                scrollRect.verticalScrollbar.navigation = nav;
            }

            if (scrollRect.horizontalScrollbar != null)
            {
                var nav = scrollRect.horizontalScrollbar.navigation;
                nav.mode = Navigation.Mode.None; // 네비게이션 비활성화
                scrollRect.horizontalScrollbar.navigation = nav;
            }
        }
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
            }
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

            // Enter 키로 선택된 슬롯 처리
            if (Input.GetKeyDown(KeyCode.Return))
            {
                var selectedSlot = categorySlots[currentCategoryIndex][selectedSlotIndex];
                OnSlotSelected(selectedSlot); // 슬롯 선택 로직 실행

                selectedSlot.OnClick(); // 슬롯 클릭 메서드 직접 호출
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

        void OnSlotSelected(InventorySlot selectedSlot)
        {
            // 선택된 슬롯에 대한 처리
            Debug.Log("선택된 슬롯: " + selectedSlot);
            // 여기서 필요한 처리를 추가할 수 있습니다.
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
