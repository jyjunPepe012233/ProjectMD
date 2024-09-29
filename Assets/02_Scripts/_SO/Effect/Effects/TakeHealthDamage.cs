using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MinD.Combat;
using UnityEngine.SearchService;

namespace MinD.StatusFx {
	public class TakeHealthDamage : InstantEffect {

		public Damage damage;

		public PoiseBreakAmount poiseBreakAmount;
		public HitDirection hitDirection;
		
		
	
		protected override void OnInstantiateAs(Player player) {
			
			player.curHp -= player.attribute.damageNegation.GetCalculatedDamage(this);

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