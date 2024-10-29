using UnityEngine;
using UnityEngine.AI;
using MinD.Runtime.Managers;
using MinD.SO.EnemyState;
using UnityEngine.Serialization;

namespace MinD.Runtime.Entity {

[RequireComponent(typeof(EnemyStateMachine))]
[RequireComponent(typeof(EnemyCombatHandler))]
[RequireComponent(typeof(EnemyAnimationHandler))]
[RequireComponent(typeof(EnemyColliderHandler))]
[RequireComponent(typeof(EnemyUtilityHandler))]
[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemy : BaseEntity {
	
	[HideInInspector] public NavMeshAgent navAgent;
	
	[HideInInspector] public EnemyStateMachine stateMachine;
	[HideInInspector] public EnemyCombatHandler combat;
	[HideInInspector] public EnemyAnimationHandler animation;
	[HideInInspector] public EnemyColliderHandler collider;
	[HideInInspector] public EnemyUtilityHandler utility;
	
	
	[Header("[ States Info ]")]
	public EnemyState currentState;
	public EnemyState previousState;

	[Header("[ Attributes ]")]
	public float curHp;

	[Header("[ Flags ]")]
	public bool isPerformingAction;
	public bool isInCombat;
	
	
	[HideInInspector] public EnemyState[] states;
	


	protected override void Awake() {

		base.Awake();

		navAgent = GetComponent<NavMeshAgent>();

		stateMachine = GetComponent<EnemyStateMachine>();
		combat = GetComponent<EnemyCombatHandler>();
		animation = GetComponent<EnemyAnimationHandler>();
		collider = GetComponent<EnemyColliderHandler>();
		utility = GetComponent<EnemyUtilityHandler>();
		

		stateMachine.owner = this;
		combat.owner = this;
		animation.owner = this;
		collider.owner = this;
		utility.owner = this;
		
		
		
		WorldEntityManager.Instance.RegisteringEnemyOnWorld(this);
	}

	private void Start() {
		Setup();
	}
	
	protected override void Update() {
		
		base.Update();

		stateMachine.ExecuteStateTick();

	}
	
	

	protected virtual void Setup() {

		SetupStates();
		
		collider.BeIgnoreCollisionWithMyColliders();
	}

	protected abstract void SetupStates();



}

}