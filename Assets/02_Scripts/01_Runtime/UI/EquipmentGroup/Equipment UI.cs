using System.Collections.Generic;
using MinD.Enums;
using MinD.Runtime.Entity;
using MinD.Runtime.UI;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public List<Transform> EquipmentPanels; // 각 카테고리별 패널 (Talisman, Tool, Protection, Weapon)
    public GameObject EquipmentSlotPrefab;

    private InventoryUI inventoryUI; // 인벤토리 UI 참조
    private PlayerInventoryHandler playerInventory; // 플레이어 인벤토리 핸들러 참조
    private int currentPanelIndex = -1; // 현재 활성화된 패널 인덱스 (-1: 인벤토리 모드)
    private int currentSlotIndex = 0; // 현재 선택된 슬롯 인덱스
    private int columns = 5; // 슬롯의 열 수
    public bool isInteractingWithEquipmentPanel = false; // 장착 패널 상호작용 여부

    void Start()
    {
        inventoryUI = FindObjectOfType<InventoryUI>();
        playerInventory = FindObjectOfType<PlayerInventoryHandler>();
        CreateEquipmentSlots();
        UpdateSelectedSlot(false); // 기본 선택 슬롯 비활성화
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) // C 키로 패널 전환
        {
            ToggleEquipmentPanel();
        }

        if (isInteractingWithEquipmentPanel) // 패널이 활성화된 경우
        {
            HandleSlotSelection();

            if (Input.GetKeyDown(KeyCode.Return)) // Enter 키를 눌렀을 때
            {
                TriggerSlotAction();
            }

            if (Input.GetKeyDown(KeyCode.E)) // E 키로 슬롯 해제
            {
                ClearSelectedSlot();
            }
        }
    }

    private void ToggleEquipmentPanel()
    {
        if (currentPanelIndex >= 0 && currentPanelIndex < EquipmentPanels.Count)
        {
            UpdateSelectedSlot(false); // 현재 패널의 선택 이미지 비활성화
        }

        currentPanelIndex++;
        if (currentPanelIndex > EquipmentPanels.Count - 1) // 마지막 패널 이후에는 인벤토리로
        {
            currentPanelIndex = -1; // 인벤토리 모드로 전환
        }

        isInteractingWithEquipmentPanel = (currentPanelIndex != -1);

        if (isInteractingWithEquipmentPanel) // 장비 패널 활성화 상태
        {
            ResetSlotIndex();
            UpdateSelectedSlot(true); // 선택 이미지 활성화
            inventoryUI.DisableSelectionImage(); // 인벤토리 선택 이미지 비활성화
        }
        else // 인벤토리 모드로 돌아감
        {
            inventoryUI.EnableSelectionImage(0); // 인벤토리의 첫 번째 슬롯 선택 이미지 활성화
        }
    }

    private void HandleSlotSelection()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveSelection(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveSelection(1);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveSelection(-columns); // 위로 이동
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveSelection(columns); // 아래로 이동
        }
    }

    private void MoveSelection(int direction)
    {
        if (currentPanelIndex < 0 || currentPanelIndex >= EquipmentPanels.Count) return;

        Transform currentPanel = EquipmentPanels[currentPanelIndex];
        int slotCount = currentPanel.childCount;

        UpdateSelectedSlot(false);
        currentSlotIndex += direction;

        if (currentSlotIndex % columns < 0) currentSlotIndex += columns;
        else if (currentSlotIndex % columns >= columns) currentSlotIndex -= columns;

        if (currentSlotIndex < 0) currentSlotIndex = slotCount - 1;
        else if (currentSlotIndex >= slotCount) currentSlotIndex = 0;

        UpdateSelectedSlot(true);
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

        // 슬롯 인덱스가 초과하지 않도록 제한
        currentSlotIndex = Mathf.Clamp(currentSlotIndex, 0, slotCount - 1);

        Transform selectedSlot = currentPanel.GetChild(currentSlotIndex);
        EquipmentSlot equipmentSlot = selectedSlot.GetComponent<EquipmentSlot>();

        if (equipmentSlot != null)
        {
            equipmentSlot.selectionImage.SetActive(activate);
        }
    }

    private void ClearSelectedSlot()
    {
        if (currentPanelIndex < 0 || currentPanelIndex >= EquipmentPanels.Count) return;

        Transform currentPanel = EquipmentPanels[currentPanelIndex];
        Transform selectedSlot = currentPanel.GetChild(currentSlotIndex);
        EquipmentSlot equipmentSlot = selectedSlot.GetComponent<EquipmentSlot>();

        if (equipmentSlot != null)
        {
            equipmentSlot.ClearSlot();
            playerInventory.UnequipEquipment((EquipmentSlots)equipmentSlot.slotIndex);
            Debug.Log($"Slot cleared: Category {equipmentSlot.categoryId}, Index {equipmentSlot.slotIndex}");
        }
    }

    private void TriggerSlotAction()
    {
        if (currentPanelIndex < 0 || currentPanelIndex >= EquipmentPanels.Count) return;

        Transform currentPanel = EquipmentPanels[currentPanelIndex];
        Transform selectedSlot = currentPanel.GetChild(currentSlotIndex);
        EquipmentSlot equipmentSlot = selectedSlot.GetComponent<EquipmentSlot>();

        if (equipmentSlot != null)
        {
            // Add logic for equipping items from inventory
            Debug.Log($"Slot action triggered: Category {equipmentSlot.categoryId}, Index {equipmentSlot.slotIndex}");
        }
    }
}
