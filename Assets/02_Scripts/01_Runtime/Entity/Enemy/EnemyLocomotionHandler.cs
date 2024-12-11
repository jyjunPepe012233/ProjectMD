using UnityEngine;

namespace MinD.Runtime.Entity {

public class EnemyLocomotionHandler : BaseEntityHandler<Enemy> {
	
	private bool isMoved;
	
	public void PivotTowards(Vector3 direction) {

		float pivotAngle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
		// CLAMP MINIMUM THE ABSOLUTE VALUE OF PIVOT ANGLE TO 90
		if (Mathf.Abs(pivotAngle) < 90) {
			pivotAngle = Mathf.Sign(pivotAngle) * 90;
		}
		
		owner.animator.SetFloat("Pivot Lerp Factor", pivotAngle / 90);
		// DIRECTION OF PIVOT ANIMATION IS DECIDE BY SIGN OF FACTOR VALUE.
		// BLEND THRESHOLD OF 90 DEGREE TURN IS 1,
		// 180 DEGREE TURN IS 2.
		owner.animation.PlayTargetAnimation("Pivot_BlendTree", 0.2f, true, true);
		
	}

	public void RotateToDesireDirection() {

		if (owner.navAgent.desiredVelocity.Equals(Vector3.zero)) {
			return;
		}
		
		transform.rotation = Quaternion.RotateTowards(
			transform.rotation,
			Quaternion.LookRotation(owner.navAgent.desiredVelocity),
			Time.deltaTime * owner.attribute.angularSpeed
			);
	}
	public void RotateToTarget() {

		transform.rotation = Quaternion.RotateTowards(
			transform.rotation,
			Quaternion.LookRotation(owner.currentTarget.transform.position - transform.position), 
			Time.deltaTime * owner.attribute.angularSpeed
			);

		Debug.DrawRay(transform.position, owner.currentTarget.transform.position - transform.position, Color.red, 1f);
	}
	
	public void ResetMoveDirectionParameter() {
		
		// LERP ANIMATION FACTOR TO ZERO IF ENEMY IS NOT MOVING
		if (!isMoved) {
			owner.animation.LerpMoveDirectionParameter(0, 0);
		}
		isMoved = false;
		
	}
	
	public void MoveToForward(bool run = false) {
		
		owner.animator.SetFloat("Base Locomotion Speed", owner.attribute.moveSpeed);
		owner.animation.LerpMoveDirectionParameter(0, (run ? 2 : 1));

		isMoved = true;
	}

	public void StrafeToward(Vector3 strafeLocalDirection) {

		owner.animator.SetFloat("Base Locomotion Speed", owner.attribute.moveSpeed);
		
		strafeLocalDirection.y = 0;
		strafeLocalDirection.Normalize();
		owner.animation.LerpMoveDirectionParameter(strafeLocalDirection.x, strafeLocalDirection.z);

		isMoved = true;
	}

}

}