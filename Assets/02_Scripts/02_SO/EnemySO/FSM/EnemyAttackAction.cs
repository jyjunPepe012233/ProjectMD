using UnityEngine;

namespace MinD.SO.EnemySO {

[CreateAssetMenu(menuName = "MinD/Enemy SO/FSM/Attack Action", fileName = "EnemyType ActionName", order = int.MinValue)]
public class EnemyAttackAction : ScriptableObject {
	
	public AnimationClip actionClip;

	[Header("[ Combat Action Setting ]")]
	[Range(0, 1)] public float actionWeight;
	[Space(5)]
	public float actionRecoveryTime = 0.5f;
	[Range(-180, 180)] public float minimumAngle = -35f;
	[Range(-180, 180)] public float maximumAngle = 35f;
	public float minDistance = 0f;
	public float maxDistance = 2f;

		
	[Header("[ Combo Setting ]")]
	public bool canPerformCombo = false;
	[Range(0, 1)] public float chanceToCombo = 0.5f;
	public EnemyAttackAction comboAction;
}

}