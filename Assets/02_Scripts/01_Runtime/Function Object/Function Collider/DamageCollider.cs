using System;
using System.Collections;
using System.Collections.Generic;
using MinD.Combat;
using UnityEngine;
using MinD.StatusFx;
using UnityEditor;

public class DamageCollider : MonoBehaviour {
	
	[HideInInspector] public Damage damage;
	[HideInInspector] public bool againWhenExit;
	
	[HideInInspector] public List<BaseEntity> blackList; // IGNORE THIS DAMAGE COLLIDER
	
	
	private List<BaseEntity> damagedEntity = new List<BaseEntity>();
	
	private TakeHealthDamage damageEffect;

	
	private void Awake() {
		
		damageEffect = (TakeHealthDamage)StatusFxDataBase.Instance.GetEffectData(InstantEffectType.TakeHealthDamage);
		damageEffect.damage = damage;
		
	}


	private void OnTriggerEnter(Collider other) {

		BaseEntity damageTarget = other.GetComponentInParent<BaseEntity>();

		// TARGET HASN'T ENTITY COMPONENT 
		if (damageTarget == null) {
			
			damageTarget = other.GetComponent<BaseEntity>();

			if (damageTarget == null)
				return;
		}

		// CANCEL DAMAGE IF TARGET ENTITY ALREADY DAMAGED
		if (damagedEntity.Contains(damageTarget))
			return;

		// CANCEL DAMAGE IF TARGET IS BLACKLIST
		if (blackList.Contains(damageTarget))
			return;
		
		
		// GET HIT DIRECTION
		float hitPointAngle =
			Vector3.SignedAngle(-transform.forward, damageTarget.transform.forward, Vector3.up);
			// 데미지콜라이더의 방향이 데미지의 방향이다
		
		if (hitPointAngle >= -45 && hitPointAngle < 45)
			damageEffect.hitDirection = HitDirection.Front;
		
		else if (hitPointAngle >= 45 && hitPointAngle < 135)
			damageEffect.hitDirection = HitDirection.Right;
		
		else if (hitPointAngle >= 135 || hitPointAngle < -135)
			damageEffect.hitDirection = HitDirection.Back;
		
		else
			damageEffect.hitDirection = HitDirection.Left;
		
		// GIVE EFFECT TO TARGET
		damagedEntity.Add(damageTarget);
		damageTarget.statusFx.AddInstantEffect(damageEffect);

	}

	private void OnTriggerExit(Collider other) {

		if (!againWhenExit)
			return;
		
		BaseEntity target = other.GetComponentInParent<BaseEntity>();

		// TARGET HASN'T ENTITY COMPONENT 
		if (target == null) {
			
			target = other.GetComponent<BaseEntity>();

			if (target == null)
				return;
		}

		if (damagedEntity.Contains(target))
			damagedEntity.Remove(target);
	}
	
	

	private void OnDisable() {
		damagedEntity.Clear();
	}
	
}
