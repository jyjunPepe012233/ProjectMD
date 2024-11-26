using MinD.SO.Item;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    public Image itemImage; // 아이템 이미지를 표시할 UI Image
    public Text itemCountText; // 아이템 개수를 표시할 UI Text
    public int categoryId; // 아이템 카테고리 ID

    public void UpdateSlot(Item item)
    {
        if (item != null)
        {
            itemImage.sprite = item.itemImage; // 아이템 이미지 설정
            itemImage.gameObject.SetActive(true); // 이미지 활성화
            
            // 아이템 개수가 1보다 크면 텍스트를 표시
            if (item.itemCount > 1)
            {
                itemCountText.text = item.itemCount.ToString(); // 아이템 개수 설정
                itemCountText.gameObject.SetActive(true); // 개수 텍스트 활성화
            }
            else
            {
                itemCountText.gameObject.SetActive(false); // 개수 텍스트 비활성화
            }
        }
        else
        {
            itemImage.gameObject.SetActive(false); // 이미지 비활성화
            itemCountText.gameObject.SetActive(false); // 개수 텍스트 비활성화
        }
    }
}