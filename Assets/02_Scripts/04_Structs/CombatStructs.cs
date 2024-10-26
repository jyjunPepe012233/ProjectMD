using System;
using MinD.SO.StatusFX.Effects;
using UnityEngine;

namespace MinD.Structs {

[Serializable]
public struct Damage {

	public int physical;
	public int magic;
	public int fire;
	public int frost;
	public int lightning;
	public int holy;

}

[Serializable]
public struct DamageNegation {

	// 0~1
	[Range(0, 1)] public float physical;
	[Range(0, 1)] public float magic;
	[Range(0, 1)] public float fire;
	[Range(0, 1)] public float frost;
	[Range(0, 1)] public float lightning;
	[Range(0, 1)] public float holy;

	public int GetCalculatedDamage(TakeHealthDamage damageEffect) {

		int finalDamage = 0;
		finalDamage += (int)((1 - physical) * damageEffect.damage.physical);
		finalDamage += (int)((1 - magic) * damageEffect.damage.magic);
		finalDamage += (int)((1 - fire) * damageEffect.damage.fire);
		finalDamage += (int)((1 - frost) * damageEffect.damage.frost);
		finalDamage += (int)((1 - lightning) * damageEffect.damage.lightning);
		finalDamage += (int)((1 - holy) * damageEffect.damage.holy);

		return finalDamage;
	}
}

}