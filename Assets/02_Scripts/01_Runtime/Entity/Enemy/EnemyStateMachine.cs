using MinD.SO.EnemySO.State;
using UnityEngine;

namespace MinD.Runtime.Entity {

public class EnemyStateMachine : MonoBehaviour {

	[HideInInspector] public Enemy owner;

	private int currentStateIndex;
	private int currentGlobalStateIndex;

	
	
	public void ExecuteStateTick() {
		
		// IF CHARACTER DIED, DON'T EXECUTE METHOD IN A STATES
		if (owner.isDeath) {
			return;
		}

		if (owner.currentState != null) {
			owner.currentState.Tick();
		} else {
			Debug.LogError("!! THE ENEMY'S STATE IS NULL !!");
		}


		if (owner.globalState != null) {
			owner.globalState.Tick();
		} else {
			Debug.LogError("!! THE ENEMY'S GLOBAL STATE IS NULL !!");
		}
	}

	public int GetCurrentStateIndex() {
		return currentStateIndex;
	}
	public int GetCurrentGlobalStateIndex() {
		return currentGlobalStateIndex;
	}
	

	public void ChangeStateByIndex(int stateIndex) {

		EnemyState newState = owner.states[stateIndex];

		if (newState == null)
			return;
		
		
		owner.previousState = owner.currentState;
		if (owner.previousState != null) {
			owner.previousState.Exit();
		}


		owner.currentState = newState;
		owner.currentState.enemy = owner;
		owner.currentState.Enter();

		
		currentStateIndex = stateIndex;
	}
	public void ChangeGlobalStateByIndex(int stateIndex) {
		
		EnemyState newState = owner.globalStates[stateIndex];

		if (newState == null)
			return;

		if (owner.globalState != null) {
			owner.globalState.Exit();
		}
		
		owner.globalState = newState;
		owner.globalState.enemy = owner;
		owner.globalState.Enter();
		
		
		currentGlobalStateIndex = stateIndex;
	}


	public void ExitAllState() {
		
		if (owner.currentState != null) {
			owner.currentState.Exit();
			owner.currentState = null;
		}

		if (owner.previousState) {
			owner.previousState = null;
		}

		if (owner.globalState) {
			owner.globalState.Exit();
			owner.globalState = null;
		}
		
	}
}

}