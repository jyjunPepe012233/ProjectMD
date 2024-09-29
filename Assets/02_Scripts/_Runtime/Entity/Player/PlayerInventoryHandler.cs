using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryHandler : MonoBehaviour {

	public Player owner;

	private Weapon weaponSlot;



	private void EquipWeapon(Weapon newWeapon) {

		weaponSlot = newWeapon;
		

	}
}
