using System.Linq;
using MinD.Enums;
using MinD.Runtime.DataBase;
using MinD.Runtime.Managers;
using MinD.SO.Item;
using UnityEngine;

namespace MinD.Runtime.Entity {

public class PlayerInventoryHandler : MonoBehaviour {

	[HideInInspector] public Player owner;


	[Header("[ Equipment Slot ]")]
	public Weapon weaponSlot;
	public Protection protectionSlot;

	[Space(5)]
	public int allowedTalismanSlotCount;
	public Talisman[] talismanSlots = new Talisman[5];

	[Space(5)]
	public Tool[] toolSlots = new Tool[10];
	

	[Header("[ Magic Slot ]")]
	public int currentMagicSlot;
	public Magic[] magicSlots = new Magic[1]; // CHANGE SLOT SIZE BY ATTRIBUTE IN RUNTIME

	private int usingMemory; // MEMORY AMOUNT OF CURRENT USING MAGICS


	[Header("[ Owned Item Array ]")]
	[SerializeField] public Item[] playerItemList;

	
	[Header("[ Debug ]")]
	public Magic equipMagic;

	public void OnValidate() {
		if (equipMagic != null) {
			EquipMagic(equipMagic, 0);
			equipMagic = null;
		}
	}




	public void LoadItemData() {

		// SET DATA LIST LENGTH
		playerItemList = new Item[ItemDataBase.Instance.GetAllItemsCount()];

		// TODO: LOAD THE ITEM FROM SAVE DAT
	}

	// load quickslot data
	// load slot and set selected Magic



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



	public void EquipEquipment(Equipment equipment, EquipmentSlots targetSlot) {

		// INSERT EQUIPMENT IN SLOT
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

	public void UnequipEquipment(EquipmentSlots targetSlot) {

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



	public bool EquipMagic(Magic magic, int slotPos) {

		// CANCEL IF EXCEED THE MEMORY CAPACITY
		if (magic.memoryCost + usingMemory > owner.attribute.memoryCapacity) {
			return false;
		}

		// CANCEL IF slotPos PARAMETER IS OVER THE LIST SIZE
		if (slotPos < 0 || slotPos >= magicSlots.Length) {
			return false;
		}


		usingMemory += magic.memoryCost;
		magicSlots[slotPos] = magic;

		return true;
	}

	public void UnequipMagic(int slotPos) {

		// CANCEL IF slotPos PARAMETER IS OVER THE LIST SIZE
		if (slotPos < 0 || slotPos >= magicSlots.Length) {
			return;
		}

		usingMemory -= magicSlots[slotPos].memoryCost;
		magicSlots[slotPos] = null;

	}

	// RESIZE MAGIC SLOT



	public void HandleQuickSlotSwapping() {
		HandleMagicSlotSwapping();
	}

	private void HandleMagicSlotSwapping() {

		// CHECK INPUT FLAG
		if (PlayerInputManager.Instance.swapMagicInput == 0) {
			return;
		}


		// CANCEL SWAP IF PLAYER HASN'T ANY MAGIC
		if (magicSlots.Count(i => i != null) == 0)
			return;


		if (PlayerInputManager.Instance.swapMagicInput == 1) {
			while (true) {
				// TO SKIP EMPTY MAGIC
				// REMAINDER OPERATING TO CYCLE THE LIST
				currentMagicSlot = (currentMagicSlot + 1) % magicSlots.Length;
				if (magicSlots[currentMagicSlot] != null) {
					break;
				}
			}

		} else if (PlayerInputManager.Instance.swapMagicInput == -1) {
			while (true) {
				currentMagicSlot = (currentMagicSlot - 1 + magicSlots.Length) % magicSlots.Length;
				if (magicSlots[currentMagicSlot] != null) {
					break;
				}
			}
		}

		PlayerInputManager.Instance.swapMagicInput = 0;
	}

}

}