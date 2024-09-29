using System.Collections;
using System.Collections.Generic;
using MinD;
using MinD.Combat;
using UnityEngine;

public abstract class Weapon : Equipment {

	public GameObject weaponPrefab;

	[Header("[ Weapon Status ]")]
	public SpiritAffinity requiredAffinity;
	[Space(5)]
	public Damage damage;
}
