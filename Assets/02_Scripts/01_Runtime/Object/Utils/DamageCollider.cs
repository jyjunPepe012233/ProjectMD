using System.Collections.Generic;
using MinD.Runtime.Entity;
using MinD.SO.StatusFX.Effects;
using MinD.SO.Utils;
using UnityEngine;

namespace MinD.Runtime.Utils {

public class DamageCollider : MonoBehaviour {
	
	public DamageData soData;


	public List<BaseEntity> blackList = new List<BaseEntity>(); // IGNORE THIS DAMAGE COLLIDER
	
	private List<BaseEntity> damagedEntity = new List<BaseEntity>();

	

	private void OnTriggerEnter(Collider other) {
		
		if (soData == null) {
;			return;
		}
		
		
		
		BaseEntity damageTarget = other.GetComponentInParent<BaseEntity>();

		// TARGET HASN'T ENTITY COMPONENT 
		if (damageTarget == null) {

			damageTarget = other.GetComponent<BaseEntity>();

			if (damageTarget == null) {
				return;
			}
		}
		
		if (damageTarget.isInvincible)
			return;
		
		// CANCEL DAMAGE IF TARGET ENTITY ALREADY DAMAGED
		if (damagedEntity.Contains(damageTarget))
			return;

		if (blackList.Contains(damageTarget)) {
			return;
		}



		TakeHealthDamage damageEffect = new TakeHealthDamage();

		damageEffect.damage = soData.damage;
		damageEffect.poiseBreakDamage = soData.poiseBreakDamage;
		
		
		// GET HIT DIRECTION
		Vector3 hitDirx = other.ClosestPoint(transform.position) - transform.position; // HIT DETECTION DIRECTION OF THE THIS COLLIDER
		damageEffect.hitAngle = Vector3.SignedAngle(damageTarget.transform.forward, -hitDirx, Vector3.up);

		// GIVE EFFECT TO TARGET
		damagedEntity.Add(damageTarget);
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