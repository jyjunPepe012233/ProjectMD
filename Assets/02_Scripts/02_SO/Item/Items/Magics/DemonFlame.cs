using System.Runtime.Serialization;
using UnityEngine;

namespace MinD.Magics {

	[CreateAssetMenu(menuName="MinD/Item/Items/Magics/Demon Flame")]
	public class DemonFlame : Magic {
		
		public override void OnUse(Player player) {
			
			player.animation.PlayTargetAction("DemonFlame", isPerformingAction, applyRootMotion, canRotate, canMove);
			
		}

		public override void InstantiateMagicObject(Player player) {

			Vector3 summonPos = player.transform.position + player.transform.up * 2.3f;

			DemonFlameSpirit demonFlame = ObjectDataBase.Instance.InstantiateMagic("DemonFlame_Spirit").GetComponent<DemonFlameSpirit>();
			demonFlame.transform.position = summonPos;

			if (player.combat.target != null) {
				demonFlame.Shoot(player, player.combat.target);
			} else {
				demonFlame.Shoot(player, null);
			}
			
		}
	}

}