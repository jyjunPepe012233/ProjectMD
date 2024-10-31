using System.Collections.Generic;
using MinD.Runtime.Entity;
using MinD.SO.StatusFX.Effects;
using MinD.SO.Utils;
using UnityEngine;

namespace MinD.Runtime.Utils {

public class DamageCollider : MonoBehaviour {
	
	public DamageData soData;


	public List<BaseEntity> blackList; // IGNORE THIS DAMAGE COLLIDER
	
	private List<BaseEntity> damagedEntity = new List<BaseEntity>();

	

	private void OnTriggerEnter(Collider other) {

		if (soData == null) {
			return;
		}
		
		

		BaseEntity damageTarget = other.GetComponentInParent<BaseEntity>();

		// TARGET HASN'T ENTITY COMPONENT 
		if (damageTarget == null) {

			damageTarget = other.GetComponent<BaseEntity>();

			if (damageTarget == null)
				return;
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
		float hitPointAngle =
			Vector3.SignedAngle(-transform.forward, damageTarget.transform.forward, Vector3.up);
			// 데미지콜라이더의 방향이 데미지의 방향이다

		if (hitPointAngle >= -45 && hitPointAngle < 45)
			damageEffect.hitDirection = "F";

		else if (hitPointAngle >= 45 && hitPointAngle < 135)
			damageEffect.hitDirection = "R";

		else if (hitPointAngle >= 135 || hitPointAngle < -135)
			damageEffect.hitDirection = "B";

		else
			damageEffect.hitDirection = "L";

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