using System.Collections.Generic;
using MinD.Runtime.Entity;
using MinD.SO.StatusFX;
using MinD.SO.StatusFX.Effects;
using MinD.SO.Utils;
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
		Vector3 hitDirx = other.ClosestPoint(transform.position) - transform.position; // HIT DETECTION DIRECTION OF THE THIS COLLIDER
		float hitAngle = Vector3.SignedAngle(damageTarget.transform.forward, -hitDirx, Vector3.up);
		
		damagedEntity.Add(damageTarget);

		InstantEffect damageEffect = null;
		if (damageTarget is Player player) {
			
			if (player.combat.isParrying) {
				damageEffect = new AbsorbMagic(soData.absorbMp, hitDirx);
				
			} else if (player.combat.usingDefenseMagic) {
				damageEffect = new TakeDefensedHealthDamage(soData.damage, soData.poiseBreakDamage, hitAngle);

			} else {
				damageEffect = new TakeHealthDamage(soData.damage, soData.poiseBreakDamage, hitAngle);

			}
		} else { 
			damageEffect = new TakeHealthDamage(soData.damage, soData.poiseBreakDamage, hitAngle);
			
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