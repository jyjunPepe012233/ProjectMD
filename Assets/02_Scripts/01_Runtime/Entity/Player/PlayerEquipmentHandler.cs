using MinD.SO.Item;
using UnityEngine;

namespace MinD.Runtime.Entity {

public class PlayerEquipmentHandler : BaseEntityHandler<Player> {
	
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

}