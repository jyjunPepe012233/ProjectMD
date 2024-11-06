using MinD.Runtime.Entity;
using MinD.SO.StatusFX.Effects;
using MinD.SO.Utils;
using UnityEngine;

namespace MinD.SO.EnemySO.State.StateGroups {

public class DemonOTFFStateGroup {
	public enum States {
		Idle,
		BaseChase,
		ComboAttack1,
		RunAttack1,
		DodgeBackward,
		SummonDemonFlameSpirit,
		GetHit
	}

	public enum GlobalStates {
		WaitUntilGetHit
	}
	

	public class Idle : EnemyState {

		[SerializeField] private float elapsedTime;

		public override void Enter() {

			elapsedTime = 0;


			if (enemy.isInCombat)
				enemy.animation.PlayTargetAnimation("Combat Locomotion Tree", 0.1f);
			else
				enemy.animation.PlayTargetAnimation("Base Locomotion Tree", 0.1f);

		}

		public override void Tick() {

			// START COMBAT
			enemy.combat.target = enemy.combat.FindTargetBySight(110, 23, 6);

			if (enemy.combat.target != null) {

				if (enemy.isInCombat) {

					enemy.stateMachine.ChangeStateByIndex((int)States.ComboAttack1);

				} else {
					enemy.animation.PlayTargetAnimation("Idle To Combat", 0.001f);
					// TRANSFORM FLAGS IN STATEBEHAVIOUR(ResetEnemyFlags)
				}

			} else {

				// OFF COMBAT MODE
				elapsedTime += Time.deltaTime;

				if (elapsedTime > 5 && enemy.isInCombat) {
					enemy.animation.PlayTargetAnimation("Combat To Idle", 0.001f);
					// TRANSFORM FLAGS IN STATEBEHAVIOUR(ResetEnemyFlags)
				}
			}

		}

		public override void Exit() {
		}

	}
	
	public class BaseChase : EnemyState {

		public override void Enter() {
			enemy.navAgent.isStopped = false;
		}

		public override void Tick() {

			enemy.combat.target = enemy.combat.FindTargetBySight(130, 23, 13);

			if (enemy.combat.target == null) {

				enemy.stateMachine.ChangeStateByIndex((int)States.Idle);

			} else {

				// CHASING LOCOMOTION
				enemy.navAgent.SetDestination(enemy.combat.target.transform.position);
				Vector3 dirx = enemy.transform.InverseTransformDirection(enemy.navAgent.desiredVelocity).normalized;

				float speed = 0.975f;
				enemy.animation.SetBaseLocomotionParameter(dirx.x * speed, dirx.z * speed);


				// JUDGE NEXT ATTACK BY DISTANCE
				if (enemy.combat.DistanceToTarget() < 9.5f) {

					if (Random.value < 0.5f) {
						// 50%
						enemy.stateMachine.ChangeStateByIndex((int)States.RunAttack1);
					} else {
						enemy.stateMachine.ChangeStateByIndex((int)States.SummonDemonFlameSpirit);
					}

				} else if (enemy.combat.DistanceToTarget() < 4) {
					enemy.stateMachine.ChangeStateByIndex((int)States.ComboAttack1);
				}
			}

		}

		public override void Exit() {
		}

	}
	
	public class ComboAttack1 : EnemyState {

		public override void Enter() {

			enemy.navAgent.isStopped = true;


			// PLAY ANIMATION BY PROBABILITY
			bool useFullCombo = (Random.value < 0.8f); // 80%

			if (useFullCombo) {
				enemy.animation.PlayTargetAnimation("Combo_Attack_01_All", 0.001f);
			} else {
				enemy.animation.PlayTargetAnimation("Combo_Attack_01_01", 0.001f);
			}

		}

		public override void Tick() {

			// IF ANIMATION IS FINISHED
			if (!enemy.isPerformingAction) {

				// TARGET IS IN RADIUS 4
				if (enemy.combat.DistanceToTarget() < 6f)
					enemy.stateMachine.ChangeStateByIndex((int)States.DodgeBackward);
				else
					enemy.stateMachine.ChangeStateByIndex((int)States.BaseChase);
			}

		}

		public override void Exit() {

		}

	}
	
	public class RunAttack1 : EnemyState {

		public override void Enter() {

			enemy.combat.target = enemy.combat.FindTargetBySight(130, 20, 11);

			if (enemy.combat.target != null) {

				if (enemy.previousState is BaseChase) {
					enemy.animation.PlayTargetAnimation("Run_Attack_01", 0.01f);
				} else {
					enemy.animation.PlayTargetAnimation("Run_Attack_01", 0.1f);
				}

			}

		}

		public override void Tick() {

			// IF ANIMATION IS OVER, JUDGE NEXT STATE
			if (!enemy.isPerformingAction) {
				enemy.stateMachine.ChangeStateByIndex((int)States.BaseChase);
			}

		}

		public override void Exit() {

		}

	}
	
	public class DodgeBackward : EnemyState {

		public override void Enter() {

			enemy.navAgent.isStopped = true;

			enemy.combat.RotateToTarget(0.1f);
			enemy.animation.PlayTargetAnimation("Dodge_Combat_B", 0.001f);

		}

		public override void Tick() {

			// IF ANIMATION IS OVER, JUDGE THE NEXT MOVE
			if (!enemy.isPerformingAction) {

				if (enemy.combat.DistanceToTarget() < 4)
					enemy.stateMachine.ChangeStateByIndex((int)States.DodgeBackward);
				else {

					if (Random.value < 0.4f) {
						// 40%
						enemy.stateMachine.ChangeStateByIndex((int)States.BaseChase);
					} else {
						enemy.stateMachine.ChangeStateByIndex((int)States.RunAttack1);
					}

				}
			}

		}

		public override void Exit() {
		}

	}
	
	public class SummonDemonFlameSpirit : EnemyState {

		public override void Enter() {

			enemy.combat.target = enemy.combat.FindTargetBySight(130, 20, 12);

			// CHECK TARGET IS EXIST
			if (enemy.combat.target == null)
				enemy.stateMachine.ChangeStateByIndex((int)States.Idle);
			else {

				if (enemy.previousState is BaseChase)
					enemy.animation.PlayTargetAnimation("Summon Spirit", 0.001f);
				else
					enemy.animation.PlayTargetAnimation("Summon Spirit", 0.1f);

			}

		}

		public override void Tick() {

			if (!enemy.isPerformingAction) {
				// WAIT THAN EXIT ANIMATION
				enemy.stateMachine.ChangeStateByIndex((int)States.ComboAttack1);
			}

		}

		public override void Exit() {

		}
	}

	public class GetHit : EnemyState {
		
		public override void Enter() {

			// ATTEMPT POISE BREAK
			TakeHealthDamage thisHit = (TakeHealthDamage)enemy.statusFx.instantEffectSlot;
			int poiseBreakAmount = TakeHealthDamage.GetPoiseBreakAmount(thisHit.poiseBreakDamage, enemy.attribute.poiseBreakResistance);
			
			enemy.animation.AttemptHumanoidPoiseBreak(poiseBreakAmount, thisHit.hitAngle);
		}

		public override void Tick() {
			
			if (!enemy.isPerformingAction) {
				enemy.stateMachine.ChangeStateByIndex((int)States.Idle);
			}
			
		}

		public override void Exit() {
		}
	}
	


	public class CheckingGetHit : EnemyState {

		private void OnGetHit() {
			
			// IS ALREADY PLAYING POISE BREAK, DON'T CHANGE STATE
			if (!enemy.immunePoiseBreak &&
			    enemy.stateMachine.GetCurrentStateIndex() != (int)States.GetHit) {
				
				enemy.stateMachine.ChangeStateByIndex((int)States.GetHit);
			}
			
		}
		
		public override void Enter() {
			
			enemy.getHitAction += OnGetHit;
			
		}

		public override void Tick() {
			
		}

		public override void Exit() {

			enemy.getHitAction -= OnGetHit;

		}
	}
}

}