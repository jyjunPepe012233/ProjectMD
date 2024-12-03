using MinD.Runtime.Entity;
using UnityEngine;

namespace MinD.SO.EnemySO {

[CreateAssetMenu(menuName = "MinD/Enemy SO/State/Combat Stance", fileName = "Type_Combat_Stance")]
public class CombatStanceState : EnemyState {

	public override EnemyState Tick(Enemy self) {


		return self.combatStanceState;

	}
}

}