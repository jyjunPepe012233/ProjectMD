using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image itemImage; // 아이템 이미지
    public Text itemCountText; // 아이템 개수
    public GameObject selectionImage; // 선택 이미지

    private Item currentItem; // 현재 슬롯에 있는 아이템
    private InventoryUI inventoryUI; // 인벤토리 UI 참조

    public int slotId; // 슬롯 ID
    public int categoryId; // 카테고리 ID

    void Start()
    {
        inventoryUI = FindObjectOfType<InventoryUI>(); // InventoryUI 참조 가져오기
    }

    public void SetItem(Item item)
    {
        currentItem = item;

        if (item != null && item.itemCount > 0)
        {
            itemImage.sprite = item.itemImage;
            itemImage.enabled = true;
            itemCountText.text = item.itemCount.ToString();
            itemCountText.enabled = true;
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        currentItem = null;
        itemImage.enabled = false;
        itemCountText.enabled = false;
        SetSelected(false); // 슬롯을 비우면 선택 이미지를 숨김
    }

    public void SetSelected(bool isSelected)
    {
        if (selectionImage != null)
        {
            selectionImage.SetActive(isSelected);
        }
    }

    // 슬롯 클릭 시
    public void OnClick()
    {
        if (inventoryUI != null)
        {
            inventoryUI.OnSlotClicked(this);
        }
    }
}