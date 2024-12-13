using System.Collections.Generic;
using System.Linq;
using MinD.Runtime.Entity;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MinD.SO.EnemySO {

public abstract class CombatStanceState : EnemyState {

	[SerializeField] private EnemyAttackAction[] attackActions;
	
	[Space(10)]
	[SerializeField] protected float exitCombatStanceRadius;
	
	

	protected AttackState AttemptAttackAction(Enemy self, EnemyAttackAction thisAttack) {
		
		self.combat.attackActionRecoveryTimer = 0;
		((HumanoidEnemy)self).strafeTimer = 0;
		((HumanoidEnemy)self).isDashingToTarget = false;

		self.combat.latestAttack = thisAttack;
		// DECIDE WHETHER TO USE COMBO
		self.combat.willPerformCombo = Random.value < thisAttack.chanceToCombo && thisAttack.canPerformCombo;
		self.combat.comboAttack = self.combat.willPerformCombo ? thisAttack.comboAttack : null;

		return self.attackState;
	}
	
	protected EnemyAttackAction GetAttackActionRandomly(Enemy self) {
		
		bool CanIUseThisAttack(EnemyAttackAction action) {

			if (action.minAngle > self.combat.AngleToTarget() ||
			    action.maxAngle < self.combat.AngleToTarget()) {
				return false;
			}
			if (action.minDistance > self.combat.DistanceToTarget() ||
			    action.maxDistance < self.combat.DistanceToTarget()) {
				return false;
			}
			
			// TRYING REPEAT ACTION
			if (!action.canRepeatAction && action == self.combat.latestAttack) {
				return false;
			}

			return true;
		}


		// IF NEXT COMBO ATTACK IS RESERVED
		if (self.combat.willPerformCombo) {
			
			if (CanIUseThisAttack(self.combat.comboAttack)) {
				return self.combat.comboAttack;
			}
			return null;
 		}
		
		
		// IF NO COMBO IS RESERVED
		List<EnemyAttackAction> availableAttacks = new List<EnemyAttackAction>();
		for (int i = 0; i < attackActions.Length; i++) {

			if (CanIUseThisAttack(attackActions[i])) {
				availableAttacks.Add(attackActions[i]);
			}
		}
		if (availableAttacks.Count == 0) {
			return null;
		}


		float totalWeight = availableAttacks.Sum(i => i.actionWeight);
		float randomPointOnWeight = Random.Range(0, totalWeight);
		
		float w = 0;
		for (int i = 0; i < availableAttacks.Count; i++) {
			
			w += availableAttacks[i].actionWeight;
			
			if (w > randomPointOnWeight) {
				return availableAttacks[i];
			}
		}
		
		throw new UnityException();
	}
	
}

}