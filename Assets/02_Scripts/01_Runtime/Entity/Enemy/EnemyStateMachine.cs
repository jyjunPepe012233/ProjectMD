using MinD.SO.EnemySO;

namespace MinD.Runtime.Entity {

public class EnemyStateMachine : BaseEntityHandler<Enemy> {

	public void SetState(EnemyState nextState) {

		if (nextState == null) {
			return;
		}
		
		if (nextState != owner.currentState) {
			owner.currentState = nextState;
		}
	}
	
	public void HandleState() {

		if (owner.currentState == null) {
			return;
		}

		SetState(owner.currentState.Tick(owner));
	}
	
}

}