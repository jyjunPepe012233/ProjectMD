using MinD.Runtime.Entity;
using UnityEngine;

namespace MinD.SO.EnemySO {

[CreateAssetMenu(menuName = "MinD/Enemy SO/FSM/Idle", fileName = "Type_Idle")]
public class IdleState : EnemyState {
	
	[SerializeField] private float detectAngle;
	[SerializeField] private float detectRadius;
	[SerializeField] private float absoluteDetectRadius;
	
	

	public override EnemyState Tick(Enemy self) {
		
		self.animation.LerpMoveDirectionParameter(0, 0);
		
		
		self.currentTarget = self.combat.FindTargetBySight(detectAngle, detectRadius, absoluteDetectRadius);

		// IF TARGET IS DETECTED, SWITCH STATE TO PURSUE TARGET
		if (self.currentTarget == null) {
			return this;
		} else {
			return self.ToHumanoid.pursueTargetState;
		}
		
	}
	
}

}