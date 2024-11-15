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
        // 각 패널에 맞는 슬롯 수 지정
        int[] slotCounts = { 10, 5, 1, 1 }; 

        for (int i = 0; i < EquipmentPanels.Count; i++)
        {
            if (i < slotCounts.Length)
            {
                List<InventorySlot> slots = inventoryUI.CreateSlots(EquipmentPanels[i], slotCounts[i], i);
                
            }
        }
    }
}