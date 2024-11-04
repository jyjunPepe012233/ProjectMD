using UnityEngine;
using UnityEngine.AI;
using MinD.Runtime.Managers;
using MinD.SO.EnemySO;
using UnityEngine.Serialization;

namespace MinD.Runtime.Entity {

[RequireComponent(typeof(EnemyStateMachine))]
[RequireComponent(typeof(EnemyCombatHandler))]
[RequireComponent(typeof(EnemyAnimationHandler))]
[RequireComponent(typeof(EnemyUtilityHandler))]
[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemy : BaseEntity {
	
	[HideInInspector] public NavMeshAgent navAgent;
	
	[HideInInspector] public EnemyStateMachine stateMachine;
	[HideInInspector] public EnemyCombatHandler combat;
	[HideInInspector] public EnemyAnimationHandler animation;
	[HideInInspector] public EnemyUtilityHandler utility;
	
	[Space(15)]
	public EnemyState currentState;
	public EnemyState previousState;

	[Header("[ Attributes ]")]
	[SerializeField] private int curHp;
	public int CurHp {
		get => curHp;
		set {
			curHp = value;
			if (curHp <= 0) {
				// DIE
			}
			
			curHp = Mathf.Clamp(curHp, 0, attribute.maxHp);
		}
	}
	public EnemyAttribute attribute;
	
	
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
		utility = GetComponent<EnemyUtilityHandler>();
		

		stateMachine.owner = this;
		combat.owner = this;
		animation.owner = this;
		utility.owner = this;
		
		
		WorldEntityManager.Instance.RegisteringEnemyOnWorld(this);
	}

	private void OnEnable() {
		Setup();
	}
	
	protected override void Update() {
		
		base.Update();

		stateMachine.ExecuteStateTick();

	}
	
	

	protected virtual void Setup() {

		SetupStates();

		curHp = attribute.maxHp;
		utility.AllCollisionIgnoreSetup();
	}

	protected abstract void SetupStates();



}

}