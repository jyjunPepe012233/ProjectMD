using System.Buffers;
using MinD.Runtime.DataBase;
using MinD.Runtime.Entity;
using MinD.Structs;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.Serialization;

namespace MinD.SO.StatusFX.Effects {

[CreateAssetMenu(fileName = "TakeDefensedHealthDamage", menuName = "MinD/Status Effect/Effects/TakeDefensedHealthDamage")]
public class TakeDefensedHealthDamage : TakeHealthDamage {
	
	private Vector3 attackDirx3D;
	
	public TakeDefensedHealthDamage(BaseEntity perpetrator, Damage damage, int poiseBreakDamage, float attackAngle, Vector3 attackDirx3D) : base(perpetrator, damage, poiseBreakDamage, attackAngle) {
		base.perpetrator = perpetrator;
		base.damage = damage;
		base.poiseBreakDamage = poiseBreakDamage;
		base.attackAngle = attackAngle;
		this.attackDirx3D = attackDirx3D;
	}
	
	
	
//	protected override void OnInstantiateAs(Player player) {
//		
//		// INVOKE ACTION
//		player.getHitAction.Invoke(this as TakeHealthDamage);
//		
//		
//
//		int realDamage = damage.AllDamage;
//		int negatedDamage = TakeHealthDamage.GetCalculatedDamage(damage, player.attribute.damageNegation);
//		
//		
//		
//		// STAMINA DRAIN BY NEGATED AMOUNT
//		int staminaDrain = Mathf.Max((realDamage - negatedDamage) / 3, 1); // MINIMUM OF STAMINA DRAIN AMOUNT IS 1
//
//		float staminaDrainAmount = Mathf.Clamp01((float)player.CurStamina / staminaDrain);
//		// ㄴ AMOUNT OF SUCCESSFULLY DRAINED STAMINA
//		// ㄴ 0 IS PLAYER HASN'T STAMINA ENOUGH TO DEFENSE DAMAGE
//		// ㄴ 1 IS PLAYER HAS STAMINA ENOUGH
//
//		player.CurStamina -= staminaDrain;
//		
//		if (staminaDrainAmount <= 0.45f) { // if 45% of stamina wasn't drain
//			// GUARD BREAK AND KNOCK DOWN
//			
//			// DRAIN HP BY ALL DAMAGE
//			player.CurHp -= (int)(realDamage * 1.4f);
//			
//			player.combat.defenseMagic.PlayGuardBreakVFX();
//			player.combat.ReleaseDefenseMagic(false, false);
//
//			if (!player.isDeath) {
//				player.animation.PlayTargetAction("Defense_Break", 0.15f, true, true, false, false);
//			}
//			
//		} else {
//			// SUCCESSFULLY DEFENSE ATTACK
//			
//			// DRAIN HP
//			player.CurHp -= negatedDamage;
//			player.CurHp -= (int)(realDamage * (1-staminaDrainAmount)); // DRAINING HP BY AMOUNT OF CAN'T DRAINED STAMINA
//			
//			
//			// INSTANTIATE VFX
//			player.combat.defenseMagic.PlayHitVFX(attackDirx3D);
//			
//
//		}
//	}

}

}