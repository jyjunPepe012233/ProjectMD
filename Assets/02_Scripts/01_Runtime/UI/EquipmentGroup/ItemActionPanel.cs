using System.Collections;
using System.Linq;
using MinD.Enums;
using MinD.Runtime.Entity;
using MinD.SO.Item;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using NotImplementedException = System.NotImplementedException;

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

            playerInventoryHandler = FindObjectOfType<PlayerInventoryHandler>();
            inventoryUI = FindObjectOfType<InventoryUI>();

            if (playerInventoryHandler == null)
                Debug.LogError("PlayerInventoryHandler not found in the scene!");

            if (inventoryUI == null)
                Debug.LogError("InventoryUI not found in the scene!");
        }

        void Update()
        {
            if (panel.activeSelf)
            {
                HandleInput(); // 입력 처리
            }
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Q)) // Q 키로 패널 닫기
            {
                HidePanel();
                return;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ChangeSelection(-1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ChangeSelection(1);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(HandleButtonClickAfterDelay());
            }
        }

        private IEnumerator HandleButtonClickAfterDelay()
        {
            yield return null;

            if (selectedButtonIndex >= 0 && selectedButtonIndex < actionButtons.Length)
            {
                actionButtons[selectedButtonIndex].onClick.Invoke();
            }
        }

        private void ChangeSelection(int direction)
        {
            selectedButtonIndex += direction;
            if (selectedButtonIndex < 0)
            {
                selectedButtonIndex = actionButtons.Length - 1;
            }
            else if (selectedButtonIndex >= actionButtons.Length)
            {
                selectedButtonIndex = 0;
            }

            UpdateButtonSelection();
        }

        private void UpdateButtonSelection()
        {
            for (int i = 0; i < actionButtons.Length; i++)
            {
                ActionSlot actionSlot = actionButtons[i].GetComponent<ActionSlot>();
                if (actionSlot != null)
                {
                    actionSlot.SetSelected(i == selectedButtonIndex);
                }
            }
        }

        public void ShowPanel(Item item)
        {
            currentItem = item;
            panel.SetActive(true);
            UpdateButtonSelection();
        }

        public void HidePanel()
        {
            panel.SetActive(false);
        }

        private void OnEquipButtonClicked()
        {
            if (currentItem is Equipment equipment)
            {
                EquipItemBasedOnCategory(equipment);
            }
            HidePanel(); // 착용 후 패널 닫기
            inventoryUI.UpdateInventoryUI();
        }


        private void EquipItemBasedOnCategory(Equipment equipment)
        {
            switch (equipment.categoryId)
            {
                case 0:
                    EquipTalisman(equipment);
                    break;
                case 1:
                    EquipTool(equipment);
                    break;
                case 2:
                    EquipProtection(equipment);
                    break;
                case 3:
                    EquipWeapon(equipment);
                    break;
                default:
                    Debug.LogWarning("Unknown equipment category!");
                    break;
            }
        }

        private void EquipTalisman(Equipment equipment)
        {
            for (int i = 0; i < 5; i++) // Talisman 슬롯을 순회
            {
                if (playerInventoryHandler.talismanSlots[i] == null) // 비어있는 슬롯 확인
                {
                    playerInventoryHandler.talismanSlots[i] = (Talisman)equipment; // 슬롯에 아이템 장착
                    Debug.Log($"착용: {equipment.itemName} (탈리스만 {i + 1})");

                    // UI 업데이트 (해당 슬롯만 업데이트)
                    var talismanSlot = FindObjectsOfType<EquipmentSlot>().FirstOrDefault(slot => slot.categoryId == 0 && slot.slotIndex == i);
                    talismanSlot?.UpdateSlot(equipment);
                    break; // 아이템 장착 후 종료
                }
            }
        }

        private void EquipTool(Equipment equipment)
        {
            for (int i = 0; i < 10; i++) // Tool 슬롯을 순회
            {
                if (playerInventoryHandler.toolSlots[i] == null) // 비어있는 슬롯 확인
                {
                    playerInventoryHandler.toolSlots[i] = (Tool)equipment; // 슬롯에 아이템 장착
                    Debug.Log($"착용: {equipment.itemName} (도구 {i + 1})");

                    // UI 업데이트 (해당 슬롯만 업데이트)
                    var toolSlot = FindObjectsOfType<EquipmentSlot>().FirstOrDefault(slot => slot.categoryId == 1 && slot.slotIndex == i);
                    toolSlot?.UpdateSlot(equipment);
                    break; // 아이템 장착 후 종료
                }
            }
        }

        private void EquipProtection(Equipment equipment)
        {
            if (playerInventoryHandler.protectionSlot == null) // Protection 슬롯이 비어있다면
            {
                playerInventoryHandler.protectionSlot = (Protection)equipment; // Protection 슬롯에 아이템 장착
                Debug.Log($"착용: {equipment.itemName} (방어구)");

                // UI 업데이트 (해당 슬롯만 업데이트)
                var protectionSlot = FindObjectsOfType<EquipmentSlot>().FirstOrDefault(slot => slot.categoryId == 2);
                protectionSlot?.UpdateSlot(equipment);
            }
            else
            {
                Debug.LogWarning("방어구 슬롯이 이미 가득 차 있습니다.");
            }
        }

        private void EquipWeapon(Equipment equipment)
        {
            if (playerInventoryHandler.weaponSlot == null) // Weapon 슬롯이 비어있다면
            {
                playerInventoryHandler.weaponSlot = (Weapon)equipment; // Weapon 슬롯에 아이템 장착
                Debug.Log($"착용: {equipment.itemName} (무기)");

                // UI 업데이트 (해당 슬롯만 업데이트)
                var weaponSlot = FindObjectsOfType<EquipmentSlot>().FirstOrDefault(slot => slot.categoryId == 3);
                weaponSlot?.UpdateSlot(equipment);
            }
            else
            {
                Debug.LogWarning("무기 슬롯이 이미 가득 찼습니다.");
            }
        }





        private void OnDropButtonClicked()
        {
            if (currentItem != null)
            {
                currentItem.itemCount--;
                Debug.Log($"Dropped: {currentItem.itemName}");
                // EquipmentSlot 업데이트
                UpdateEquipmentSlotCount();

                if (currentItem.itemCount <= 0) // 아이템 수가 0이거나 이하일 경우
                {
                    currentItem = null; // 현재 아이템 초기화
                    HidePanel();
                }
            }

            inventoryUI.UpdateInventoryUI();
        }

        private void UpdateEquipmentSlotCount()
        {
            var equipmentSlots = FindObjectsOfType<EquipmentSlot>();
            foreach (var slot in equipmentSlots)
            {
                if (slot.categoryId == currentItem.categoryId) // 현재 아이템의 카테고리 확인
                {
                    slot.UpdateSlot(currentItem); // 슬롯 업데이트
                    break; // 첫 번째 슬롯만 업데이트
                }
            }
        }

        private void OnDestroyButtonClicked()
        {
            if (currentItem != null)
            {
                currentItem.itemCount--;
                Debug.Log($"Destroyed: {currentItem.itemName}");
                // EquipmentSlot 업데이트
                UpdateEquipmentSlotCount();

                if (currentItem.itemCount <= 0) // 아이템 수가 0이거나 이하일 경우
                {
                    currentItem = null; // 현재 아이템 초기화
                    HidePanel();
                }
            }

            inventoryUI.UpdateInventoryUI();
        }

        public bool IsActive()
        {
            return panel.activeSelf;
        }
    }
}

                           
