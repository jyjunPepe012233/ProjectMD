using System.Collections.Generic;
using MinD.Runtime.UI;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public List<Transform> EquipmentPanels; // 각 카테고리별 패널 (Talisman, Tool, Protection, Weapon)
    public GameObject EquipmentSlotPrefab;

    private InventoryUI inventoryUI;

    void Start()
    {
        inventoryUI = FindObjectOfType<InventoryUI>();
        CreateEquipmentSlots();
    }

    public void CreateEquipmentSlots()
    {
        int[] slotCounts = { 1, 1, 5, 10 }; // 각 장비 슬롯 개수
        int[] categoryIds = { 2, 3, 0, 1 }; // 슬롯의 카테고리 ID 배열
        
        for (int i = 0; i < EquipmentPanels.Count; i++)
        {
            Transform panel = EquipmentPanels[i];
            for (int j = 0; j < slotCounts[i]; j++)
            {
                GameObject slotObj = Instantiate(EquipmentSlotPrefab, panel);
                EquipmentSlot slot = slotObj.GetComponent<EquipmentSlot>();
                if (slot != null)
                {
                    slot.categoryId = categoryIds[i]; // 카테고리 ID 설정
                    slot.slotIndex = j; // slotIndex 설정
                }
            }
        }
    }
}