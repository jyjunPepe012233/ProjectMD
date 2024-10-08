using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : Singleton<ItemDataBase> {

	[SerializeField] private ItemSoList itemSoList;
	private List<Item> indexedItemList;
	
	

	void OnEnable() {

		IndexingItemList();

	}

	void IndexingItemList() {

		int settingId = 0;

		foreach (Weapon item in itemSoList.weaponList) {
			item.itemId = settingId;
			indexedItemList.Add(item);
			
			settingId += 1;
		}

		foreach (Protection item in itemSoList.protectionList) {
			item.itemId = settingId;
			indexedItemList.Add(item);
			
			settingId += 1;
		}

		foreach (Talisman item in itemSoList.talismanList) {
			item.itemId = settingId;
			indexedItemList.Add(item);

			settingId += 1;
		}

		foreach (Tool item in itemSoList.toolList) {
			item.itemId = settingId;
			indexedItemList.Add(item);

			settingId += 1;
		}
	}


	public int GetAllItemsCount() {
		return indexedItemList.Count;
	}

	
	// GET ORIGINAL DATA SO OF ITEM TO ITEM ID
	public Item GetItemSo(int itemId) {

		Item item = indexedItemList[itemId];
		
		if (item != null)
			return item;
		
		Debug.LogError("Can't Find The Item That Has Id " + itemId);
		return null;
	}
	

}
