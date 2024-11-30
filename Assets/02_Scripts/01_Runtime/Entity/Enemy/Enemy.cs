using System;
using UnityEngine;
using UnityEngine.AI;
using MinD.Runtime.Managers;
using MinD.SO.EnemySO.State;

namespace MinD.Runtime.Entity {

[RequireComponent(typeof(EnemyStateMachine))]
[RequireComponent(typeof(EnemyAnimationHandler))]
[RequireComponent(typeof(EnemyAttributeHandler))]
[RequireComponent(typeof(EnemyLocomotionHandler))]
[RequireComponent(typeof(EnemyCombatHandler))]
[RequireComponent(typeof(EnemyUtilityHandler))]
[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemy : BaseEntity {
	
	[HideInInspector] public NavMeshAgent navAgent;
	
	[HideInInspector] public EnemyStateMachine state;
	[HideInInspector] public EnemyAnimationHandler animation;
	[HideInInspector] public EnemyAttributeHandler attribute;
	[HideInInspector] public EnemyLocomotionHandler locomotion;
	[HideInInspector] public EnemyCombatHandler combat;
	[HideInInspector] public EnemyUtilityHandler utility;
	
	[HideInInspector] public Vector3 worldPlacedPosition;
	[HideInInspector] public Quaternion worldPlacedRotation;
	
	
	[Header("[ States ]")]
	public EnemyState currentState;
	public EnemyState previousState;
	[Space(3)]
	public EnemyState globalState;
	
	[HideInInspector] public EnemyState[] states;
	[HideInInspector] public EnemyState[] globalStates;
	
	[HideInInspector] public BaseEntity currentTarget;
	
	public override int CurHp {
		get => curHp;
		set => curHp = Mathf.Clamp(value, 0, attribute.MaxHp);
	}
	
	
	[Header("[ Flags ]")]
	public bool isPerformingAction;
	public bool isInCombat;
	
	public Action getHitAction = new Action(() => {});
	
	

	// SETUP A OBJECT SETTINGS
	protected override void Awake() {

		base.Awake();

		navAgent = GetComponent<NavMeshAgent>();

		state = GetComponent<EnemyStateMachine>();
		animation = GetComponent<EnemyAnimationHandler>();
		attribute = GetComponent<EnemyAttributeHandler>();
		locomotion = GetComponent<EnemyLocomotionHandler>();
		combat = GetComponent<EnemyCombatHandler>();
		utility = GetComponent<EnemyUtilityHandler>();

		WorldEnemyManager.Instance.RegisteringEnemyOnWorld(this);
		
		// SET START POSITION
		NavMesh.SamplePosition(transform.position, out NavMeshHit placedPositionOnSurface, Mathf.Infinity, NavMesh.AllAreas);
		worldPlacedPosition = placedPositionOnSurface.position;
		transform.position = worldPlacedPosition;
		worldPlacedRotation = transform.rotation;
		
		utility.AllCollisionIgnoreSetup();
		
		
		SetupStatesArray();
		Reload();
	} 
	
	// CALL SETUP
	protected override void Update() {
		
		base.Update();

		state.ExecuteStateTick();
		locomotion.HandleAllLocomotion();

	}
	
	
	
	
	// ASSIGN STATE ARRAY AND 
	protected abstract void SetupStatesArray();
	
	
	// SETUP START STATE AND RUNTIME ATTRIBUTE SETTING
	public virtual void Reload() {
		
		curHp = attribute.MaxHp;
		transform.position = worldPlacedPosition;
		transform.position = worldPlacedPosition;
		
		// NEED TO SET THE START STATE IN OVERRIDE
	}
}

}