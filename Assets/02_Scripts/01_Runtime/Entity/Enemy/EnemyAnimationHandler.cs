namespace MinD.Runtime.Entity {

public class EnemyAnimationHandler : BaseEntityAnimationHandler {
	
	

	public void PlayTargetAnimation(string stateName, float normalizedTransformDuration, bool applyRootMotion, bool isPerformingAction) {
	
		owner.animator.CrossFadeInFixedTime(stateName, normalizedTransformDuration);
		
		owner.animator.applyRootMotion = applyRootMotion;
		((Player)owner).isPerformingAction = isPerformingAction;
	}

}

}