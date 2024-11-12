using MinD.SO.Item;
using UnityEngine;
using UnityEngine.UI;

namespace MinD.Runtime.UI {

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
            inventoryUI.OnSlotClicked(this);
        }
    }
}

}