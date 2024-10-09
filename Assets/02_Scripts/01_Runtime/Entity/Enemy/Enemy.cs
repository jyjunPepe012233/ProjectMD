using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MinD;
using MinD.Enemys;
using UnityEngine;
using UnityEngine.Serialization;

namespace MinD {
	
	public abstract class Enemy : BaseEntity {
		
		[Header("[ Current State ]")]
		public EnemyState currentState;
		public EnemyState previousState;
		
		public EnemyStateMachine stateMachine;
		
		
		private void Awake() {

			stateMachine = GetComponent<EnemyStateMachine>();
			stateMachine.owner = this;
			
			Setup();
		}

		protected virtual void Setup() {

			WorldEntityManager.Instance.RegisteringEnemyOnWorld(this);

		}


//		public Transform FindTargetViaSight(float viewingAngle, float viewingRadius, float unConditionalDetectRadius = 0) {
//
//			Collider[] colliders = Physics.OverlapSphere(transform.position, viewingRadius, WorldEntityManager.Instance.entityLayerMask);
//			
//			if (colliders.Length == 0)
//				return null;
//
//			
//			// MAKE CANDIDATES ARRAY
//			List<Transform> candidates = new List<Transform>();
//			
//			foreach (Collider collider in colliders)
//				candidates.Add(collider.transform);
//			
//			
//			// SORTING CANDIDATES ARRAY BY PROXIMITY
//			if (candidates.Count > 1) {
//
//				for (int i = 0; i < candidates.Count; i++) {
//					// SELECTION SORT
//
//					Transform closest = candidates[i];
//
//					for (int j = i; j < candidates.Count; j++) {
//
//						Transform thisEntity = candidates[j];
//
//						if (Vector3.Distance(transform.position, thisEntity.position) < Vector3.Distance(transform.position, closest.position))
//							(candidates[i], candidates[j]) = (candidates[j], candidates[i]);
//					}
//				}
//			}
//			
//			
//			// SET TARGET BY UNCONDITIONAL DETECT RADIUS
//			if (Vector3.Distance(candidates[0].position, transform.position) < unConditionalDetectRadius)
//				return candidates[0];
//			
//
//			// SET TARGET BY SIGHT
//			foreach (Transform candidate in candidates) {
//
//				if (Vector3.Angle(transform.forward, candidate.position - transform.position) < viewingAngle)
//					// IF CANDIDATE IN VIEWING ANGLE
//					return candidate;
//			}
//
//		}
		
	}
}