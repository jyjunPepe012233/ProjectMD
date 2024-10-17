using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image itemImage;    // 아이템 이미지
    public Text itemCountText; // 아이템 개수

    private Item currentItem;  // 현재 슬롯에 아이템

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
    }

    // 슬롯 클릭 시
    public void OnClick()
    {
        if (currentItem != null)
        {
            // 아이템 정보를 UI에 표시
            Debug.Log($"아이템 이름: {currentItem.itemName}, 설명: {currentItem.itemDescription}");
        }
    }
}