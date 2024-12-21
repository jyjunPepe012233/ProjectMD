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
        public GameObject panel;
        public Button wearButton;
        public Button dropButton;
        public Button destroyButton;

        private Item currentItem;
        private PlayerInventoryHandler playerInventoryHandler;
        private InventoryUI inventoryUI;

        public Button[] actionButtons;
        public int selectedButtonIndex = 0;

        private int equippedTalismanCount => playerInventoryHandler.talismanSlots.Count(t => t != null);

        void Start()
        {
            panel.SetActive(false);
            actionButtons = new Button[] { wearButton, dropButton, destroyButton };

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
                HandleInput();
            }
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Q))
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
            HidePanel();
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
            for (int i = 0; i < 5; i++)
            {
                if (playerInventoryHandler.talismanSlots[i] == null)
                {
                    playerInventoryHandler.talismanSlots[i] = (Talisman)equipment;
                    Debug.Log($"착용: {equipment.itemName} (탈리스만 {i + 1})");

                    var talismanSlot = FindObjectsOfType<EquipmentSlot>().FirstOrDefault(slot => slot.categoryId == 0 && slot.slotIndex == i);
                    talismanSlot?.UpdateSlot(equipment);
                    break;
                }
            }
        }

        private void EquipTool(Equipment equipment)
        {
            for (int i = 0; i < 10; i++)
            {
                if (playerInventoryHandler.toolSlots[i] == null)
                {
                    playerInventoryHandler.toolSlots[i] = (Tool)equipment;
                    Debug.Log($"착용: {equipment.itemName} (도구 {i + 1})");

                    var toolSlot = FindObjectsOfType<EquipmentSlot>().FirstOrDefault(slot => slot.categoryId == 1 && slot.slotIndex == i);
                    toolSlot?.UpdateSlot(equipment);
                    break;
                }
            }
        }

        private void EquipProtection(Equipment equipment)
        {
            if (playerInventoryHandler.protectionSlot == null)
            {
                playerInventoryHandler.protectionSlot = (Protection)equipment;
                Debug.Log($"착용: {equipment.itemName} (방어구)");

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
            if (playerInventoryHandler.weaponSlot == null)
            {
                playerInventoryHandler.weaponSlot = (Weapon)equipment;
                Debug.Log($"착용: {equipment.itemName} (무기)");

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
                UpdateEquipmentSlotCount();

                if (currentItem.itemCount <= 0)
                {
                    currentItem = null;
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
                if (slot.categoryId == currentItem.categoryId)
                {
                    slot.UpdateSlot(currentItem);
                    break;
                }
            }
        }

        private void OnDestroyButtonClicked()
        {
            if (currentItem != null)
            {
                currentItem.itemCount--;
                Debug.Log($"Destroyed: {currentItem.itemName}");
                UpdateEquipmentSlotCount();

                if (currentItem.itemCount <= 0)
                {
                    currentItem = null;
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