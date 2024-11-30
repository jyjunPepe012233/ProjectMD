using UnityEngine;

namespace MinD.Runtime.Entity {

public class EnemyAnimationHandler : BaseEntityAnimationHandler<Enemy> {

	public void PlayTargetAnimation(string stateName, float normalizedTransformDuration = 0.2f) {

		owner.animator.CrossFadeInFixedTime(stateName, normalizedTransformDuration);

	}

}

}