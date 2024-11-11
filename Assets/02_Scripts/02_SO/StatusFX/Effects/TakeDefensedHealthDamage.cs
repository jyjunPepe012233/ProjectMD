using MinD.Enums;
using MinD.Runtime.Entity;
using MinD.Structs;
using UnityEngine;

namespace MinD.SO.StatusFX.Effects {

[CreateAssetMenu(fileName = "TakeDefensedHealthDamage", menuName = "MinD/Status Effect/Effects/TakeDefensedHealthDamage")]
public class TakeDefensedHealthDamage : InstantEffect {

	public Damage damage;
	public int poiseBreakDamage;

	public float hitAngle;


	public TakeDefensedHealthDamage(Damage damage, int poiseBreakDamage, float hitAngle) {
		this.damage = damage;
		this.poiseBreakDamage = poiseBreakDamage;
		this.hitAngle = hitAngle;
	}

	
	protected override void OnInstantiateAs(Player player) {
		
		
		
	}

	protected override void OnInstantiateAs(Enemy enemy) {
	}	

}

}