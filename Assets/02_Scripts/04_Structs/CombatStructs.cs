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
	public float physical;
	public float magic; 
	public float fire; 
	public float frost; 
	public float lightning; 
	public float holy;

	public static DamageNegation operator +(DamageNegation a, DamageNegation b) {
		
		a.physical += b.physical;
		a.magic += b.magic;
		a.fire += b.fire;
		a.frost += b.frost;
		a.lightning += b.lightning;
		a.holy += b.holy;

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