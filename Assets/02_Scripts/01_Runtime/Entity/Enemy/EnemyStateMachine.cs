using UnityEngine;

namespace MinD {

	public class EnemyStateMachine : MonoBehaviour {

		[HideInInspector] public Enemy owner;

		[SerializeField] private string currentState; // TO SHOW STATE IN INSPECTOR



		public void ExecuteStateTick() {

			if (owner.currentState != null)
				owner.currentState.Tick(owner);
			else
				Debug.LogError("!! THE ENEMY'S STATE IS NULL !!");
			
		}
		
		public void ChangeStateByIndex(int stateIndex) {

			EnemyState newState = owner.states[stateIndex];
			
			if (newState == null)
				return;

			if (owner.previousState != null) {
				
				owner.previousState = owner.currentState;
				owner.previousState.Exit(owner);
				
			}

			owner.currentState = newState;
			owner.currentState.Enter(owner);

			currentState = owner.currentState.GetType().Name;
		}
		
		
		
	}

}