using MinD.Runtime.DataBase;
using MinD.Runtime.Object.Magics;
using MinD.SO.EnemySO;
using MinD.SO.EnemySO.States;
using UnityEngine;

namespace MinD.Runtime.Entity.Enemies {

public class DemonOTFF : Enemy {

	[Header("[ DemonOTFF ]")]
	[SerializeField] private GameObject spirit;
	[SerializeField] private Transform spiritSummonPosition;



	protected override void SetupStates() {

		states = new EnemyState[6];
		states[(int)States.Idle] = new Idle();
		states[(int)States.BaseChase] = new BaseChase();
		states[(int)States.ComboAttack1] = new ComboAttack1();
		states[(int)States.RunAttack1] = new RunAttack1();;
		states[(int)States.DodgeBackward] = new DodgeBackward();;
		states[(int)States.SummonDemonFlameSpirit] = new SummonDemonFlameSpirit();;


		stateMachine.ChangeStateByIndex((int)States.Idle);
	}

	public void SummonSpirit() {

		DemonFlameSpirit newSpirit = Instantiate(spirit).GetComponent<DemonFlameSpirit>();

		newSpirit.transform.position = spiritSummonPosition.position;
		newSpirit.Shoot(this, combat.target);
	}
}

}