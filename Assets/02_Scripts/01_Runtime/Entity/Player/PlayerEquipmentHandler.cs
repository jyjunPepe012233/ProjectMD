using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerEquipmentHandler : MonoBehaviour {

	[HideInInspector] public Player owner;

	public Transform rightHand;

	private GameObject currentInstantiatedWeapon;


	public void ChangeWeapon(Weapon weapon) {
		
		if (currentInstantiatedWeapon != null) {
			Destroy(currentInstantiatedWeapon);
			currentInstantiatedWeapon = null;
		}

		GameObject obj = Instantiate(weapon.weaponPrefab, rightHand);

		obj.transform.localPosition = weapon.weaponPositionOffset;
		obj.transform.localEulerAngles = weapon.weaponAngleOffset;

	}
}