using System;
using System.Collections.Generic;
using System.Linq;
using MinD.Runtime.Entity;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MinD.SO.EnemySO {

[CreateAssetMenu(menuName = "MinD/Enemy SO/FSM/Combat Stance", fileName = "Type_CombatStance")]
public class CombatStanceState : EnemyState {

	public EnemyAttackAction[] attackActions;
	
	[Space(10)]
	[SerializeField] private float combatPursueMinDistance;
	[SerializeField] private float combatPursueMaxDistance;
	[Space(10)]
	[SerializeField] private float exitCombatStanceRadius;

	[Space(10)]
	[SerializeField] private float maxStrafeTime; // WHEN STRAFE TIMER IS OVER MAXIMUM, START DASHING TO TARGET
	

	public override EnemyState Tick(Enemy self) {

		if (self.isPerformingAction) {
			return self.currentState;
		}

		if (self.currentTarget.isDeath) {
			return self.ToHumanoid.pursueTargetState;
		}

		if (self.combat.DistanceToTarget() > exitCombatStanceRadius) {
			return self.ToHumanoid.pursueTargetState;
		}
		
		self.navAgent.SetDestination(self.currentTarget.transform.position);
		
		
		// DASHING TO TARGET STATE
		if (self.combat.isDashingToTarget) {
			self.locomotion.RotateToDesireDirection();
			self.locomotion.MoveToForward(true);

			if (self.combat.DistanceToTarget() < combatPursueMinDistance) {
				self.combat.isDashingToTarget = false;
			}
		}
		
		
		

		// CHECK NEXT ATTACK
		if (self.combat.attackActionRecoveryTimer < (self.combat.latestAttack != null ? self.combat.latestAttack.actionRecoveryTime : 0) ) {
			// IS IN RECOVERY
			self.combat.attackActionRecoveryTimer += Time.deltaTime;
			
		} else {
			
			var nextAttack = GetAttackActionRandomly(self);
			if (nextAttack != null) {
				
				self.combat.attackActionRecoveryTimer = 0;
				self.combat.strafeTimer = 0;
				self.combat.isDashingToTarget = false;

				self.combat.latestAttack = nextAttack;
				// DECIDE WHETHER TO USE COMBO
				self.combat.willPerformCombo = Random.value < nextAttack.chanceToCombo && nextAttack.canPerformCombo;
				self.combat.comboAttack = self.combat.willPerformCombo ? nextAttack.comboAttack : null;
				
				return self.ToHumanoid.attackState;
			}
		}
		
		
		
		// IF AVAILABLE STATE IS NOT EXISTS,
		// PURSUE PROPER DISTANCE TO COMBAT
		Vector3 strafeDirx = Vector3.zero;
		
		if (self.combat.DistanceToTarget() < combatPursueMinDistance) {
			strafeDirx.z = -1;
		} else if (self.combat.DistanceToTarget() > combatPursueMaxDistance) {
			strafeDirx.z = 1;
		}

		
		Vector3 targetLocalMoveDirx = self.currentTarget.transform.InverseTransformDirection(self.currentTarget.cc.velocity.normalized);
		
		if (targetLocalMoveDirx.x > 0.5) { // TARGET IS ON RIGHT
			strafeDirx.x = 1;
		} else if (targetLocalMoveDirx.x < -0.5) { // TARGET IS ON LEFT
			strafeDirx.x = -1;
		}
		
		
		// WHEN STRAFE TIMER IS OVER MAXIMUM, START DASHING TO TARGET
		self.combat.strafeTimer += Time.deltaTime;
		if (self.combat.strafeTimer > maxStrafeTime) {
			self.combat.isDashingToTarget = true;
		}
		
		
		if (!strafeDirx.Equals(Vector3.zero)) {
			self.locomotion.StrafeToward(strafeDirx);
		}
		self.locomotion.RotateToDesireDirection();
		
		
		
		return self.currentState;
	}

	
	
	private EnemyAttackAction GetAttackActionRandomly(Enemy self) {
		
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