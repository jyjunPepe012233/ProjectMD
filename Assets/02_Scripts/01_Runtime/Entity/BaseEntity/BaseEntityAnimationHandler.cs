using JetBrains.Annotations;
using UnityEngine;

namespace MinD.Runtime.Entity {

public class BaseEntityAnimationHandler<TOwner> : BaseEntityHandler<TOwner> where TOwner : BaseEntity {

	[SerializeField] private float moveDirectionLerpSpeed = 6;
	private Vector2 locomotionParam;
	
	public void LerpMoveDirectionParameter(float horizontal, float vertical) {

		locomotionParam = Vector2.Lerp(locomotionParam, new Vector2(horizontal, vertical), Time.deltaTime * 6f);
		foreach (AnimatorControllerParameter parameter in owner.animator.parameters) {

			if (parameter.name == "Horizontal")
				owner.animator.SetFloat("Horizontal", locomotionParam.x);

			if (parameter.name == "Vertical")
				owner.animator.SetFloat("Vertical", locomotionParam.y);
		}

	}
	
	
	[CanBeNull]
	public string GetPoiseBreakAnimation(int poiseBreakAmount, float hitAngle) {
		// BASIC OF THIS FUNCTION IS HUMANOID POISE BREAK ATTEMPTING 
	
	
		// DECIDE DIRECTION OF POISE BREAK ANIMATION BY HIT DIRECTION
		string hitDirection;
		// SET HIT DIRECTION	
		if (hitAngle >= -45 && hitAngle < 45) {
			hitDirection = "F";

		} else if (hitAngle >= 45 && hitAngle < 135) {
			hitDirection = "R";
		
		}  else if (hitAngle >= 135 && hitAngle < -135) {
			hitDirection = "B";
		
		} else {
			hitDirection = "L";
		}
	
	
	
		string stateName = "Hit_";

		// DECIDE ANIMATION BY CALCULATED POISE BREAK AMOUNT
		if (poiseBreakAmount >= 80) {
			stateName += "KnockDown_Start";
		
			Vector3 angle = transform.eulerAngles;
			angle.y += hitAngle;
			transform.eulerAngles = angle;

		} else if (poiseBreakAmount >= 55) {
			stateName += "Large_";
			stateName += hitDirection;

		} else if (poiseBreakAmount >= 20) {
			stateName += "Default_";
			stateName += hitDirection;
		
		} else {
			return null; // IF POISE BREAK AMOUNT IS BELOW TO 20, POISE BREAK DOESN'T OCCUR
		}

		return stateName;
	}
	
}

}