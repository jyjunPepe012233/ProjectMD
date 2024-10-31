using MinD.Structs;
using UnityEngine;

namespace MinD.Runtime.Entity {

public class EnemyAttributeHandler : MonoBehaviour {

	public int maxHp;
	
	[Space(5)]
	public float speed;

	[Space(15)]
	public DamageNegation damageNegation;
	public int poiseBreakResistance;

}

}