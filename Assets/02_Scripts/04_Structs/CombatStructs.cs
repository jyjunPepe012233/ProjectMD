using System;
using System.Collections.Generic;
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
	public float physical {
		get => finalPhysical;
		set => _physical = value;
	}
	public float magic  {
		get => finalMagic;
		set => _magic = value;
	}
	public float fire {
		get => finalFire;
		set => _fire = value;
	}
	public float frost {
		get => finalFrost;
		set => _frost = value;
	}
	public float lightning {
		get => finalLightning;
		set => _lightning = value;
	}
	public float holy {
		get => finalHoly;
		set => _holy = value;
	}

	// BASE NEGATION VALUE OF THIS STRUCT
	[SerializeField, Range(-1, 1)] private float _physical, _magic, _fire, _frost, _lightning, _holy;

	
	private List<DamageNegation> multiplyingNegations;
	
	// FINALLY CALCULATED NEGATION VALUE BY multiplyingNegations LIST
	private float finalPhysical, finalMagic, finalFire, finalFrost, finalLightning, finalHoly;
	
	
	
	private void RefreshNegationCalculate() {
		
		finalPhysical = _physical;
		finalMagic = _magic;
		finalFire = _fire;
		finalFrost = _frost;
		finalLightning = _lightning;
		finalHoly = _holy;

		for (int i = 0; i < multiplyingNegations.Count; i++) {

			finalPhysical += (1 - finalPhysical) * multiplyingNegations[i].physical;
			finalMagic += (1 - finalMagic) * multiplyingNegations[i].magic;
			finalFire += (1 - finalFire) * multiplyingNegations[i].fire;
			finalFrost += (1 - finalFrost) * multiplyingNegations[i].frost;
			finalLightning += (1 - finalLightning) * multiplyingNegations[i].lightning;
			finalHoly += (1 - finalHoly) * multiplyingNegations[i].holy;

		}

	}

	
	
	public static DamageNegation operator *(DamageNegation a, DamageNegation b) {

		if (a.multiplyingNegations == null) {
			a.multiplyingNegations = new List<DamageNegation>();
		}
		
		
		a.multiplyingNegations.Add(b);
		a.RefreshNegationCalculate();
		
		return a;
	}
	
	public static DamageNegation operator /(DamageNegation a, DamageNegation b) {
		
		if (a.multiplyingNegations == null) {
			a.multiplyingNegations = new List<DamageNegation>();
		}
		
		// FIND AND REMOVE ONCE OF EQUAL STRUCT
		for (int i = 0; i < a.multiplyingNegations.Count; i++) {
			
			if (a.multiplyingNegations[i].Equals(b)) {
				a.multiplyingNegations.RemoveAt(i);
				a.RefreshNegationCalculate();
				return a;
			}
			
		}
		
		Debug.Log("!! DAMAGE NEGATION OPERATOR CAN'T OPERATE!");
		return a;
	}
	
}

}