using MinD.Structs;
using UnityEngine;

namespace MinD.SO.EnemySO {

[CreateAssetMenu(menuName = "MinD/Enemy SO/Enemy Attribute", fileName = "Enemy Attribute")]
public class EnemyAttribute : ScriptableObject {

	public int maxHp;
	public float speed;

	[Space(10)]
	public DamageNegation damageNegation;
	[Range(0, 100)] public int poiseBreakResistance;
}

}