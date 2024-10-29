using MinD.Runtime.Utils;
using MinD.Structs;
using UnityEngine;

namespace MinD.Runtime.Object.Magics {

public class LghtOfDrgnSlyngProjectile : MonoBehaviour {

	public DamageCollider damageCollider;

	private Vector3 direction;
	private float speed;
	private float distance;

	private float currentDistance;

	public void Shoot(Vector3 origin, Vector3 direction, float speed, float distance, Damage damage) {

		transform.position = origin;
		this.direction = direction;
		this.speed = speed;
		this.distance = distance;

		currentDistance = 0;

		damageCollider.damageEffect.damage = damage;
	}


	void FixedUpdate() {

		transform.position += direction * (speed * Time.fixedDeltaTime);
		currentDistance += speed * Time.fixedDeltaTime;

		if (currentDistance > distance) {
			Destroy(gameObject); // AFTER, SWITCH TO DESTROY WITH OBJECT POOLING
		}

	}

}

}