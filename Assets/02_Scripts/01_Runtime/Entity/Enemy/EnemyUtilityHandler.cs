using System.Linq;
using MinD.Runtime.System;
using MinD.Runtime.Utils;
using UnityEngine;

namespace MinD.Runtime.Entity {

public class EnemyUtilityHandler : BaseEntityHandler<Enemy> {
	
	[Space(10)]
	[SerializeField] private GameObject[] ownedObjects;
	[SerializeField] private GameObject[] prefabs;
	
	
	
	public void AllCollisionIgnoreSetup() {
		
		// RESET ARRAY TO THERE'S ONLY AVAILABLE(RIGHT LAYER) COLLIDER
		Collider[] allCols = GetComponentsInChildren<Collider>(true);

		
		// IGNORE BODY COLLIDERS WITH OWNED DAMAGE COLLIDER
		var ownedDamageCols = allCols.Where(c => c.gameObject.layer == LayerMask.NameToLayer("DamageCollider"));
		
		foreach (Collider col in ownedDamageCols) {
			PhysicUtility.IgnoreCollisionUtil(owner, col);
		}
		
	}


	public GameObject InstantiateObject(string prefabName) {
		foreach (GameObject obj in prefabs) {
			if (obj.name == prefabName) {
				return Instantiate(obj);
			}
		}
		
		throw new UnityException("!! CAN'T FIND " + owner.name + " REGISTERED PREFAB THE NAMED " + prefabName);
	}
	
	public void EnableObject(string targetObjects) {
		// MULTIPLE OBJECTS ARE SEPARATE BY SPACE 

		string[] targetObjNames = targetObjects.Split();

		foreach (string targetObjName in targetObjNames) {

			bool findTarget = false;

			foreach (GameObject obj in ownedObjects) {
				if (obj.name == targetObjName) {
					obj.SetActive(true);
					findTarget = true;
				}
			}

			if (!findTarget) {
				throw new UnityException("!! CAN'T FIND " + owner.name + " OWNED OBJECT THE NAMED " + targetObjNames);
			}
		}

	}

	public void DisableObject(string targetObjects) {
		// MULTIPLE OBJECTS ARE SEPARATE BY SPACE 

		string[] targetObjNames = targetObjects.Split();

		foreach (string targetObjName in targetObjNames) {

			bool findTarget = false;

			foreach (GameObject obj in ownedObjects) {
				if (obj.name == targetObjName) {
					obj.SetActive(false);
					findTarget = true;
				}
			}

			if (!findTarget) {
				throw new UnityException("!! CAN'T FIND " + owner.name + " OWNED OBJECT THE NAMED " + targetObjNames);
			}
		}
	}
	
	
	public void ResetDamageColliderToHitAgain(string colliderName) {

		foreach (GameObject obj in ownedObjects) {

			if (obj.name == colliderName) {
				var cols = obj.GetComponentsInChildren<DamageCollider>();
				foreach (DamageCollider col in cols) {
					col.ResetToHitAgain();
				}

				return;
			}

		}

		throw new UnityException("!! CAN'T FIND " + owner.name + " OWNED DAMAGE COLLIDER THE NAMED " + colliderName);
	}





}

}