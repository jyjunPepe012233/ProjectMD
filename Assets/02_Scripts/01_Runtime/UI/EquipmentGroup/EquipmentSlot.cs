using MinD.SO.Item;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    public Image itemImage; // 아이템 이미지를 표시하는 UI
    public Text itemCountText; // 아이템 개수를 표시하는 UI
    public GameObject selectionImage; // 슬롯 선택 이미지를 활성화/비활성화
    public int categoryId; // 슬롯의 카테고리 ID
    public int slotIndex; // 슬롯의 인덱스

    [SerializeField] private Item currentItem; // 현재 슬롯에 담긴 아이템 (없으면 null)

    /// <summary>
    /// 슬롯에 새로운 아이템을 업데이트합니다.
    /// </summary>
    public void UpdateSlot(Item item)
    {
        if (item == null)
        {
            ClearSlot(); // null인 경우 슬롯 비우기
            return;
        }

        currentItem = item;
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

    /// <summary>
    /// 슬롯을 초기화하여 아이템 정보를 제거합니다.
    /// </summary>
    public void ClearSlot()
    {
        currentItem = null;
        itemImage.gameObject.SetActive(false);
        itemCountText.gameObject.SetActive(false);
    }

    /// <summary>
    /// 슬롯 클릭 시 동작을 정의합니다.
    /// </summary>
    public void OnSlotClicked()
    {
        if (currentItem != null)
        {
            Debug.Log($"슬롯 클릭: 카테고리 ID: {categoryId}, 슬롯 인덱스: {slotIndex}, 아이템 이름: {currentItem.itemName}");
        }
        else
        {
            Debug.Log($"슬롯 클릭: 카테고리 ID: {categoryId}, 슬롯 인덱스: {slotIndex}, 빈 슬롯");
        }
    }
}