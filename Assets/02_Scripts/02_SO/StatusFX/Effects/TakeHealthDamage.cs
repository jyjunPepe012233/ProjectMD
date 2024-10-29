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



	protected override void OnInstantiateAs(Player player) {

		player.CurHp -= player.attribute.damageNegation.GetCalculatedDamage(this);

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