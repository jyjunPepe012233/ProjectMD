using MinD.Structs;
using UnityEngine;

namespace MinD.Runtime.Entity {

public class EnemyAttributeHandler : MonoBehaviour {

	public int maxHp;
	public float speed;

	[Space(10)]
	public DamageNegation damageNegation;
	public int poiseBreakResistance;

}

}