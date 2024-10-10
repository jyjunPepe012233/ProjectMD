using MinD.Enemys;
using UnityEngine;

namespace MinD.Enemys {

	public partial class Infercus {

		
		public enum States {
			SleepIdle,
			Idle,
		}

		public class SleepIdle : EnemyState {
			
			public override void Enter(Enemy enemy) {
			}

			public override void Tick(Enemy enemy) {
			}

			public override void Exit(Enemy enemy) {
			}
			
		}
		
		public class Idle : EnemyState {

			public float elapsedInIdle;
				
				
			public override void Enter(Enemy enemy) {
			}

			public override void Tick(Enemy enemy) {

				elapsedInIdle += Time.deltaTime;

				if (elapsedInIdle > 15) {
					enemy.stateMachine.ChangeState((int)States.SleepIdle);
				}
			}

			public override void Exit(Enemy enemy) {
				
				elapsedInIdle = 0;
				
				
			}
			
		}
		
		
		
	}

	
	

}