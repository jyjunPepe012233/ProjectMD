using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MinD.Combat;
using MinD.UI;
using UnityEngine.SearchService;

namespace MinD.StatusFx {

	[CreateAssetMenu(fileName = "TakeHealthDamage", menuName = "MinD/Status Effect/Effects/TakeHealthDamage")]
	public class TakeHealthDamage : InstantEffect {

		public Damage damage;

		public PoiseBreakAmount poiseBreakAmount;
		public HitDirection hitDirection;
		
		
	
		protected override void OnInstantiateAs(Player player) {
			
			player.curHP -= player.attribute.damageNegation.GetCalculatedDamage(this);

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
			PlayerHUDManager.Instance.RefreshHPBar();

		}
	
		protected override void OnInstantiateAs(Enemy enemy) {
			
            
            
		}
		
	}
}