using UnityEditor.Animations;
using UnityEngine;

namespace MinD.Runtime.Entity {

public class EnemyAnimationHandler : BaseEntityAnimationHandler<Enemy> {
	
	

	public void PlayTargetAnimation(string stateName, float normalizedTransformDuration, bool applyRootMotion, bool isPerformingAction) {
		
		owner.animator.CrossFadeInFixedTime(stateName, normalizedTransformDuration);
		
		owner.animator.applyRootMotion = applyRootMotion;
		owner.isPerformingAction = isPerformingAction;
	}

}

}