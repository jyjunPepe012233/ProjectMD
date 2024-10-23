using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MinD {
	
	[RequireComponent(typeof(EnemyStateMachine))]
	[RequireComponent(typeof(EnemyCombatHandler))]
	[RequireComponent(typeof(EnemyCollisionHandler))]
	[RequireComponent(typeof(EnemyAnimationHandler))]
	[RequireComponent(typeof(EnemyEquipmentHandler))]
	[RequireComponent(typeof(NavMeshAgent))]
	public abstract class Enemy : BaseEntity {
		
		[Header("[ States Info ]")]
		public EnemyState currentState;
		public EnemyState previousState;
		[HideInInspector] public EnemyState[] states;
		
		[Header("[ Current Status ]")]
		public float curHp;

		[Header("[ Attribute Settings ]")]
		public float maxHp;

		[Header("[ Flags ]")]
		public bool isPerformingAction;
		public bool isInCombat;
		
		
		[HideInInspector] public EnemyStateMachine stateMachine;
		[HideInInspector] public EnemyCombatHandler combat;
		[HideInInspector] public EnemyCollisionHandler collision;
		[HideInInspector] public EnemyAnimationHandler animation;
		[HideInInspector] public EnemyEquipmentHandler equipment;

		[HideInInspector] public NavMeshAgent agent;
		
		
		
		protected override void Awake() {

			base.Awake();
			
			agent = GetComponent<NavMeshAgent>();
			
			
			stateMachine = GetComponent<EnemyStateMachine>();
			combat = GetComponent<EnemyCombatHandler>();
			collision = GetComponent<EnemyCollisionHandler>();
			animation = GetComponent<EnemyAnimationHandler>();
			equipment = GetComponent<EnemyEquipmentHandler>();
			
			stateMachine.owner = this;
			combat.owner = this;
			collision.owner = this;
			animation.owner = this;
			equipment.owner = this;
		}

		private void Start() {
			Setup();
		}

		protected virtual void Setup() {
			
			SetupStates();

			WorldEntityManager.Instance.RegisteringEnemyOnWorld(this);

		}

		protected abstract void SetupStates();

		protected virtual void Update() {

			stateMachine.ExecuteStateTick();

		}
		
	}
}