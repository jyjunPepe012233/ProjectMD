using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace MinD.Runtime.Entity {

public class EnemyColliderHandler : MonoBehaviour {

	[HideInInspector] public Enemy owner;
	
	public Collider[] bodyColliders;



	public void BeIgnoreCollisionWithMyColliders() {
		
		// RESET ARRAY TO THERE'S ONLY AVAILABLE(RIGHT LAYER) COLLIDER
		bodyColliders = bodyColliders.Where(col => col.gameObject.layer == LayerMask.GetMask("Entity")) .ToArray();
		
		foreach (Collider collider1 in bodyColliders) {
				foreach (Collider collider2 in bodyColliders) {
					Physics.IgnoreCollision(collider1, collider2);
				}
		}
	}

}

}