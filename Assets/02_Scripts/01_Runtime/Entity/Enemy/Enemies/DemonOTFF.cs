using MinD.Runtime.DataBase;
using MinD.Runtime.Object.Magics;
using MinD.SO.EnemyState;
using MinD.SO.EnemyState.States.DemonOTFF;
using UnityEngine;

namespace MinD.Runtime.Entity.Enemies {

public class DemonOTFF : Enemy {

	[Header("[ DemonOTFF ]")]
	[SerializeField] private Transform spiritSummonPosition;


	protected override void SetupStates() {

		states = new EnemyState[6];
		states[(int)States.Idle] = ScriptableObject.CreateInstance<Idle>();
		states[(int)States.BaseChase] = ScriptableObject.CreateInstance<BaseChase>();
		states[(int)States.ComboAttack1] = ScriptableObject.CreateInstance<ComboAttack1>();
		states[(int)States.RunAttack1] = ScriptableObject.CreateInstance<RunAttack1>();;
		states[(int)States.DodgeBackward] = ScriptableObject.CreateInstance<DodgeBackward>();;
		states[(int)States.SummonDemonFlameSpirit] = ScriptableObject.CreateInstance<SummonDemonFlameSpirit>();;


		stateMachine.ChangeStateByIndex((int)States.Idle);
	}

	public void SummonSpirit() {

		DemonFlameSpirit newSpirit = ObjectDataBase.Instance.InstantiateMagic("DemonFlame_Spirit").GetComponent<DemonFlameSpirit>();

		newSpirit.transform.position = spiritSummonPosition.position;
		newSpirit.Shoot(this, combat.target);
	}
}

}