using System.Collections.Generic;
using MinD.Enums;
using MinD.Runtime.Entity;
using MinD.Runtime.UI;
using MinD.SO.Item;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public List<Transform> EquipmentPanels; // 각 카테고리별 패널 (Talisman, Tool, Protection, Weapon)
    public GameObject EquipmentSlotPrefab;

    private InventoryMenu _inventoryMenu;
    private PlayerInventoryHandler playerInventory;
    private int currentPanelIndex = -1; // (-1: 인벤토리 모드)
    private int currentSlotIndex = 0;
    private int columns = 5;
    
    public bool isInteractingWithEquipmentPanel = false;
    public bool isEquipmentPanelActive = true;
    private bool isCreateEquipmentSlots = false;

    void OnEnable()
    {
        _inventoryMenu = FindObjectOfType<InventoryMenu>();
        playerInventory = FindObjectOfType<PlayerInventoryHandler>();
        if (!isCreateEquipmentSlots)
        {
            CreateEquipmentSlots();
        }
        UpdateSelectedSlot(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleAllEquipmentPanels();
        }
    }
    
    private void ToggleAllEquipmentPanels()
    {
        if (isInteractingWithEquipmentPanel)
        {
            return;
        }

        isEquipmentPanelActive = !isEquipmentPanelActive;

        foreach (Transform panel in EquipmentPanels)
        {
            panel.gameObject.SetActive(isEquipmentPanelActive);
        }

        if (!isEquipmentPanelActive)
        {
            isInteractingWithEquipmentPanel = false;
            currentPanelIndex = -1;
            
        }
        else
        {
            if (currentPanelIndex == -1)
            {
                currentPanelIndex = -1;
            }

            ResetSlotIndex();
            UpdateSelectedSlot(false);
        }
    }
    public void CreateEquipmentSlots()
    {
        int[] slotCounts = { 1, 1, 5, 10 };
        int[] categoryIds = { 2, 3, 0, 1 };

        for (int i = 0; i < EquipmentPanels.Count; i++)
        {
            Transform panel = EquipmentPanels[i];
            for (int j = 0; j < slotCounts[i]; j++)
            {
                GameObject slotObj = Instantiate(EquipmentSlotPrefab, panel);
                EquipmentSlot slot = slotObj.GetComponent<EquipmentSlot>();
                if (slot != null)
                {
                    slot.categoryId = categoryIds[i];
                    slot.slotIndex = j;
                }
            }
        }

        isCreateEquipmentSlots = true;
    }

    private void ResetSlotIndex()
    {
        if (currentPanelIndex >= 0 && currentPanelIndex < EquipmentPanels.Count)
        {
            int slotCount = EquipmentPanels[currentPanelIndex].childCount;
            currentSlotIndex = Mathf.Clamp(currentSlotIndex, 0, slotCount - 1);
        }
        else
        {
            currentSlotIndex = 0;
        }
    }

    private void UpdateSelectedSlot(bool activate = true)
    {
        if (currentPanelIndex < 0 || currentPanelIndex >= EquipmentPanels.Count) return;

        Transform currentPanel = EquipmentPanels[currentPanelIndex];
        int slotCount = currentPanel.childCount;

        currentSlotIndex = Mathf.Clamp(currentSlotIndex, 0, slotCount - 1);

        Transform selectedSlot = currentPanel.GetChild(currentSlotIndex);
        EquipmentSlot equipmentSlot = selectedSlot.GetComponent<EquipmentSlot>();

        if (equipmentSlot != null)
        {
            equipmentSlot.selectionImage.SetActive(activate);
        }
    }

    public void ClearSelectedSlot()
    {
        if (currentPanelIndex < 0 || currentPanelIndex >= EquipmentPanels.Count) return;

        Transform currentPanel = EquipmentPanels[currentPanelIndex];
        Transform selectedSlot = currentPanel.GetChild(currentSlotIndex);
        EquipmentSlot equipmentSlot = selectedSlot.GetComponent<EquipmentSlot>();

        if (equipmentSlot != null)
        {
            equipmentSlot.ClearSlot();
            playerInventory.UnequipEquipment((EquipmentSlots)equipmentSlot.slotIndex);
        }
    }

    public void UpdateEquipmentSlots()
    {
        if (playerInventory == null)
        {
            playerInventory = FindObjectOfType<PlayerInventoryHandler>();
        }

        for (int panelIndex = 0; panelIndex < EquipmentPanels.Count; panelIndex++)
        {
            Transform panel = EquipmentPanels[panelIndex];
            foreach (Transform slotTransform in panel.GetComponentsInChildren<Transform>(true))
            {
                EquipmentSlot equipmentSlot = slotTransform.GetComponent<EquipmentSlot>();
                if (equipmentSlot != null)
                {
                    UpdateSlot(equipmentSlot);
                }
            }
        }
    }


    private void UpdateSlot(EquipmentSlot slot)
    {
        Equipment equipment = null;

        // 슬롯의 카테고리와 인덱스에 따라 장비를 가져옴
        switch (slot.categoryId)
        {
            case 0: // Talisman
                if (slot.slotIndex < playerInventory.talismanSlots.Length)
                {
                    equipment = playerInventory.talismanSlots[slot.slotIndex];
                }
                break;

            case 1: // Tool
                if (slot.slotIndex < playerInventory.toolSlots.Length)
                {
                    equipment = playerInventory.toolSlots[slot.slotIndex];
                }
                break;

            case 2: // Protection
                if (slot.slotIndex == 0)
                {
                    equipment = playerInventory.protectionSlot;
                }
                break;

            case 3: // Weapon
                if (slot.slotIndex == 0)
                {
                    equipment = playerInventory.weaponSlot;
                }
                break;
        }

        if (equipment != null)
        {
            slot.UpdateSlot(equipment);
        }
        else
        {
            slot.ClearSlot();
        }
    }

}
