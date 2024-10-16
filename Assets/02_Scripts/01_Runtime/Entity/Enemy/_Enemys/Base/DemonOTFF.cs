using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using MinD;
using MinD.Enemys;
using UnityEngine;

namespace MinD.Enemys {

	public class DemonOTFF : Enemy {

		protected override void SetupStates() {

			states = new EnemyState[3];
			states[(int)DemonOTFFOwnedStates.States.Idle] = new DemonOTFFOwnedStates.Idle();
			states[(int)DemonOTFFOwnedStates.States.BaseChase] = new DemonOTFFOwnedStates.BaseChase();
			states[(int)DemonOTFFOwnedStates.States.ComboAttack1] = new DemonOTFFOwnedStates.ComboAttack1();

			
			stateMachine.ChangeStateByIndex((int)DemonOTFFOwnedStates.States.Idle);
		}
	}

}