using System.Buffers;
using MinD.Enemys;
using UnityEngine;
using UnityEngine.AI;

namespace MinD.Enemys.DemonOTFFOwnedStates {
		
	public enum States {
		Idle,
		BaseChase,
		ComboAttack1
	}
	
	public class Idle : EnemyState {

		[SerializeField] private float elapsedTime;
		
		public override void Enter(Enemy enemy) {
			
			elapsedTime = 0;


			if (enemy.isInCombat)
				enemy.animation.PlayTargetAction("Combat Locomotion Tree", 0.001f);
			else
				enemy.animation.PlayTargetAction("Base Locomotion Tree", 0.001f);
			
		}

		public override void Tick(Enemy enemy) {
			
			
			// START COMBAT
			BaseEntity target = enemy.combat.FindTargetBySight(110, 23, 6);
			
			if (target != null) {

				if (enemy.isInCombat) {
					
					enemy.stateMachine.ChangeStateByIndex((int)States.BaseChase);
					
				}
				else {
					enemy.animation.PlayTargetAction("Idle To Combat", 0.001f);
					// TRANSFORM FLAGS IN STATEBEHAVIOUR(ResetEnemyFlags)
				}
				
			} else {
				
				// OFF COMBAT MODE
				elapsedTime += Time.deltaTime;
				
				if (elapsedTime > 5 && enemy.isInCombat) {
					enemy.animation.PlayTargetAction("Combat To Idle", 0.001f);
					// TRANSFORM FLAGS IN STATEBEHAVIOUR(ResetEnemyFlags)
				}
			}

		}

		public override void Exit(Enemy enemy) {
		}
		
	}


	public class BaseChase : EnemyState {
		
		public override void Enter(Enemy enemy) {
			enemy.agent.isStopped = false;
			enemy.combat.target = enemy.combat.FindTargetBySight(130, 23, 6);
		}

		public override void Tick(Enemy enemy) {

			if (enemy.combat.target == null) {
				enemy.combat.target = enemy.combat.FindTargetBySight(130, 23, 6);
				
			} else {

				// CHASING LOCOMOTION
				enemy.agent.SetDestination(enemy.combat.target.transform.position);
				Vector3 dirx = enemy.transform.InverseTransformDirection(enemy.agent.desiredVelocity).normalized;
				
				float speed = 0.975f;
				enemy.animation.SetBaseLocomotionParameter(dirx.x * speed, dirx.z * speed);

				
				// STOP BY DISTANCE
				if (Vector3.Distance(enemy.transform.position, enemy.combat.target.transform.position) < 3.5) {
					
					// STOP
					if (enemy.agent.destination != enemy.transform.position) {
						enemy.agent.SetDestination(enemy.transform.position);
						enemy.stateMachine.ChangeStateByIndex((int)States.ComboAttack1);
					}

				}
			}
			
		}

		public override void Exit(Enemy enemy) {
		}
		
	}



	public class ComboAttack1 : EnemyState {
		
		public override void Enter(Enemy enemy) {
			
			enemy.agent.isStopped = true;
			
			
			// PLAY ANIMATION BY PROBABILITY
			bool useFullCombo = (Random.value < 0.5f); // 50%

			if (useFullCombo) {
				enemy.animation.PlayTargetAction("Combo_Attack_01_All", 0.001f, true, true, true);
			} else {
				enemy.animation.PlayTargetAction("Combo_Attack_01_01", 0.001f, true, true, true);
			}
			
		}

		public override void Tick(Enemy enemy) {
			
			// IF ANIMATION IS FINISHED
			if (!enemy.isPerformingAction) {
				enemy.stateMachine.ChangeStateByIndex((int)States.BaseChase);
			}

		}

		public override void Exit(Enemy enemy) {
		}
	}
	
	
	
}