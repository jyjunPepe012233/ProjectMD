using System;
using UnityEngine;

namespace MinD.Enemys {


	public partial class Infercus : Enemy {
		
		protected override void Setup() {
			
			base.Setup();
			
		}

		protected override void SetupStateList() {

			states = new EnemyState[1];
			states[0] = new Idle();

		}
	}

}