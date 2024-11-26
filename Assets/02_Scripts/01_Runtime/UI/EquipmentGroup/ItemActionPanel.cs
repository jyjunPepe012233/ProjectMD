using System.Collections;
using System.Linq;
using MinD.Enums;
using MinD.Runtime.Entity;
using MinD.SO.Item;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace MinD.Runtime.UI
{
    public class ItemActionPanel : MonoBehaviour
    {
        public GameObject panel; // 패널 오브젝트
        public Button wearButton; // 착용 버튼
        public Button dropButton; // 버리기 버튼
        public Button destroyButton; // 파기 버튼

        private Item currentItem; // 현재 아이템
        private PlayerInventoryHandler playerInventoryHandler; // 플레이어 인벤토리 핸들러
        private InventoryUI inventoryUI; // 인벤토리 UI

        public Button[] actionButtons; // 버튼 리스트
        public int selectedButtonIndex = 0; // 현재 선택된 버튼 인덱스

        private int equippedTalismanCount => playerInventoryHandler.talismanSlots.Count(t => t != null); // 착용된 탈리스만 수

        void Start()
        {
            panel.SetActive(false); // 시작할 때 패널 비활성화
            actionButtons = new Button[] { wearButton, dropButton, destroyButton }; // 버튼 배열 초기화

            // 버튼 클릭 이벤트 등록
            wearButton.onClick.AddListener(OnEquipButtonClicked);
            dropButton.onClick.AddListener(OnDropButtonClicked);
            destroyButton.onClick.AddListener(OnDestroyButtonClicked);

            // PlayerInventoryHandler 인스턴스 찾기
            playerInventoryHandler = FindObjectOfType<PlayerInventoryHandler>();
            if (playerInventoryHandler == null)
            {
                Debug.LogError("PlayerInventoryHandler not found in the scene!");
            }

            // InventoryUI 인스턴스 찾기
            inventoryUI = FindObjectOfType<InventoryUI>();
            if (inventoryUI == null)
            {
                Debug.LogError("InventoryUI not found in the scene!");
            }
        }

        void Update()
        {
            if (panel.activeSelf) // 패널이 열려 있을 때만 입력 처리
            {
                HandleInput(); // 입력 처리
            }
        }

        private void HandleInput()
        {
            // Q 키 입력 처리로 패널을 닫음
            if (Input.GetKeyDown(KeyCode.Q))
            {
                HidePanel(); // 패널 숨기기
                return; // 입력 처리 종료
            }

            // 방향키 입력 처리
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ChangeSelection(-1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ChangeSelection(1);
            }

            // Enter 키 입력 처리
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(HandleButtonClickAfterDelay()); // 버튼 클릭을 지연 처리
            }
        }

        private IEnumerator HandleButtonClickAfterDelay()
        {
            yield return null; // 다음 프레임까지 대기

            if (selectedButtonIndex >= 0 && selectedButtonIndex < actionButtons.Length)
            {
                actionButtons[selectedButtonIndex].onClick.Invoke(); // 현재 선택된 버튼 클릭
            }
        }

        private void ChangeSelection(int direction)
        {
            // 선택된 버튼 인덱스 변경
            selectedButtonIndex += direction;
            if (selectedButtonIndex < 0)
            {
                selectedButtonIndex = actionButtons.Length - 1; // 위로 스크롤 시 마지막으로 돌아감
            }
            else if (selectedButtonIndex >= actionButtons.Length)
            {
                selectedButtonIndex = 0; // 아래로 스크롤 시 처음으로 돌아감
            }

            UpdateButtonSelection(); // 선택된 버튼 업데이트
        }

        private void UpdateButtonSelection()
        {
            for (int i = 0; i < actionButtons.Length; i++)
            {
                // 선택된 버튼에 따라 활성화 상태 변경
                ActionSlot actionSlot = actionButtons[i].GetComponent<ActionSlot>();
                if (actionSlot != null)
                {
                    actionSlot.SetSelected(i == selectedButtonIndex); // 선택 여부에 따라 활성화
                }
            }
        }

        public void ShowPanel(Item item, int slotIndex)
        {
            currentItem = item; // 현재 아이템 설정
            panel.SetActive(true); // 패널 활성화
            UpdateButtonSelection(); // 패널을 열 때 버튼 선택 초기화
        }

        public void HidePanel()
        {
            panel.SetActive(false); // 패널 비활성화
        }

        private void OnEquipButtonClicked()
        {
            if (currentItem is Equipment equipment)
            {
                // categoryId에 맞는 슬롯 찾기
                switch (equipment.categoryId)
                {
                    case 0: // Talisman
                        EquipTalisman(equipment);
                        break;

                    case 1: // Tool
                        EquipTool(equipment);
                        break;

                    case 2: // Protection
                        EquipProtection(equipment);
                        break;

                    case 3: // Weapon
                        EquipWeapon(equipment);
                        break;

                    default:
                        Debug.LogWarning("알 수 없는 아이템 카테고리입니다.");
                        break;
                }
            }

            if (currentItem.itemCount == 0)
            {
                HidePanel(); // 아이템이 0개일 경우 패널 숨기기
            }

            inventoryUI.UpdateInventoryUI(); // 인벤토리 UI 업데이트
        }

        private void EquipTalisman(Equipment equipment)
        {
            // Talisman 슬롯에 장착
            if (equippedTalismanCount < 5) // 최대 탈리스만 슬롯 수 확인
            {
                for (int i = 0; i < 5; i++)
                {
                    var talismanSlot = (EquipmentSlots)(2 + i); // Talisman_01부터 Talisman_05까지
                    if (playerInventoryHandler.talismanSlots[i] == null) // 해당 슬롯이 비어있다면
                    {
                        playerInventoryHandler.EquipEquipment(equipment, talismanSlot);
                        Debug.Log($"착용: {currentItem.itemName} (탈리스만 {i + 1})");

                        // EquipmentSlot 업데이트
                        var equipmentSlots = FindObjectsOfType<EquipmentSlot>();
                        foreach (var slot in equipmentSlots)
                        {
                            if (slot.categoryId == 0) // Talisman 카테고리 ID 확인
                            {
                                slot.UpdateSlot(currentItem); // 슬롯 업데이트
                            }
                        }
                        break;
                    }
                }
            }
            else
            {
                Debug.LogWarning("탈리스만 슬롯이 가득 찼습니다.");
            }
        }

        private void EquipTool(Equipment equipment)
        {
            // Tool 슬롯에 장착
            if (playerInventoryHandler.toolSlots.Count(t => t != null) < 10) // 최대 도구 슬롯 수 확인
            {
                for (int i = 0; i < 10; i++)
                {
                    var toolSlot = (EquipmentSlots)(7 + i); // Tool_01부터 Tool_10까지
                    if (playerInventoryHandler.toolSlots[i] == null) // 해당 슬롯이 비어있다면
                    {
                        playerInventoryHandler.EquipEquipment(equipment, toolSlot);
                        Debug.Log($"착용: {currentItem.itemName} (도구 {i + 1})");

                        // EquipmentSlot 업데이트
                        var equipmentSlots = FindObjectsOfType<EquipmentSlot>();
                        foreach (var slot in equipmentSlots)
                        {
                            if (slot.categoryId == 1) // Tool 카테고리 ID 확인
                            {
                                slot.UpdateSlot(currentItem); // 슬롯 업데이트
                            }
                        }
                        break;
                    }
                }
            }
            else
            {
                Debug.LogWarning("도구 슬롯이 가득 찼습니다.");
            }
        }

        private void EquipProtection(Equipment equipment)
        {
            // Protection 슬롯에 장착
            if (playerInventoryHandler.protectionSlot == null) // Protection 슬롯이 비어있다면
            {
                playerInventoryHandler.EquipEquipment(equipment, EquipmentSlots.Protection); // Protection 슬롯에 장착
                Debug.Log($"착용: {currentItem.itemName} (방어구)");

                // EquipmentSlot 업데이트
                var equipmentSlots = FindObjectsOfType<EquipmentSlot>();
                foreach (var slot in equipmentSlots)
                {
                    if (slot.categoryId == 2) // Protection 카테고리 ID 확인
                    {
                        slot.UpdateSlot(currentItem); // 슬롯 업데이트
                    }
                }
            }
            else
            {
                Debug.LogWarning("방어구 슬롯이 이미 가득 차 있습니다.");
            }
        }

        private void EquipWeapon(Equipment equipment)
        {
            // Weapon 슬롯에 장착
            if (playerInventoryHandler.weaponSlot == null) // Weapon 슬롯이 비어있다면
            {
                playerInventoryHandler.EquipEquipment(equipment, EquipmentSlots.Weapon); // Weapon 슬롯에 장착
                Debug.Log($"착용: {currentItem.itemName} (무기)");

                // EquipmentSlot 업데이트
                var equipmentSlots = FindObjectsOfType<EquipmentSlot>();
                foreach (var slot in equipmentSlots)
                {
                    if (slot.categoryId == 3) // Weapon 카테고리 ID 확인
                    {
                        slot.UpdateSlot(currentItem); // 슬롯 업데이트
                    }
                }
            }
            else
            {
                Debug.LogWarning("무기 슬롯이 이미 가득 차 있습니다.");
            }
        }


        private void OnDropButtonClicked()
        {
            // 버리기 로직
            if (currentItem != null)
            {
                if (currentItem.itemCount > 0)
                {
                    currentItem.itemCount--; // 아이템 갯수 감소
                    Debug.Log($"버리기: {currentItem.itemName}"); // 아이템 이름 로그 출력
                }

                if (currentItem.itemCount == 0)
                {
                    HidePanel(); // 아이템이 0개일 경우 패널 숨기기
                }
            }

            inventoryUI.UpdateInventoryUI(); // 인벤토리 갱신
        }

        private void OnDestroyButtonClicked()
        {
            // 파기 로직
            if (currentItem != null)
            {
                if (currentItem.itemCount > 0)
                {
                    currentItem.itemCount--; // 아이템 갯수 감소
                    Debug.Log($"파기: {currentItem.itemName}"); // 아이템 이름 로그 출력
                }

                if (currentItem.itemCount == 0)
                {
                    HidePanel(); // 아이템이 0개일 경우 패널 숨기기
                }
            }

            inventoryUI.UpdateInventoryUI(); // 인벤토리 갱신
        }

        public bool IsActive()
        {
            return panel.activeSelf; // 패널이 열렸는지 여부에 따라 true/false 반환
        }
    }
}

                           
