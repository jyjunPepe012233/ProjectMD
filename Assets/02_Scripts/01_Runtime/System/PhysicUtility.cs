using System.Linq;
using MinD.Runtime.Entity;
using UnityEngine;

namespace MinD.Runtime.System {

public static class PhysicUtility {
	
	public static void IgnoreCollisionUtil(BaseEntity entity, Collider collider) {

		Collider[] cols = entity.GetComponentsInChildren<Collider>(true);

		foreach (Collider col in cols) {
			Physics.IgnoreCollision(col, collider);
		}
	}

	public static void SetActiveChildrenColliders(Transform root, bool active, int layerMask = ~0, bool includeInactive = false) {
		
		// GET COLLIDER COMPONENTS IN CHILDREN WHAT LAYER IS INCLUDED IN LAYERMASK PARAMETER
		Collider[] cols = root.GetComponentsInChildren<Collider>(includeInactive)
			.Where(col => layerMask == (layerMask | col.gameObject.layer)) .ToArray();

		foreach (Collider col in cols) {
			col.enabled = active;
		}
		
	}
	
}

}