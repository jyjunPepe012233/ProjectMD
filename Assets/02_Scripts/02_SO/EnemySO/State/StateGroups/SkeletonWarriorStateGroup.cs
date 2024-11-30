namespace MinD.SO.EnemySO.State.StateGroups {

public class SkeletonWarriorStateGroup {

	public enum States {
		Idle,
		PursueTarget,
		LeapAttack,
		Attack1,
		CombatAttack1,
		DodgeBackward,
		GetHit,
	}

	public class Idle : EnemyState {
		public override void Enter() {
			
		}

		public override void Tick() {
			throw new System.NotImplementedException();
		}

		public override void Exit() {
			throw new System.NotImplementedException();
		}
	}
	
}

}