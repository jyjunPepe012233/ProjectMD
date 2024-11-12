using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MinD.Runtime.Entity;
using MinD.SO.Item;

public class InventoryUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform inventoryPanel;
    private List<InventorySlot> slots = new List<InventorySlot>();

    private PlayerInventoryHandler playerInventory; 

    void Start()
    {
        playerInventory = FindObjectOfType<Player>().inventory;

        CreateSlots(25);
        UpdateInventoryUI();
    }

    void CreateSlots(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
            GameObject newSlotObject = Instantiate(slotPrefab, inventoryPanel);
            InventorySlot slot = newSlotObject.GetComponent<InventorySlot>();
            slots.Add(slot);
        }
    }
    
    public void UpdateInventoryUI()
    {
        Item[] playerItems = playerInventory.playerItemList;
        int slotIndex = 0; 

        for (int i = 0; i < playerItems.Length; i++)
        {
            if (playerItems[i] != null && playerItems[i].itemCount > 0)
            {
                if (slotIndex < slots.Count)
                {
                    slots[slotIndex].SetItem(playerItems[i]); 
                    slotIndex++; 
                }
            }
        }

        for (int i = slotIndex; i < slots.Count; i++)
        {
            slots[i].ClearSlot();
        }
    }

}