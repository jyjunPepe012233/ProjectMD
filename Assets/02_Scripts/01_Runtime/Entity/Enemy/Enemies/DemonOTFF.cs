using MinD.Runtime.Object.Magics;
using MinD.SO.EnemySO.State;
using MinD.SO.EnemySO.State.StateGroups;
using UnityEngine;

namespace MinD.Runtime.Entity.Enemies {


public class DemonOTFF : Enemy {

	[Header("[ DemonOTFF ]")]
	[SerializeField] private GameObject spirit;
	[SerializeField] private Transform spiritSummonPosition;



	protected override void SetupStates() {

		states = new EnemyState[7];
		states[(int)DemonOTFFStateGroup.States.Idle] = new DemonOTFFStateGroup.Idle();
		states[(int)DemonOTFFStateGroup.States.BaseChase] = new DemonOTFFStateGroup.BaseChase();
		states[(int)DemonOTFFStateGroup.States.ComboAttack1] = new DemonOTFFStateGroup.ComboAttack1();
		states[(int)DemonOTFFStateGroup.States.RunAttack1] = new DemonOTFFStateGroup.RunAttack1();;
		states[(int)DemonOTFFStateGroup.States.DodgeBackward] = new DemonOTFFStateGroup.DodgeBackward();;
		states[(int)DemonOTFFStateGroup.States.SummonDemonFlameSpirit] = new DemonOTFFStateGroup.SummonDemonFlameSpirit();
		states[(int)DemonOTFFStateGroup.States.GetHit] = new DemonOTFFStateGroup.GetHit();

		globalStates = new EnemyState[1];
		globalStates[(int)DemonOTFFStateGroup.GlobalStates.WaitUntilGetHit] = new DemonOTFFStateGroup.CheckingGetHit();
		
		stateMachine.ChangeStateByIndex((int)DemonOTFFStateGroup.States.Idle);
		stateMachine.ChangeGlobalStateByIndex((int)DemonOTFFStateGroup.GlobalStates.WaitUntilGetHit);
		
	}

	public void SummonSpirit() {

		DemonFlameSpirit newSpirit = Instantiate(spirit).GetComponent<DemonFlameSpirit>();

		newSpirit.transform.position = spiritSummonPosition.position;
		newSpirit.Shoot(this, combat.target);
	}
}

}