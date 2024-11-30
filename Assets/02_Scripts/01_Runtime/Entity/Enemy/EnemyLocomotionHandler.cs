using UnityEngine;

namespace MinD.Runtime.Entity {

public class EnemyLocomotionHandler : BaseEntityHandler<Enemy> {

	public bool canMove {
		get => _canMove;
		set {
			_canMove = value;
			owner.navAgent.isStopped = !value;
		}
	}
	private bool _canMove;

	
	
	public void MoveToDestination(float? speed = null) {

		if (!_canMove) {
			return;
		}
		
		Vector3 localVelocity = transform.InverseTransformDirection(owner.navAgent.desiredVelocity);
		localVelocity *= speed ?? owner.attribute.speed;
		
		owner.animation.LerpMoveDirectionParameter(localVelocity.x, localVelocity.z);
	}

	public void HandleAllLocomotion() {

		if (_canMove == false) {
			owner.animation.LerpMoveDirectionParameter(0, 0);
		}
		
	}
}

}