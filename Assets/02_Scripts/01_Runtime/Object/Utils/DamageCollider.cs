using System.Collections.Generic;
using MinD.Runtime.Entity;
using MinD.SO.Object;
using MinD.SO.StatusFX;
using MinD.SO.StatusFX.Effects;
using UnityEngine;

namespace MinD.Runtime.Utils {

public class DamageCollider : MonoBehaviour {
	
	public DamageData soData;


	public List<BaseEntity> blackList = new List<BaseEntity>(); // IGNORE THIS DAMAGE COLLIDER
	
	private List<BaseEntity> damagedEntity = new List<BaseEntity>();

	

	private void OnTriggerEnter(Collider other) {
		
		if (soData == null) { // IF NO DAMAGE DATA IS REGISTERED, THE DAMAGE COLLIDER WILL DISABLE
			gameObject.SetActive(false);
;			return;
		}
		
		
		
		BaseEntity damageTarget = other.GetComponentInParent<BaseEntity>();
		if (damageTarget == null) {
			return;
		}
		
		
		
		if (damageTarget.isInvincible) {
			return;
		}  
		if (damagedEntity.Contains(damageTarget)) { // CANCEL DAMAGE IF TARGET ENTITY ALREADY DAMAGED
			return;
		}
		if (blackList.Contains(damageTarget)) {
			return;
		}
		
		
		// GET HIT DIRECTION
		Vector3 attackDirx = other.ClosestPoint(transform.position) - transform.position; // ATTACK DIRECTION AS TARGET
		float attackAngle = Vector3.SignedAngle(damageTarget.transform.forward, -attackDirx, Vector3.up);
		
		damagedEntity.Add(damageTarget);

		InstantEffect damageEffect = null;
		if (damageTarget is Player player) {
			
			if (player.combat.isParrying) {
				damageEffect = new AbsorbMagic(soData.absorbMp, attackDirx);
				
			} else if (player.combat.usingDefenseMagic) {
				damageEffect = new TakeDefensedHealthDamage(soData.damage, soData.poiseBreakDamage, attackDirx);

			} else {
				damageEffect = new TakeHealthDamage(soData.damage, soData.poiseBreakDamage, attackAngle);
			}
			
		} else { 
			damageEffect = new TakeHealthDamage(soData.damage, soData.poiseBreakDamage, attackAngle);
			
		}

		// GIVE EFFECT TO TARGET
		damageTarget.statusFx.AddInstantEffect(damageEffect);
	}



	private void OnDisable() {
		ResetToHitAgain();
	}

	public void ResetToHitAgain() {
		damagedEntity.Clear();
	}

}

}