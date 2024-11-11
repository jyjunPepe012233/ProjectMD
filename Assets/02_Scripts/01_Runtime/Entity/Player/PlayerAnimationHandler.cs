using UnityEngine;

namespace MinD.Runtime.Entity {

public class PlayerAnimationHandler : MonoBehaviour {

	[HideInInspector] public Player owner;
	
	private float moveBlendLerpSpeed = 6;
	private Vector2 moveBlend;

	

	public void Awake() {
		owner = GetComponent<Player>();
	}


	public void LerpMovementBlendTree(float horizontal, float vertical) {

		moveBlend = Vector2.Lerp(moveBlend, new Vector2(horizontal, vertical), Time.deltaTime * moveBlendLerpSpeed);

		owner.animator.SetFloat("MoveHorizontal", moveBlend.x);
		owner.animator.SetFloat("MoveVertical", moveBlend.y);
	}



	public void PlayTargetAction(string stateName,
		bool isPerformingAnimation,
		bool applyRootMotion = false,
		bool canRotate = true,
		bool canMove = true) {

		float normalizedTransitionDuration = 0.2f / owner.animator.GetCurrentAnimatorStateInfo(0).length;
		owner.animator.CrossFade(stateName, normalizedTransitionDuration);

		owner.animator.applyRootMotion = applyRootMotion;

		owner.isPerformingAction = isPerformingAnimation;
		owner.canRotate = canRotate;
		owner.canMove = canMove;
		// THOSE FLAGS RESET WHEN STATE IS BACK TO 'DEFAULT MOVEMENT'

	}
	
	public void PlayTargetAction(string stateName, float absoluteTransitionDuration,
		bool isPerformingAnimation,
		bool applyRootMotion = false,
		bool canRotate = true,
		bool canMove = true) {

		float normalizedTransitionDuration = absoluteTransitionDuration / owner.animator.GetCurrentAnimatorStateInfo(0).length;
		owner.animator.CrossFade(stateName, normalizedTransitionDuration);

		owner.animator.applyRootMotion = applyRootMotion;

		owner.isPerformingAction = isPerformingAnimation;
		owner.canRotate = canRotate;
		owner.canMove = canMove;
		// THOSE FLAGS RESET WHEN STATE IS BACK TO 'DEFAULT MOVEMENT'

	}
}

}