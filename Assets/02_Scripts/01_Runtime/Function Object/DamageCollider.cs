using System;
using System.Collections;
using System.Collections.Generic;
using MinD.Combat;
using UnityEngine;
using MinD.StatusFx;

[RequireComponent(typeof(Collider))]
public class DamageCollider : MonoBehaviour {

	[Header("[ Damage Setting ]")]
	public Damage damage;
	[Space(5)]
	[SerializeField] private bool againWhenExit;
	[Space(10)]
	public List<BaseEntity> blackList; // IGNORE THIS DAMAGE COLLIDER

	private List<BaseEntity> damagedEntity = new List<BaseEntity>(); 
	
	private TakeHealthDamage damageEffect;
	private Collider collider;

	
	private void Awake() {
		collider = GetComponent<Collider>();
 
		damageEffect = (TakeHealthDamage)WorldStatusFxManager.Instance.InstantiateInstantEffect(InstantEffectType.TakeHealthDamage);
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


	private void OnDrawGizmosSelected() {

		Gizmos.color = Color.blue;
		
		Gizmos.DrawSphere(transform.position, 0.5f);
		Gizmos.DrawRay(transform.position, transform.forward * 2);
		Gizmos.DrawSphere(transform.position + (transform.forward * 2), 0.2f);
		
		Gizmos.DrawWireSphere(transform.position, 2f);
		
	}
}
