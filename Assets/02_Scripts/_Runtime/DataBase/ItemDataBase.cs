using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : Singleton<ItemDataBase> {

	private ItemSOList itemSoList;

	void Awake() {

		GenerateItemId();
		
	}

	void GenerateItemId() {

		int settingId = 0;

		foreach (Weapon weapon in itemSoList.weaponList) {
			weapon.itemId = settingId;
			settingId += 1;
		}

	}

}
