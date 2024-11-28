using MinD.SO.Item;
using UnityEngine;
using UnityEngine.UI;

namespace MinD.Runtime.UI
{
    public class InventorySlot : MonoBehaviour
    {
        public Image itemImage;
        public Text itemCountText;
        public GameObject selectionImage;

        private Item currentItem;
        private InventoryUI inventoryUI;

        public int categoryId; // 슬롯의 카테고리를 나타내는 변수 추가

        void Start()
        {
            inventoryUI = FindObjectOfType<InventoryUI>();
        }

        public void SetItem(Item item, int itemCategoryId)
        {
            if (categoryId != itemCategoryId) // 카테고리 ID가 일치하지 않으면 아이템을 설정하지 않음
            {
                ClearSlot();
                return;
            }

            currentItem = item;

            if (item != null)
            {
                itemImage.sprite = item.itemImage;
                itemImage.enabled = true;

                // 아이템 수가 2개 이상일 때만 카운트 표시
                if (item.itemCount >= 2)
                {
                    itemCountText.text = item.itemCount.ToString();
                    itemCountText.enabled = true; // 카운트 텍스트 활성화
                }
                else
                {
                    itemCountText.enabled = false; // 카운트 텍스트 비활성화
                }
            }
            else
            {
                ClearSlot(); // 아이템이 null인 경우 슬롯 비우기
            }
        }

        public void ClearSlot()
        {
            currentItem = null;
            itemImage.enabled = false;
            itemCountText.enabled = false;
            SetSelected(false);
        }

        public void SetSelected(bool isSelected)
        {
            if (selectionImage != null)
            {
                selectionImage.SetActive(isSelected);
            }
        }

        public void OnClick()
        {
            if (inventoryUI != null)
            {
                inventoryUI.OnSlotClicked(this); // 클릭된 슬롯을 InventoryUI로 전달

                var item = GetCurrentItem();
                if (item != null && item.itemCount > 0) // 아이템이 있을 때만 패널 표시
                {
                    int slotIndex = inventoryUI.GetSlotIndex(this); // 슬롯의 인덱스를 가져오는 메서드 호출
                    inventoryUI.itemActionPanel.ShowPanel(item); // 아이템과 슬롯 인덱스를 전달
                }
            }
        }

        public Item GetCurrentItem()
        {
            return currentItem; // 현재 슬롯의 아이템 반환
        }
    }
}
