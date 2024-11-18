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
    int[] slotCounts = { 10, 5, 1, 1 }; 

    for (int i = 0; i < EquipmentPanels.Count; i++)
    {
        Transform panel = EquipmentPanels[i];
        for (int j = 0; j < slotCounts[i]; j++)
        {
            GameObject slotObj = Instantiate(EquipmentslotPrefab, panel);
            InventorySlot slot = slotObj.GetComponent<InventorySlot>();
            if (slot != null)
            {
                slot.categoryId = i;
            }
        }
    }
}

}