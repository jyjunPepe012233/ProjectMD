using System;
using MinD.StatusFx;
using MinD.Combat;
using UnityEngine;

namespace MinD.Combat {

	[Serializable]
	public struct DamageNegation {
	
		// 0~1
		[Range(0, 1)] public float physical;
		[Range(0, 1)] public float magic;
		[Range(0, 1)] public float fire;
		[Range(0, 1)] public float frost;
		[Range(0, 1)] public float lightning;
		[Range(0, 1)] public float holy;
	
		public int GetFinalDamage(TakeHealthDamage damage) {
	
			int finalDamage = 0;
			finalDamage += (int)((1 - physical) * damage.physicalDamage);
			finalDamage += (int)((1 - magic) * damage.magicDamage);
			finalDamage += (int)((1 - fire) * damage.fireDamage);
			finalDamage += (int)((1 - frost) * damage.frostDamage);
			finalDamage += (int)((1 - lightning) * damage.lightningDamage);
			finalDamage += (int)((1 - holy) * damage.holyDamage);
	
			return finalDamage;
		}
	}
}

