using MinD.Runtime.Entity;
using MinD.Structs;
using UnityEngine;

namespace MinD.SO.StatusFX.Effects {

[CreateAssetMenu(fileName = "TakeDefensedHealthDamage", menuName = "MinD/Status Effect/Effects/TakeDefensedHealthDamage")]
public class TakeDefensedHealthDamage : InstantEffect {

	public Damage damage;
	public int poiseBreakDamage;

	public float hitAngle;


	public TakeDefensedHealthDamage(Damage damage, int poiseBreakDamage, float hitAngle) {
		this.damage = damage;
		this.poiseBreakDamage = poiseBreakDamage;
		this.hitAngle = hitAngle;
	}

	
	protected override void OnInstantiateAs(Player player) {

		int realDamage = damage.AllDamage;
		int negatedDamage = TakeHealthDamage.GetCalculatedDamage(damage, player.attribute.damageNegation);
		
		
		player.CurHp -= negatedDamage;
		
		
		
		// STAMINA DRAIN BY NEGATED AMOUNT
		int staminaDrain = Mathf.Max((realDamage - negatedDamage) / 3, 1); // MINIMUM OF STAMINA DRAIN IS 1

		float staminaDrainAmount = Mathf.Clamp01((float)player.CurStamina / staminaDrain);
		// ㄴ AMOUNT OF SUCCESSFULLY DRAINED STAMINA
		// ㄴ 0 IS PLAYER HASN'T STAMINA ENOUGH TO DEFENSE DAMAGE
		// ㄴ 1 IS PLAYER HAS STAMINA ENOUGH

		player.CurStamina -= staminaDrain;
		
		if (staminaDrainAmount <= 0.2f) { // if 20% of stamina wasn't drain
			// GUARD BREAK AND KNOCK DOWN
			
		} else if (staminaDrainAmount <= 0.5) { // if 50% of stamina wasn't drain 
			// GUARD BREAK
			
		} else {
			// GUARD SUCCESSFULLY
			
		}
		
	}

	protected override void OnInstantiateAs(Enemy enemy) {
	}	

}

}