using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dummy Staff", menuName = "MinD/Item/Items/Weapons/Staffs/Dummy Staff")]
public class DummyStaff : Weapon {
	
	public override void OnEquip(Player owner) {
		Debug.Log("장비함!");
	}

	public override void Execute(Player owner) {

	}

	public override void OnUnequip(Player owner) {
		Debug.Log("장비 해제!");
	}
}
