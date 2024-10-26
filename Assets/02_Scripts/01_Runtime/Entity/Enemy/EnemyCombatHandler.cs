using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using MinD.Runtime.DataBase;


namespace MinD.Runtime.Entity {

public class EnemyCombatHandler : MonoBehaviour {

	[HideInInspector] public Enemy owner;
	
	public BaseEntity target;
	private List<BaseEntity> candidates = new List<BaseEntity>();

	
	
	public float DistanceToTarget() {
		return Vector3.Distance(target.transform.position, owner.transform.position);
	}


	public BaseEntity FindTargetBySight(float viewingAngle, float viewingRadius, float unConditionalDetectRadius = 0) {

		Collider[] colliders = Physics.OverlapSphere(transform.position, viewingRadius, PhysicLayerDataBase.Instance.entityLayer);

		if (colliders.Length == 0)
			return null;


		// MAKE CANDIDATES ARRAY
		candidates.Clear();
		foreach (Collider collider in colliders) {

			var candidate = collider.GetComponentInParent<BaseEntity>();
			if (candidate == null) {

				candidate = GetComponent<BaseEntity>();

				if (candidate == null)
					continue;
			}

			if (candidate == owner)
				continue;

			if (candidates.Contains(candidate))
				continue;

			candidates.Add(candidate);
		}


		// SORTING CANDIDATES ARRAY BY PROXIMITY
		if (candidates.Count > 1) {
			for (int i = 0; i < candidates.Count; i++) {
				// SELECTION SORT

				BaseEntity closest = candidates[i];

				for (int j = i; j < candidates.Count; j++) {

					BaseEntity thisEntity = candidates[j];

					if (Vector3.Distance(transform.position, thisEntity.transform.position) < Vector3.Distance(transform.position, closest.transform.position))
						(candidates[i], candidates[j]) = (candidates[j], candidates[i]);
				}
			}

		} else if (candidates.Count == 0) {
			return null;
		}


		// SET TARGET BY UNCONDITIONAL DETECT RADIUS
		if (Vector3.Distance(candidates[0].transform.position, transform.position) < unConditionalDetectRadius) {
			return candidates[0];
		}


		// SET TARGET BY SIGHT(ANGLE)
		foreach (BaseEntity candidate in candidates) {

			if (Vector3.Angle(transform.forward, candidate.transform.position - transform.position) < viewingAngle)
				// IF CANDIDATE IN VIEWING ANGLE
				if (!Physics.Linecast(owner.targetOptions[0] /* MY MAIN TARGET OPTION */.position, candidate.targetOptions[0].position, PhysicLayerDataBase.Instance.environmentLayer)) {
					// IF NO OBSTACLE IS CHECKED BETWEEN CANDIDATE AND ENEMY
					return candidate;
				}
		}

		return null;
	}

	public void RotateToTarget(float duration) {
		StartCoroutine(RotateToTargetCoroutine(duration));
	}

	private IEnumerator RotateToTargetCoroutine(float duration) {

		Quaternion startRotation = transform.rotation;
		Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
		targetRotation.eulerAngles = new Vector3(0, targetRotation.eulerAngles.y, 0);

		float t = 0;
		while (true) {

			t += Time.deltaTime / duration;

			transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

			if (t > 1) {
				yield break;
			}

			yield return null;
		}
	}


	public void OnDrawGizmos() {

		if (target != null) {

			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(transform.position, target.transform.position);

		}

	}
}

}