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

    public void UpdateSlot(Item item)
    {
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
            itemImage.gameObject.SetActive(false);
            itemCountText.gameObject.SetActive(false);
        }
    }

    public void OnSlotClicked()
    {
        Debug.Log($"장착슬롯 클릭 categoryId: {categoryId}, slotIndex: {slotIndex}");
    }
}