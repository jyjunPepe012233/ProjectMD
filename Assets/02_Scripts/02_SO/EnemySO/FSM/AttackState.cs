using System.Buffers;
using System.Collections.Generic;
using MinD.Runtime.Entity;
using MinD.Runtime.Managers;
using MinD.Runtime.System;
using UnityEditor.Animations;
using UnityEngine;

namespace MinD.SO.EnemySO {

[CreateAssetMenu(menuName = "MinD/Enemy SO/FSM/Attack", fileName = "Type_Attack")]
public class AttackState : EnemyState {

	public override EnemyState Tick(Enemy self) {

		if (!self.isPerformingAction && !self.isInAttack) {
			self.animation.PlayTargetAnimation(self.combat.latestAttack.motionStateName, 0.2f, true, true);

			self.isInAttack = true;
			return self.currentState;

		} else if (!self.isPerformingAction && self.isInAttack) {
			// EXIT ATTACK
			self.isInAttack = false;
			return self.ToHumanoid.combatStanceState;


		} else {
			// IS PERFORMING ACTION (PERFORMING ATTACK)
			self.locomotion.RotateToTarget();
			return self.currentState;

		}
	}


}

}