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
        public Button wearButton; // 착용 버튼
        public Button dropButton; // 버리기 버튼
        public Button destroyButton; // 파기 버튼

        private Item currentItem;
        private PlayerInventoryHandler playerInventoryHandler; // PlayerInventoryHandler 인스턴스
        private InventoryUI inventoryUI; // InventoryUI 인스턴스 추가

        private int equippedTalismanCount => playerInventoryHandler.talismanSlots.Count(t => t != null);

        void Start()
        {
            panel.SetActive(false); // 시작할 때 패널 비활성화
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

        public void ShowPanel(Item item, int slotIndex)
        {
            currentItem = item; // 현재 아이템 저장
            panel.SetActive(true);
        }

        public void HidePanel()
        {
            panel.SetActive(false);
        }

        private void OnEquipButtonClicked()
        {
            // 착용 로직
            if (currentItem is Equipment equipment)
            {
                // categoryId에 따라 슬롯을 결정
                switch (equipment.categoryId)
                {
                    case 0:
                        // 각 탈리스만 슬롯은 추가 로직을 통해 결정
                        if (equippedTalismanCount < 5)
                        {
                            // Talisman_01부터 Talisman_05까지 슬롯에 착용
                            for (int i = 0; i < 5; i++)
                            {
                                var talismanSlot = (EquipmentSlots)(2 + i); // Talisman_01부터 Talisman_05까지
                                if (playerInventoryHandler.talismanSlots[i] == null) // 해당 슬롯이 비어있다면
                                {
                                    playerInventoryHandler.EquipEquipment(equipment, talismanSlot);
                                    Debug.Log($"착용: {currentItem.itemName} (탈리스만 {i + 1})"); // 아이템 이름 로그 출력
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Debug.LogWarning("탈리스만 슬롯이 가득 찼습니다."); // 슬롯이 가득 찼을 때 경고
                        }
                        break;
                    
                    case 1: // Tool
                        // 각 도구 슬롯은 추가 로직을 통해 결정
                        if (playerInventoryHandler.toolSlots.Count(t => t != null) < 10) // 현재 착용 중인 도구 수 확인
                        {
                            // Tool_01부터 Tool_10까지 슬롯에 착용
                            for (int i = 0; i < 10; i++)
                            {
                                var toolSlot = (EquipmentSlots)(7 + i); // Tool_01부터 Tool_10까지
                                if (playerInventoryHandler.toolSlots[i] == null) // 해당 슬롯이 비어있다면
                                {
                                    playerInventoryHandler.EquipEquipment(equipment, toolSlot);
                                    Debug.Log($"착용: {currentItem.itemName} (도구 {i + 1})"); // 아이템 이름 로그 출력
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Debug.LogWarning("도구 슬롯이 가득 찼습니다."); // 슬롯이 가득 찼을 때 경고
                        }
                        break;

                    case 2: // Protection
                        // Protection 슬롯이 비어 있는지 확인
                        if (playerInventoryHandler.protectionSlot == null) // Protection 슬롯이 비어있다면
                        {
                            playerInventoryHandler.EquipEquipment(equipment, EquipmentSlots.Protection);
                            Debug.Log($"착용: {currentItem.itemName} (방어구)"); 
                        }
                        else
                        {
                            Debug.LogWarning("방어구 슬롯이 이미 가득 차 있습니다."); // 슬롯이 가득 찼을 때 경고
                        }
                        break;

                    case 3: // Weapon
                        // Weapon 슬롯이 비어 있는지 확인
                        if (playerInventoryHandler.weaponSlot == null) // Weapon 슬롯이 비어있다면
                        {
                            playerInventoryHandler.EquipEquipment(equipment, EquipmentSlots.Weapon);
                            Debug.Log($"착용: {currentItem.itemName} (무기)"); 
                        }
                        else
                        {
                            Debug.LogWarning("무기 슬롯이 이미 가득 차 있습니다."); // 슬롯이 가득 찼을 때 경고
                        }
                        break;

                    // 추가적인 슬롯 카테고리 처리
                    default:
                        Debug.LogWarning("알 수 없는 아이템 카테고리입니다."); // 기본 경고 로그
                        break;
                }
            }
            HidePanel(); // 패널 숨기기
            inventoryUI.UpdateInventoryUI();
        }




        private void OnDropButtonClicked()
        {
            // 버리기 로직
            if (currentItem != null)
            {
                playerInventoryHandler.ReduceItem(currentItem.itemId); // 아이템을 감소
                Debug.Log($"버리기: {currentItem.itemName}"); // 아이템 이름 로그 출력
            }
            HidePanel(); // 패널 숨기기
            inventoryUI.UpdateInventoryUI(); // 인벤토리 갱신
        }

        private void OnDestroyButtonClicked()
        {
            // 파기 로직
            if (currentItem != null)
            {
                playerInventoryHandler.ReduceItem(currentItem.itemId); // 아이템을 감소
                Debug.Log($"파기: {currentItem.itemName}"); // 아이템 이름 로그 출력
            }
            HidePanel(); // 패널 숨기기
            inventoryUI.UpdateInventoryUI(); // 인벤토리 갱신
        }
    }
}
