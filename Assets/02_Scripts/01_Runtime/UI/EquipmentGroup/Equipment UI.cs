using System.Collections.Generic;
using MinD.Runtime.UI;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public List<Transform> EquipmentPanels; // Tools, Talisman, Weapon, Protection 패널 순서대로
    public GameObject EquipmentslotPrefab;

    private InventoryUI inventoryUI; 
    
    public void Start()
    {
        inventoryUI = FindObjectOfType<InventoryUI>();
        
        if (inventoryUI != null)
        {
            CreateEquipmentSlots();
            
        }
    }

    public void CreateEquipmentSlots()
    {
        int[] slotCounts = { 5, 10, 1, 1 }; // Talisman, Tool, Protection, Weapon 순서

        for (int i = 0; i < EquipmentPanels.Count; i++)
        {
            Transform panel = EquipmentPanels[i];
            for (int j = 0; j < slotCounts[i]; j++)
            {
                GameObject slotObj = Instantiate(EquipmentslotPrefab, panel);
                EquipmentSlot slot = slotObj.GetComponent<EquipmentSlot>();
                if (slot != null)
                {
                    slot.categoryId = i; // 카테고리 ID 할당 (0 = Talisman, 1 = Tool, 2 = Protection, 3 = Weapon)
                }
            }
        }
    }


}