using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MinD;
using MinD.Enemys;
using MinD.StatusFx;
using UnityEngine;
using UnityEngine.Serialization;

namespace MinD {
	
	public abstract class Enemy : BaseEntity {
		
		[Header("[ Current State ]")]
		public EnemyState currentState;
		public EnemyState previousState;
		public EnemyState[] states;

		[Header("[ Current Status ]")]
		public float curHp;

		[Header("[ Attribute Settings ]")]
		public float maxHp;
		
		public EnemyStateMachine stateMachine;
		public EnemyCollisionHandler collision;
		
		
		private void Awake() {

			stateMachine = GetComponent<EnemyStateMachine>();
			collision = GetComponent<EnemyCollisionHandler>();
			
			stateMachine.owner = this;
			collision.owner = this;
			
			Setup();
		}

		protected virtual void Setup() {
			
			SetupStateList();

			WorldEntityManager.Instance.RegisteringEnemyOnWorld(this);

		}

		protected abstract void SetupStateList();

		protected virtual void Update() {
			
			stateMachine.ExecuteStateTick();
			
		}


		public Transform FindTargetBySight(float viewingAngle, float viewingRadius, float unConditionalDetectRadius = 0) {

			Collider[] colliders = Physics.OverlapSphere(transform.position, viewingRadius, PhysicLayerDataBase.Instance.entityLayer);
			
			if (colliders.Length == 0)
				return null;

			
			// MAKE CANDIDATES ARRAY
			List<Transform> candidates = new List<Transform>();

			foreach (Collider collider in colliders)
				foreach (Transform candidate in collider.GetComponent<BaseEntity>().bodyPoint)
					candidates.Add(candidate);
			
			
			// SORTING CANDIDATES ARRAY BY PROXIMITY
			if (candidates.Count > 1) {
				for (int i = 0; i < candidates.Count; i++) {
					// SELECTION SORT

					Transform closest = candidates[i];

					for (int j = i; j < candidates.Count; j++) {

						Transform thisEntity = candidates[j];

						if (Vector3.Distance(transform.position, thisEntity.position) < Vector3.Distance(transform.position, closest.position))
							(candidates[i], candidates[j]) = (candidates[j], candidates[i]);
					}
				}
				
			}
			
			
			// SET TARGET BY UNCONDITIONAL DETECT RADIUS
			if (Vector3.Distance(candidates[0].position, transform.position) < unConditionalDetectRadius)
				return candidates[0];
			

			// SET TARGET BY SIGHT
			foreach (Transform candidate in candidates) {

				if (Vector3.Angle(transform.forward, candidate.position - transform.position) < viewingAngle)
					// IF CANDIDATE IN VIEWING ANGLE
					if (!Physics.Linecast(bodyPoint[0]/* MAIN BODY POINT */.position, candidate.position, PhysicLayerDataBase.Instance.environmentLayer))
						// IF NO OBSTACLE IS CHECKED BETWEEN CANDIDATE AND ENEMY
						return candidate;
			}

			return null;
		}
		
	}
}