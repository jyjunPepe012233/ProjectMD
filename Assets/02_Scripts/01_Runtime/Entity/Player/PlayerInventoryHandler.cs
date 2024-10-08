using System.Collections;
using System.Collections.Generic;
using MinD;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventoryHandler : MonoBehaviour {

	[HideInInspector] public Player owner;

	// INVENTORY
	[SerializeField] private Weapon weaponSlot;
	[SerializeField] private Protection protectionSlot;
	[SerializeField] private Talisman[] talismanSlots = new Talisman[5];
	[SerializeField] private Tool[] toolSlots = new Tool[10];
	
	[SerializeField] private Item[] playerItemList;



	public void LoadItemData() {
		
		// SET DATA LIST LENGTH
		playerItemList = new Item[ItemDataBase.Instance.GetAllItemsCount()]; 
		
		// TODO: LOAD THE ITEM FROM SAVE DAT
	}


	
	private Item CreateItem(int itemId) {
		
		Item newItem = Instantiate(ItemDataBase.Instance.GetItemSo(itemId));
		playerItemList[itemId] = newItem;

		return newItem;
	}
	
	public bool AddItem(int itemId, int amount = 1, bool deleteExceededItem = false) {

		if (amount < 0)
			amount = 0; // MIN(0) CLAMP
		
		// USE ID AS INDEX TO FIND THE TARGET ITEM INSTANCE IN LIST
		// CAUSE ID IS GENERATE BASED ON INDEX OF ITEM SO LIST
		Item itemInList = playerItemList[itemId];

		// IF ITEM INSTANCE IS NOT CREATED
		if (itemInList == null)
			itemInList = CreateItem(itemId);
		
		
		// IF ITEM COUNT WILL EXCEEDS MAX COUNT
		if (itemInList.itemCount + amount > itemInList.itemMaxCount) {

			if (deleteExceededItem) {

				itemInList.itemCount = itemInList.itemMaxCount;
				return true;
			} else
				return false;
		}
		
		
		// WORKING NORMALLY
		itemInList.itemCount += amount;
		return true;
	}
	
	public bool ReduceItem(int itemId, int amount = 1) {

		if (amount < 0)
			amount = 0;

		// USE ID AS INDEX TO FIND THE TARGET ITEM INSTANCE IN LIST
		// CAUSE ID IS GENERATE BASED ON INDEX OF ITEM SO LIST
		Item itemInList = playerItemList[itemId];
		
		// IF ITEM INSTANCE IS NOT CREATED
		if (itemInList == null)
			return false;

		// CHECK IF THERE ARE ENOUGH ITEM TO REMOVE
		if (itemInList.itemCount < amount)
			return false;


		itemInList.itemCount -= amount;
		return true;
	}

	
	
	public void EquipItem(Equipment equipment, EquipmentSlots targetSlot) {

		switch (targetSlot) {
			
			case EquipmentSlots.Weapon:
				weaponSlot = equipment as Weapon;
				break;
			
			case EquipmentSlots.Protection:
				protectionSlot = equipment as Protection;
				break;
			
			#region Talismans
			case EquipmentSlots.Talisman_01:
				talismanSlots[0] = equipment as Talisman;
				break;

			case EquipmentSlots.Talisman_02:
				talismanSlots[1] = equipment as Talisman;
				break;

			case EquipmentSlots.Talisman_03:
				talismanSlots[2] = equipment as Talisman;
				break;

			case EquipmentSlots.Talisman_04:
				talismanSlots[3] = equipment as Talisman;
				break;

			case EquipmentSlots.Talisman_05:
				talismanSlots[4] = equipment as Talisman;
				break;
			#endregion
			
			#region Tools
			case EquipmentSlots.Tool_01:
				toolSlots[0] = equipment as Tool;
				break;

			case EquipmentSlots.Tool_02:
				toolSlots[1] = equipment as Tool;
				break;

			case EquipmentSlots.Tool_03:
				toolSlots[2] = equipment as Tool;
				break;

			case EquipmentSlots.Tool_04:
				toolSlots[3] = equipment as Tool;
				break;

			case EquipmentSlots.Tool_05:
				toolSlots[4] = equipment as Tool;
				break;

			case EquipmentSlots.Tool_06:
				toolSlots[5] = equipment as Tool;
				break;

			case EquipmentSlots.Tool_07:
				toolSlots[6] = equipment as Tool;
				break;

			case EquipmentSlots.Tool_08:
				toolSlots[7] = equipment as Tool;
				break;

			case EquipmentSlots.Tool_09:
				toolSlots[8] = equipment as Tool;
				break;

			case EquipmentSlots.Tool_10:
				toolSlots[9] = equipment as Tool;
				break;
			#endregion
		}
		
		ReduceItem(equipment.itemId);
		equipment.OnEquip(owner);
		
		if (equipment is Weapon weapon)
			owner.equipment.ChangeWeapon(weapon);
		
	}

	public void UnequipItem(EquipmentSlots targetSlot) {

		Equipment unequipedItem = null;

		// SET unequipedItem AND NULL TARGET SLOT
		switch (targetSlot) {

			case EquipmentSlots.Weapon:
				unequipedItem = weaponSlot;

				if (weaponSlot != null) {
					
					if (unequipedItem is Weapon weapon)
						owner.equipment.ChangeWeapon(null);

					weaponSlot = null;
				}
				
				weaponSlot = null;
				break;

			case EquipmentSlots.Protection:
				unequipedItem = protectionSlot;
				protectionSlot = null;
				break;

			#region Talismans

			case EquipmentSlots.Talisman_01:
				unequipedItem = talismanSlots[0];
				protectionSlot = null;
				break;

			case EquipmentSlots.Talisman_02:
				unequipedItem = talismanSlots[1];
				talismanSlots[0] = null;
				break;

			case EquipmentSlots.Talisman_03:
				unequipedItem = talismanSlots[2];
				talismanSlots[2] = null;
				break;

			case EquipmentSlots.Talisman_04:
				unequipedItem = talismanSlots[3];
				talismanSlots[3] = null;
				break;

			case EquipmentSlots.Talisman_05:
				unequipedItem = talismanSlots[4];
				talismanSlots[4] = null;
				break;

			#endregion

			#region Tools

			case EquipmentSlots.Tool_01:
				unequipedItem = toolSlots[0];
				toolSlots[0] = null;
				break;

			case EquipmentSlots.Tool_02:
				unequipedItem = toolSlots[1];
				toolSlots[1] = null;
				break;

			case EquipmentSlots.Tool_03:
				unequipedItem = toolSlots[2];
				toolSlots[2] = null;
				break;

			case EquipmentSlots.Tool_04:
				unequipedItem = toolSlots[3];
				toolSlots[3] = null;
				break;

			case EquipmentSlots.Tool_05:
				unequipedItem = toolSlots[4];
				toolSlots[4] = null;
				break;

			case EquipmentSlots.Tool_06:
				unequipedItem = toolSlots[5];
				toolSlots[5] = null;
				break;

			case EquipmentSlots.Tool_07:
				unequipedItem = toolSlots[6];
				toolSlots[6] = null;
				break;

			case EquipmentSlots.Tool_08:
				unequipedItem = toolSlots[7];
				toolSlots[7] = null;
				break;

			case EquipmentSlots.Tool_09:
				unequipedItem = toolSlots[8];
				toolSlots[8] = null;
				break;

			case EquipmentSlots.Tool_10:
				unequipedItem = toolSlots[9];
				toolSlots[9] = null;
				break;

			#endregion
		}

		AddItem(unequipedItem.itemId);
		unequipedItem.OnUnequip(owner);
	}
	
}