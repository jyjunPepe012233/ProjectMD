using UnityEngine;

namespace MinD.Runtime.Entity {

public class PlayerAnimationHandler : BaseEntityHandler<Player> {
	
	private float moveBlendLerpSpeed = 6;
	private float runBlendDampTime = 0.35f;
	
	private Vector2 moveBlend;
	private float runBlend;




	public void HandleAllParameter() {
		HandleLocomotionParameter();
	}

	private void HandleLocomotionParameter() {

		if (!owner.canRotate || !owner.canMove) {
			return;
		}
		
		
		// SET HORIZONTAL AND VERTICAL PARAMETER
		Vector3 localMoveDirx = transform.InverseTransformDirection(owner.locomotion.moveDirx);
		if (owner.isMoving) {
			moveBlend = Vector2.Lerp(moveBlend, new Vector2(localMoveDirx.x, localMoveDirx.z), Time.deltaTime * moveBlendLerpSpeed);
		} else {
			moveBlend = Vector2.Lerp(moveBlend, Vector2.zero, Time.deltaTime * moveBlendLerpSpeed);
		}

		owner.animator.SetFloat("MoveHorizontal", moveBlend.x);
		owner.animator.SetFloat("MoveVertical", moveBlend.y);
		
		
		
		// SET RUN BLEND PARAMETER
		runBlend += (owner.locomotion.isSprinting ? 1 : -1) / runBlendDampTime * Time.deltaTime;
		runBlend = Mathf.Clamp01(runBlend);
			
		owner.animator.SetFloat("RunBlend", runBlend);
	}



	public void PlayTargetAction(string stateName,
		bool isPerformingAnimation,
		bool applyRootMotion = false,
		bool canRotate = true,
		bool canMove = true) {
		
		owner.animator.CrossFadeInFixedTime(stateName, 0.2f);

		owner.animator.applyRootMotion = applyRootMotion;

		owner.isPerformingAction = isPerformingAnimation;
		owner.canRotate = canRotate;
		owner.canMove = canMove;
		// THOSE FLAGS RESET WHEN STATE IS BACK TO 'DEFAULT MOVEMENT'

	}
	
	public void PlayTargetAction(string stateName, float transitionDuration,
		bool isPerformingAnimation,
		bool applyRootMotion = false,
		bool canRotate = true,
		bool canMove = true) {
		
		owner.animator.CrossFadeInFixedTime(stateName, transitionDuration, 0);

		owner.animator.applyRootMotion = applyRootMotion;

		owner.isPerformingAction = isPerformingAnimation;
		owner.canRotate = canRotate;
		owner.canMove = canMove;
		// THOSE FLAGS RESET WHEN STATE IS BACK TO 'DEFAULT MOVEMENT'

	}
}

}