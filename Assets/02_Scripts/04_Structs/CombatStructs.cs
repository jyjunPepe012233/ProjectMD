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
	public float physical;
	public float magic; 
	public float fire; 
	public float frost; 
	public float lightning; 
	public float holy;
	
}

}