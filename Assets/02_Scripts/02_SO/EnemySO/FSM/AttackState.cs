using MinD.Runtime.Entity;
using UnityEngine;

namespace MinD.SO.EnemySO {

[CreateAssetMenu(menuName = "MinD/Enemy SO/FSM/Idle", fileName = "Type_Idle")]
public class IdleState : EnemyState {

	public override EnemyState Tick(Enemy self) {
		
		self.animation.LerpMoveDirectionParameter(0, 0);
		
		
		self.currentTarget = self.combat.FindTargetBySight();

		// IF TARGET IS DETECTED, SWITCH STATE TO PURSUE TARGET
		if (self.currentTarget == null) {
			return this;
		} else {
			return self.pursueTargetState;
		}
		
	}
	
}

}