using UnityEngine;
using UnityEngine.Serialization;

namespace MinD {

	public class EnemyCollisionHandler : MonoBehaviour {

		[HideInInspector] public Enemy owner;
		
		public FunctionColliderHandler[] registeredColliders;


		public void TurnOnCollider(string colliderName) {
			
			foreach (FunctionColliderHandler collider in registeredColliders) {

				if (collider.gameObject.name == colliderName) {
					collider.GetComponent<Collider>().enabled = true;
				}
				
			}
		}
		
		public void TurnOffCollider(string colliderName) {

			foreach (FunctionColliderHandler collider in registeredColliders) {

				if (collider.gameObject.name == colliderName) {
					collider.ResetTargetsInAllColliders();
					collider.GetComponent<Collider>().enabled = false;
				}
				
			}
		}

		public void ResetCollider(string colliderName) {
			
			foreach (FunctionColliderHandler collider in registeredColliders) {

				if (collider.name == colliderName) {
					collider.ResetTargetsInAllColliders();
				}
				
			}
			
		}

	}

}