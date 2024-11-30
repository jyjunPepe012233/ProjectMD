using MinD.SO.EnemySO.State;
using MinD.SO.EnemySO.State.StateGroups;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace MinD.Runtime.Entity.Enemies {

public class SkeletonMiner : Enemy {
	
	protected override void SetupStatesArray() {

		states = new EnemyState[8];
		states[(int)SkeletonMinerStateGroup.States.Idle] = new SkeletonMinerStateGroup.Idle();
		states[(int)SkeletonMinerStateGroup.States.Return] = new SkeletonMinerStateGroup.Return();
		states[(int)SkeletonMinerStateGroup.States.PursueTarget] = new SkeletonMinerStateGroup.PursueTarget();
		states[(int)SkeletonMinerStateGroup.States.TurnToDesire] = new SkeletonMinerStateGroup.TurnToDesire();
		states[(int)SkeletonMinerStateGroup.States.Attack1] = new SkeletonMinerStateGroup.Attack1();
		states[(int)SkeletonMinerStateGroup.States.RunAttack1] = new SkeletonMinerStateGroup.RunAttack1();
		states[(int)SkeletonMinerStateGroup.States.GetHit] = new SkeletonMinerStateGroup.GetHit();
		states[(int)SkeletonMinerStateGroup.States.Death] = new SkeletonMinerStateGroup.Death();

		globalStates = new EnemyState[1];
		globalStates[(int)SkeletonMinerStateGroup.GlobalStates.PursueDamage] = new SkeletonMinerStateGroup.PursueDamage();

	}

	public override void Reload() {
		
		base.Reload();
		
		state.ChangeStateByIndex((int)SkeletonMinerStateGroup.States.Idle);
		state.ChangeGlobalStateByIndex((int)SkeletonMinerStateGroup.GlobalStates.PursueDamage);
		
	}

	protected override void OnDeath() {
		
	}
}

}