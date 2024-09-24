using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MinD.Combat;
using UnityEngine.SearchService;

namespace MinD.StatusFx {
	public class TakeHealthDamage : InstantEffect {

		public int physicalDamage = 0;
		public int magicDamage = 0;
		public int fireDamage = 0;
		public int frostDamage = 0;
		public int lightningDamage = 0;
		public int holyDamage = 0;

		public PoiseBreakAmount poiseBreakAmount;
		public HitDirection hitDirection;
		
		
	
		protected override void OnInstantiateAs(Player player) {
			
			player.curHp -= player.attribute.damageNegation.GetFinalDamage(this);

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