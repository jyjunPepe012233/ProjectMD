using System.Collections;
using System.Collections.Generic;
using MinD;
using MinD.Combat;
using UnityEngine;

public abstract class Weapon : Equipment {

	public GameObject weaponPrefab;
	public Vector3 weaponPositionOffset;
	public Vector3 weaponAngleOffset;
	
	[Header("[ Weapon Setting ]")]
	public WeaponType weaponType;
	
	[Header("[ Weapon Status ]")]
	public SpiritAffinity weaponRequiredAffinity;
	public Damage weaponDamage;
	
}