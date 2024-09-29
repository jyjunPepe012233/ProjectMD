using System.Collections;
using System.Collections.Generic;
using MinD;
using MinD.Combat;
using UnityEngine;

public abstract class Weapon : Equipment {

	public GameObject weaponPrefab;
	
	[Header("[ Weapon Status ]")]
	public SpiritAffinity weaponRequiredAffinity;
	[Space(5)]
	public Damage weaponDamage;

	[Header("[ Setting ]")]
	public WeaponType weaponType;
}
