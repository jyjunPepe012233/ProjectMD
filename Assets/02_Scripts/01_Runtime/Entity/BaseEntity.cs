using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MinD.Runtime.Entity {

[RequireComponent(typeof(EntityStatusFxHandler))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]

public abstract class BaseEntity : MonoBehaviour {
	
	[Space(5)]
	public bool isInvincible;
	public bool immunePoiseBreak;
	public List<Transform> targetOptions;
	
	[Header("[ Attributes ]")]
	

	[HideInInspector] public bool isDeath;

	[HideInInspector] public EntityStatusFxHandler statusFx;

	[HideInInspector] public CharacterController cc;
	[HideInInspector] public Animator animator;

	public Action dieAction = new Action(() => {});


	protected virtual void Awake() {

		statusFx = GetComponent<EntityStatusFxHandler>();
		statusFx.owner = this;

		cc = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();

	}
	protected virtual void Update() {
		
		statusFx.HandleAllEffect();
		
	}
	
	protected abstract IEnumerator Die();

	private void OnDrawGizmos() {

		Gizmos.color = Color.cyan;

		if (targetOptions != null) {
			foreach (Transform targetOption in targetOptions) {
				Gizmos.DrawSphere(targetOption.position, 0.05f);
			}
		}

	}
}

}