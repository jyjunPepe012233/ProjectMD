using MinD.SO.Item;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    public Image itemImage;
    public Text itemCountText;
    public GameObject selectionImage;
    public int categoryId;
    public int slotIndex;

    [SerializeField]private Item currentItem; // 슬롯에 담긴 현재 아이템 (없으면 null)

    public void UpdateSlot(Item item)
    {
        currentItem = item; // 슬롯의 아이템 업데이트

        if (item != null)
        {
            itemImage.sprite = item.itemImage;
            itemImage.gameObject.SetActive(true);

            if (item.itemCount > 1)
            {
                itemCountText.text = item.itemCount.ToString();
                itemCountText.gameObject.SetActive(true);
            }
            else if (item.itemCount == 1)
            {
                itemCountText.gameObject.SetActive(false);
            }
            else
            {
                itemImage.gameObject.SetActive(false);
                itemCountText.gameObject.SetActive(false);
            }
        }
        else
        {
            ClearSlot(); // 슬롯을 비웁니다.
        }
    }

    public void ClearSlot()
    {
        currentItem = null; // 아이템 정보 초기화
        itemImage.gameObject.SetActive(false);
        itemCountText.gameObject.SetActive(false);
    }

    public void OnSlotClicked()
    {
        Debug.Log($"장착슬롯 클릭 categoryId: {categoryId}, slotIndex: {slotIndex}");
    }
}
