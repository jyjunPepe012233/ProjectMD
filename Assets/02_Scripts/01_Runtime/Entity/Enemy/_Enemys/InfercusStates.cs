using MinD.Enemys;

namespace MinD.Enemys {

	public partial class Infercus {

		public enum States {
			Idle
		}
		
		public class Idle : EnemyState {
		
			public override void Enter(Enemy enemy) {
				throw new System.NotImplementedException();
			}

			public override void Tick(Enemy enemy) {
				throw new System.NotImplementedException();
			}

			public override void Exit(Enemy enemy) {
				throw new System.NotImplementedException();
			}
			
		}
		
		
		
	}

	
	

}