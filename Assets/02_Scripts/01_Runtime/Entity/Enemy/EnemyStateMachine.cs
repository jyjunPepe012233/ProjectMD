using UnityEngine;

namespace MinD {

	public class EnemyStateMachine : MonoBehaviour {

		public Enemy owner;



		public void ExecuteStateTick() {

			if (owner.currentState != null)
				owner.currentState.Tick(owner);
			else
				Debug.LogError("!! THE ENEMY'S STATE IS NULL !!");
			
		}
		
		public void ChangeState(EnemyState newState) {

			if (newState == null)
				return;

			if (owner.previousState != null) {
				
				owner.previousState = owner.currentState;
				owner.previousState.Exit(owner);
				
			}

			owner.currentState = newState;
			owner.currentState.Enter(owner);

		}
		
		
		
	}

}