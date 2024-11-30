using MinD.Runtime.Entity;
using MinD.Runtime.System;
using MinD.SO.StatusFX.Effects;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.AI;

namespace MinD.SO.EnemySO.State.StateGroups {

public class SkeletonMinerStateGroup {

	public enum States {
		Idle,
		Return,
		PursueTarget,
		TurnToDesire,
		Attack1,
		RunAttack1,
		GetHit,
		Death
	}

	public enum GlobalStates {
		PursueDamage
	}

	public class Idle : EnemyState {

		private static readonly float viewingAngle = 75;
		private static readonly float viewingRadius = 21;
		private static readonly float unConditionalDetectRadius = 5;

		
		public override void Enter() {
			
		}

		public override void Tick() {
			
			self.currentTarget = self.combat.FindTargetBySight(viewingAngle, viewingRadius, unConditionalDetectRadius);
			
			if (self.currentTarget == null) {
				return;
				
			} else {

				NavMeshPath pathToTarget = new NavMeshPath();
				self.navAgent.CalculatePath(self.currentTarget.transform.position, pathToTarget);

				if (pathToTarget.status == NavMeshPathStatus.PathComplete) {
					// CAN REACH TO TARGET
					self.state.ChangeStateByIndex((int)States.PursueTarget);
				}
			}
		}

		public override void Exit() {
			
		}
	}

	public class Return : EnemyState {
		public override void Enter() {
			self.locomotion.canMove = true;
		}

		public override void Tick() {
			
			self.navAgent.SetDestination(self.worldPlacedPosition);
			

			if (self.navAgent.pathStatus != NavMeshPathStatus.PathComplete) {
				self.state.ChangeStateByIndex((int)States.Idle);
				
			} else if (self.navAgent.remainingDistance < 0.2f) {
				self.state.ChangeStateByIndex((int)States.Idle);
				
			} else {
				self.locomotion.MoveToDestination();
				
			}
		}

		public override void Exit() {
			self.locomotion.canMove = false;
		}
	}

	public class PursueTarget : EnemyState {
		
		private static readonly float viewingAngle = 75;
		private static readonly float viewingRadius = 17;
		private static readonly float unConditionalDetectRadius = 6;

		private static readonly float giveUpTargetDistance = 15;
		
		private bool isChasing;
		private float pursueTimeWithoutTarget;
		
		public override void Enter() {
			self.animation.PlayTargetAnimation("Sword_Locomotion_Tree");
		}

		public override void Tick() {
			
			// IF TIME IS ELAPSED WITHOUT TARGET, RETURN TO ORIGIN POSITION
			if (pursueTimeWithoutTarget > 2.5) {
				self.state.ChangeStateByIndex((int)States.Return);
			}
			
			
			if (self.currentTarget == null) {
				self.currentTarget = self.combat.FindTargetBySight(viewingAngle, viewingRadius, unConditionalDetectRadius);
			}
			
			if (self.currentTarget == null) {
				// COULDN'T FIND TARGET
				pursueTimeWithoutTarget += Time.deltaTime;
				
				
			} else if (isChasing) { // HANDLE CHASING
				
				self.navAgent.SetDestination(self.currentTarget.transform.position);
				
				if (self.navAgent.pathStatus != NavMeshPathStatus.PathComplete) {
					pursueTimeWithoutTarget += Time.deltaTime;
					
				} else if (self.navAgent.remainingDistance > giveUpTargetDistance) {
					// REMAINING DISTANCE IS TOO FAR TO TARGET
					pursueTimeWithoutTarget += Time.deltaTime;
					
					
				} else if (Mathf.Abs(self.combat.AngleToDesireOfAgent()) > 80) {
					self.state.ChangeStateByIndex((int)States.TurnToDesire);
					
					
				} else if (self.combat.DistanceToTarget() > 5 &&
				           self.combat.DistanceToTarget() < 6 &&
				           self.combat.AngleToTarget() < 3) {

					if (self.currentTarget is Player player) {
						if (!player.locomotion.isSprinting) {
							self.state.ChangeStateByIndex((int)States.RunAttack1);
						}
						
					} else {
						self.state.ChangeStateByIndex((int)States.RunAttack1);
					}
					
				} else if (self.combat.DistanceToTarget() < 1.8f) {
					self.state.ChangeStateByIndex((int)States.Attack1);

				} else {
					self.locomotion.MoveToDestination();
				}

			} else { // ENEMY HAS TARGET BUT NOT OR CHASING

				if (self.combat.DistanceToTarget() < 1.5f) {
					if (self.combat.AngleToTarget() > 80) {
						self.state.ChangeStateByIndex((int)States.TurnToDesire);
					} else {
						self.state.ChangeStateByIndex((int)States.Attack1);
					}
					
				} else {
					isChasing = true;
					self.locomotion.canMove = true;
				}
				
			}
			
		}

		public override void Exit() {

			isChasing = false;
			self.locomotion.canMove = false;

			pursueTimeWithoutTarget = 0;

		}
	}

	public class TurnToDesire : EnemyState {
		
		public override void Enter() {

			float angleToDesire = self.combat.AngleToDesireOfAgent();
			
			// SET MINIMUM LIMIT THE ABSOLUTE OF ANGLE(USE FOR ANIMATION BLEND FACTOR)
			angleToDesire = angleToDesire > 0 ? Mathf.Max(angleToDesire, 90) : Mathf.Min(angleToDesire, -90);
				
			self.animator.SetFloat("TurnLerpFactor", angleToDesire / 90);
			// DIRECTION OF TURN ANIMATION IS DECIDE BY SIGN OF FACTOR VALUE.
			// BLEND THRESHOLD OF 180 DEGREE TURN IS 2,
			// 90 DEGREE TURN IS 1.
			self.animation.PlayTargetAnimation("Turn_Sword_Tree", 0.2f);
		}

		public override void Tick() {
			if (!self.isPerformingAction) {
				self.state.ChangeStateByIndex((int)States.PursueTarget);
			}
		}

		public override void Exit() {

		}
	}
	
	public class Attack1 : EnemyState {
		public override void Enter() {
			self.animation.PlayTargetAnimation("Attack1", 0.2f);
		}

		public override void Tick() {

			if (!self.isPerformingAction) {
				self.state.ChangeStateByIndex((int)States.PursueTarget);
			}
			
		}

		public override void Exit() {
		}
	}

	public class RunAttack1 : EnemyState {
		public override void Enter() {
			self.animation.PlayTargetAnimation("RunAttack1", 0.1f);
		}

		public override void Tick() {
			if (!self.isPerformingAction) {
				self.state.ChangeStateByIndex((int)States.PursueTarget);
			}
		}

		public override void Exit() {
		}
	}

	public class GetHit : EnemyState {
		public override void Enter() {
			TakeHealthDamage latestDamage = (TakeHealthDamage)self.statusFx.instantEffectSlot;

			int poiseBreakAmount = TakeHealthDamage.GetPoiseBreakAmount(latestDamage.poiseBreakDamage, self.attribute.poiseBreakResistance);
			self.animation.AttemptHumanoidPoiseBreak(poiseBreakAmount, latestDamage.attackAngle);
		}

		public override void Tick() {
			if (!self.isPerformingAction) {
				self.state.ChangeStateByIndex((int)States.PursueTarget);
			}
			
		}

		public override void Exit() {
		}
	}

	public class Death : EnemyState {
		public override void Enter() {
			self.isDeath = true;
			self.isInvincible = true;
			
			self.animation.PlayTargetAnimation("Death", 0.2f);
			PhysicUtility.SetActiveChildrenColliders(self.transform, false, LayerMask.GetMask("Damageable Entity"), false);
		}

		public override void Tick() {
		}

		public override void Exit() {
		}
	}



	public class PursueDamage : EnemyState {
		private void OnGetHit() {
			if (self.CurHp == 0) {
				self.state.ChangeStateByIndex((int)States.Death);
			} else {
				self.state.ChangeStateByIndex((int)States.GetHit);
			}
		}
		public override void Enter() {
			self.getHitAction += OnGetHit;
		}
		public override void Tick() {

		}
		public override void Exit() {
			self.getHitAction -= OnGetHit;
		}
	}
}

}