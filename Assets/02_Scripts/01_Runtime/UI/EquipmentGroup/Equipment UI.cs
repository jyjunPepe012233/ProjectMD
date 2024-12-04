using System.Collections.Generic;
using MinD.Runtime.UI;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public List<Transform> EquipmentPanels; // 각 카테고리별 패널 (Talisman, Tool, Protection, Weapon)
    public GameObject EquipmentSlotPrefab;

    private InventoryUI inventoryUI; // 인벤토리 UI 참조
    private int currentPanelIndex = -1; // 현재 활성화된 패널 인덱스 (-1: 인벤토리 모드)
    private int currentSlotIndex = 0; // 현재 선택된 슬롯 인덱스
    private int columns = 5; // 슬롯의 열 수
    public bool isInteractingWithEquipmentPanel = false; // 장착 패널 상호작용 여부

    void Start()
    {
        inventoryUI = FindObjectOfType<InventoryUI>();
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
        // 현재 패널의 선택 이미지 비활성화
        if (currentPanelIndex >= 0 && currentPanelIndex < EquipmentPanels.Count)
        {
            UpdateSelectedSlot(false); // 현재 패널의 선택 이미지 비활성화
        }

        // 다음 패널 인덱스로 순환 (패널 전환)
        currentPanelIndex++;
        if (currentPanelIndex > EquipmentPanels.Count - 1) // 마지막 패널 이후에는 인벤토리로
        {
            currentPanelIndex = -1; // 인벤토리 모드로 전환
        }

        // 상호작용 상태 업데이트
        isInteractingWithEquipmentPanel = (currentPanelIndex != -1);

        if (isInteractingWithEquipmentPanel) // 장비 패널 활성화 상태
        {
            // 슬롯 인덱스 초기화 (현재 패널의 첫 번째 슬롯 선택)
            ResetSlotIndex();

            // 현재 패널 활성화
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
        if (currentPanelIndex < 0 || currentPanelIndex >= EquipmentPanels.Count) return; // 잘못된 인덱스 예외 처리

        Transform currentPanel = EquipmentPanels[currentPanelIndex];
        int slotCount = currentPanel.childCount;

        UpdateSelectedSlot(false); // 현재 선택된 슬롯 비활성화

        currentSlotIndex += direction; // 인덱스 이동

        // 경계 체크 (가로 방향)
        if (currentSlotIndex % columns < 0) // 왼쪽 경계
        {
            currentSlotIndex += columns; // 오른쪽 끝으로 이동
        }
        else if (currentSlotIndex % columns >= columns) // 오른쪽 경계
        {
            currentSlotIndex -= columns; // 왼쪽 끝으로 이동
        }

        // 경계 체크 (세로 방향)
        if (currentSlotIndex < 0) // 위쪽 경계
        {
            currentSlotIndex = slotCount - 1; // 아래쪽으로 이동
        }
        else if (currentSlotIndex >= slotCount) // 아래쪽 경계
        {
            currentSlotIndex = 0; // 위쪽으로 이동
        }

        UpdateSelectedSlot(true); // 현재 선택된 슬롯 활성화
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
            currentSlotIndex = Mathf.Clamp(currentSlotIndex, 0, slotCount - 1); // 현재 패널 슬롯 범위 내로 제한
        }
        else
        {
            currentSlotIndex = 0; // 기본값
        }
    }

    private void UpdateSelectedSlot(bool activate = true)
    {
        if (currentPanelIndex < 0 || currentPanelIndex >= EquipmentPanels.Count) return; // 패널이 활성화되지 않은 경우 무시

        Transform currentPanel = EquipmentPanels[currentPanelIndex];
        int slotCount = currentPanel.childCount;

        // 슬롯 인덱스가 초과하지 않도록 제한
        currentSlotIndex = Mathf.Clamp(currentSlotIndex, 0, slotCount - 1);

        Transform selectedSlot = currentPanel.GetChild(currentSlotIndex);
        EquipmentSlot equipmentSlot = selectedSlot.GetComponent<EquipmentSlot>();

        if (equipmentSlot != null)
        {
            equipmentSlot.selectionImage.SetActive(activate); // 선택 이미지 활성화 또는 비활성화
        }
    }

    private void ClearSelectedSlot()
    {
        if (currentPanelIndex < 0 || currentPanelIndex >= EquipmentPanels.Count) return; // 유효하지 않은 경우 무시

        Transform currentPanel = EquipmentPanels[currentPanelIndex];
        Transform selectedSlot = currentPanel.GetChild(currentSlotIndex);
        EquipmentSlot equipmentSlot = selectedSlot.GetComponent<EquipmentSlot>();

        if (equipmentSlot != null)
        {
            equipmentSlot.ClearSlot(); // 슬롯의 정보를 지우고 비활성화
        }
    }

    private void TriggerSlotAction()
    {
        if (currentPanelIndex < 0 || currentPanelIndex >= EquipmentPanels.Count) return; // 패널이 활성화되지 않은 경우 무시

        Transform currentPanel = EquipmentPanels[currentPanelIndex];
        Transform selectedSlot = currentPanel.GetChild(currentSlotIndex);
        EquipmentSlot equipmentSlot = selectedSlot.GetComponent<EquipmentSlot>();

        if (equipmentSlot != null)
        {
            equipmentSlot.OnSlotClicked(); // 슬롯 클릭 이벤트 호출
        }
    }
}
