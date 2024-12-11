using MinD.Runtime.Entity;
using UnityEngine;

namespace MinD.SO.EnemySO {

[CreateAssetMenu(menuName = "MinD/Enemy SO/FSM/Pursue Target", fileName = "Type_PursueTarget")]
public class PursueTargetState : EnemyState {
	
	[SerializeField] private float giveUpDistance;
	[SerializeField] private float enterCombatStanceRadius;

	

	public override EnemyState Tick(Enemy self) {

		// IF IN PIVOT, STAY IN PURSUE TARGET STATE
		if (self.isPerformingAction) {
			return self.currentState;
		}

		
		// IF TARGET IS NOT EXIST IN DETECT RANGE
		if (self.currentTarget == null) {
			return self.ToHumanoid.idleState;
		}
		

		self.navAgent.SetDestination(self.currentTarget.transform.position);
		
		// SWITCH STATE TO IDLE REMAINING PATH DISTANCE IS LONGER THAN GIVE UP DISTANCE 
		if (self.navAgent.remainingDistance > giveUpDistance) {
			return self.ToHumanoid.idleState;
		}
		
		// PIVOT(TURN) TO DESIRE DIRECTION AND KEEP UP THIS STATE
		if (Mathf.Abs(self.combat.AngleToDesireDirection()) > 80) {
			self.locomotion.PivotTowards(self.navAgent.desiredVelocity);
			return self.currentState;
		}

		// SWITCH STATE TO COMBAT STANCE STATE IF TARGET IS IN COMBAT RANGE
		if (Vector3.Distance(self.transform.position, self.currentTarget.transform.position) < enterCombatStanceRadius) {
			return self.ToHumanoid.idleState;
		}
		
		// CHASING
		self.locomotion.RotateToDesireDirection();
		self.locomotion.MoveToForward();
		return self.currentState;
	}
	
}

}