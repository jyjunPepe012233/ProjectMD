using System;
using UnityEngine;

namespace MinD.Enemys {
	
	public partial class Infercus : Enemy {
		
		protected override void Setup() {
			
			base.Setup();
			
		}

		protected override void SetupStateList() {

			states = new EnemyState[2];
			states[(int)States.SleepIdle] = new SleepIdle();
			states[(int)States.Idle] = new Idle();
			
			stateMachine.ChangeState((int)States.SleepIdle);
			
		}
	}

}