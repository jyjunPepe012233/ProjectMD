using MinD.Enums;
using MinD.Runtime.Entity;
using MinD.Structs;
using UnityEngine;

namespace MinD.SO.StatusFX.Effects {

[CreateAssetMenu(fileName = "TakeHealthDamage", menuName = "MinD/Status Effect/Effects/TakeHealthDamage")]
public class TakeHealthDamage : InstantEffect {

	public Damage damage;
	public int poiseBreakDamage;

	public float hitAngle;

	
	
	public static int GetCalculatedDamage(Damage damage_, DamageNegation negation_) {

		int finalDamage = 0;
		finalDamage += (int)((1 - negation_.physical) * damage_.physical);
		finalDamage += (int)((1 - negation_.magic) * damage_.magic);
		finalDamage += (int)((1 - negation_.fire) * damage_.fire);
		finalDamage += (int)((1 - negation_.frost) * damage_.frost);
		finalDamage += (int)((1 - negation_.lightning) * damage_.lightning);
		finalDamage += (int)((1 - negation_.holy) * damage_.holy);

		return finalDamage;
	}

	public static int GetPoiseBreakAmount(int poiseBreakDamage, int poiseBreakResistance) {

		float resistanceValue = (float)poiseBreakResistance / 100;


		// get minPoiseBreak(poise break amount when resistance is minimum)
		// get maxPoiseBreak(poise break amount when resistance is maximum)
		// and lerp two value as t is poise break resistance(0~1)

		float minPoiseBreak = (poiseBreakDamage); // 0 to 100
		float maxPoiseBreak = (1.3f * poiseBreakDamage) - 50; // -50 to 80 

		return Mathf.Clamp((int)(Mathf.Lerp(minPoiseBreak, maxPoiseBreak, resistanceValue)), 0, 100);
	}

	

	protected override void OnInstantiateAs(Player player) {

		// DRAIN HP
		player.CurHp -= GetCalculatedDamage(damage, player.attribute.damageNegation);
		
		// CANCEL ACTIONS
		player.combat.CancelMagicOnGetHit();
		
		// INVOKE ACTION
		player.combat.getHitAction();
		
		
		// IF PLAYER HAS IMMUNE OF POISE BREAK, DON'T GIVE POISE BREAK 
		if (player.immunePoiseBreak) {
			return;
		}
		// IF PLAYER HAS DIED, POISE BREAK IS NOT RUNNING
		if (player.isDeath) {
			return;
		}
		
		
		#region SET_POISE_BREAK_ANIMATION
		
		
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
		int poiseBreakAmount = GetPoiseBreakAmount(poiseBreakDamage, player.attribute.poiseBreakResistance);
		if (poiseBreakAmount >= 80) {
			stateName += "KnockDown_Start";
			
			Vector3 angle = player.transform.eulerAngles;
			angle.y += hitAngle;
			player.transform.eulerAngles = angle;

		} else if (poiseBreakAmount >= 55) {
			stateName += "Large_";
			stateName += hitDirection;

		} else if (poiseBreakAmount >= 20) {
			stateName += "Default_";
			stateName += hitDirection;
			
		} else {
			return; // IF POISE BREAK AMOUNT IS BELOW TO 20, POISE BREAK DOESN'T OCCUR
		}
		
		#endregion

		// PLAY POISE BREAK ANIMATION
		player.animation.PlayTargetAction(stateName, true, true, false, false);
	}

	protected override void OnInstantiateAs(Enemy enemy) {
		
		// DRAIN HP
		enemy.CurHp -= GetCalculatedDamage(damage, enemy.attribute.damageNegation);
		
		enemy.getHitAction.Invoke();
	}	

}

}