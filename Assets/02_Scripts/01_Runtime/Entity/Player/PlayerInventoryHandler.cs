using System.Collections;
using System.Collections.Generic;
using MinD;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventoryHandler : MonoBehaviour {

	private Player owner;

	// INVENTORY
	private Weapon weaponSlot;
	private Protection protectionSlot;
	private Talisman[] talismanSlots = new Talisman[5];
	private Tool[] toolSlots = new Tool[10];
	
	private List<Item> playerItemList;



	private void LoadItemData() {
		
		// SET DATA LIST LENGTH
		playerItemList = new List<Item>(ItemDataBase.Instance.GetAllItemsCount());
		
		
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

	
	public bool ReduceItem(Item targetItem, int amount = 1) {

		if (amount < 0)
			amount = 0;

		// USE ID AS INDEX TO FIND THE TARGET ITEM INSTANCE IN LIST
		// CAUSE ID IS GENERATE BASED ON INDEX OF ITEM SO LIST
		Item itemInList = playerItemList[targetItem.itemId];
		
		// IF ITEM INSTANCE IS NOT CREATED
		if (itemInList == null)
			return false;

		// CHECK IF THERE ARE ENOUGH ITEM TO REMOVE
		if (itemInList.itemCount < amount)
			return false;


		itemInList.itemCount -= amount;
		return true;
	}

	public void EquipItem(Equipment equipment, InventorySlots targetSlot) {
		
		
	}

	public void UnequipItem(InventorySlots targetSlot) {
		
		
	}
}
