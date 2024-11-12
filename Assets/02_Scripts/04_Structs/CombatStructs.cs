using System;
using System.Collections.Generic;
using MinD.SO.StatusFX.Effects;
using UnityEngine;
using UnityEngine.Serialization;

namespace MinD.Structs {

[Serializable]
public struct Damage {

	public int physical;
	public int magic;
	public int fire;
	public int frost;
	public int lightning;
	public int holy;

	public int AllDamage {
		get => physical + magic + fire + frost + lightning + holy;
		set {
			physical = value;
			magic = value;
			fire = value;
			frost = value;
			lightning = value;
			holy = value;
		}
	}
}

[Serializable]
public struct DamageNegation {

	// 0~1
	[Range(-1, 1)] public float physical;
	[Range(-1, 1)] public float magic; 
	[Range(-1, 1)] public float fire; 
	[Range(-1, 1)] public float frost; 
	[Range(-1, 1)] public float lightning; 
	[Range(-1, 1)] public float holy;
	
	private List<DamageNegation> addedNegation;

	private void ApplyNegationCalculate() {

		for (int i = 0; i < addedNegation.Count; i++) {
			
//			physical addedNegation[i]
			
		}

	}

	
	
	public static DamageNegation operator +(DamageNegation a, DamageNegation b) {

		return a;
	}
	
	public static DamageNegation operator -(DamageNegation a, DamageNegation b) {
		
		a.physical -= b.physical;
		a.magic -= b.magic;
		a.fire -= b.fire;
		a.frost -= b.frost;
		a.lightning -= b.lightning;
		a.holy -= b.holy;

		return a;
	}
	
}

}