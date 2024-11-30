using System;
using System.Collections.Generic;
using UnityEngine;

namespace MinD.Runtime.Entity {

[RequireComponent(typeof(EntityStatusFxHandler))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]

public abstract class BaseEntity : MonoBehaviour {
	
	[HideInInspector] public EntityStatusFxHandler statusFx;

	[HideInInspector] public CharacterController cc;
	[HideInInspector] public Animator animator;
	
	
	
	[HideInInspector] public bool isDeath;

	[Space(5)]
	public bool isInvincible;
	public bool immunePoiseBreak;
	public List<Transform> targetOptions;
	[Space(5)]
	[SerializeField] protected int curHp;
	public abstract int CurHp { get; set; }

	
	public Action getHitAction = new Action(()=>{});
	public Action dieAction = new Action(() => {});
	
	

	protected virtual void Awake() {
		
		statusFx = GetComponent<EntityStatusFxHandler>();

		cc = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();

	}
	protected virtual void Update() {
		
		statusFx.HandleAllEffect();
		
	}

	protected abstract void OnDeath();
}

}