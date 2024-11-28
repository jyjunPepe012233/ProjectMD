using MinD.SO.Item;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    public Image itemImage;
    public Text itemCountText;
    public int categoryId;

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
            else
            {
                itemCountText.gameObject.SetActive(false);
            }
        }
        else
        {
            itemImage.gameObject.SetActive(false);
            itemCountText.gameObject.SetActive(false);
        }
    }
}