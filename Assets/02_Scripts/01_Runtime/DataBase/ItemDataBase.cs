using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : Singleton<ItemDataBase> {

	[SerializeField] private ItemSoList itemSoList;
	

	void OnEnable() {

		GenerateItemId();
		
	}
	
	void GenerateItemId() {

		int settingId = 0;

		foreach (Weapon weapon in itemSoList.weaponList) {
			weapon.itemId = settingId;
			settingId += 1;
		}
	}
	
	public int GetAllItemsCount() {

		int count = 0;

		count += itemSoList.weaponList.Count;
		count += itemSoList.protectionList.Count;
		count += itemSoList.talismanList.Count;
		count += itemSoList.toolList.Count;
		
		return count;
	}

	
	// GET ORIGINAL DATA SO OF ITEM TO ITEM ID
	public Item GetItemSo(int itemId) {

		foreach (Item item in itemSoList.weaponList) {
			
			if (item.itemId == itemId)
				return item;
		}
		foreach (Item item in itemSoList.protectionList) {

			if (item.itemId == itemId)
				return item;
		}
		foreach (Item item in itemSoList.talismanList) {

			if (item.itemId == itemId)
				return item;
		}
		foreach (Item item in itemSoList.toolList) {

			if (item.itemId == itemId)
				return item;
		}

		Debug.LogError("Can't Find The Item That Has Id " + itemId);
		return null;
	}
	

}
