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
	
}

}