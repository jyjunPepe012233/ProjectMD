using UnityEngine;

namespace MinD {

	public class EnemyAnimationHandler : MonoBehaviour {

		[HideInInspector] public Enemy owner;

		private Vector2 locomotionParam;
		

		
		public void SetBaseLocomotionParameter(float horizontal, float vertical) {

			locomotionParam = Vector2.Lerp(locomotionParam, new Vector2(horizontal, vertical), Time.deltaTime * 6f);
			
			
			foreach (AnimatorControllerParameter parameter in owner.animator.parameters) {
				
				if (parameter.name == "Horizontal")
					owner.animator.SetFloat("Horizontal", locomotionParam.x);
				
				if (parameter.name == "Vertical")
					owner.animator.SetFloat("Vertical", locomotionParam.y);
			}
			
		}

		public void PlayTargetAction(
			string stateName,
			float normalizedTransformDuration = 0.2f,
			bool isPerformingAction = true,
			bool applyRootMotion = true,
			bool isInCombat = true) {
			
			owner.animator.CrossFade(stateName, normalizedTransformDuration);

		}
	}

}