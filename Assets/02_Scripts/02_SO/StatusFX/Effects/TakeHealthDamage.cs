using MinD.Enums;
using MinD.Runtime.Entity;
using MinD.Structs;
using UnityEngine;

namespace MinD.SO.StatusFX.Effects {

[CreateAssetMenu(fileName = "TakeHealthDamage", menuName = "MinD/Status Effect/Effects/TakeHealthDamage")]
public class TakeHealthDamage : InstantEffect {

	public Damage damage;
	public int poiseBreakDamage;
	
	public HitDirection hitDirection;

	
	
	private static int GetCalculatedDamage(Damage damage_, DamageNegation negation_) {

		int finalDamage = 0;
		finalDamage += (int)((1 - negation_.physical) * damage_.physical);
		finalDamage += (int)((1 - negation_.magic) * damage_.magic);
		finalDamage += (int)((1 - negation_.fire) * damage_.fire);
		finalDamage += (int)((1 - negation_.frost) * damage_.frost);
		finalDamage += (int)((1 - negation_.lightning) * damage_.lightning);
		finalDamage += (int)((1 - negation_.holy) * damage_.holy);

		return finalDamage;
	}
	

	protected override void OnInstantiateAs(Player player) {

		player.CurHp -= GetCalculatedDamage(damage, player.attribute.damageNegation);

		string stateName = "";
		switch (hitDirection) {

			case HitDirection.Front:
				stateName = "GetHit_Default_F";
				break;

			case HitDirection.Right:
				stateName = "GetHit_Default_R";
				break;

			case HitDirection.Back:
				stateName = "GetHit_Default_B";
				break;

			case HitDirection.Left:
				stateName = "GetHit_Default_L";
				break;
		}

		player.animation.PlayTargetAction(stateName, true, true, false, false);

	}

	protected override void OnInstantiateAs(Enemy enemy) {



	}

}

}