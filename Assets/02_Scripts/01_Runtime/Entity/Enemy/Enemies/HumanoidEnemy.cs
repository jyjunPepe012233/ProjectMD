using MinD.SO.EnemySO;
using MinD.SO.StatusFX.Effects;
using UnityEngine;

namespace MinD.Runtime.Entity {

public abstract class HumanoidEnemy : Enemy {

	private bool isKnockedDown;
	
	public IdleState idleState; 
	public PursueTargetState pursueTargetState;
	public CombatStanceState combatStanceState;
	public AttackState attackState;
	
	public override void OnDamaged(TakeHealthDamage damage) {
		
		// DECIDE DIRECTION OF POISE BREAK ANIMATION BY HIT DIRECTION
		Vector2 hitDirection = Vector2.zero;
		// SET HIT DIRECTION	
		if (damage.attackAngle >= -45 && damage.attackAngle < 45) {
			hitDirection.y = 1;

		} else if (damage.attackAngle >= 45 && damage.attackAngle < 135) {
			hitDirection.x = 1;
			
		}  else if (damage.attackAngle >= 135 && damage.attackAngle < -135) {
			hitDirection.y = -1;
			
		} else {
			hitDirection.x = -1;
		}


		int actionAmount = 0;

		// DECIDE ANIMATION BY CALCULATED POISE BREAK AMOUNT
		int poiseBreakAmount = TakeHealthDamage.GetPoiseBreakAmount(damage.poiseBreakDamage, attribute.PoiseBreakResistance);
		if (poiseBreakAmount >= 80) {
			actionAmount = 3;
			hitDirection = Vector2.up;
			
			Vector3 angle = transform.eulerAngles;
			angle.y += damage.attackAngle;
			transform.eulerAngles = angle;

		} else if (poiseBreakAmount >= 45) {
			actionAmount = 2;

		} else if (poiseBreakAmount >= 20) {
			actionAmount = 1;
			
		} else {
			return; // IF POISE BREAK AMOUNT IS BELOW TO 20, POISE BREAK DOESN'T OCCUR
		}

		animator.SetFloat("HitHorizontal", hitDirection.x);
		animator.SetFloat("HitVertical", hitDirection.y);
		animation.PlayTargetAnimation("Hit_Direction_Tree", 0.2f, true, true);
	}
}

}