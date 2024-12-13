using MinD.Runtime.Entity;
using UnityEngine;

namespace MinD.SO.EnemySO {

public abstract class IdleState : EnemyState {
	
	[SerializeField] protected float detectAngle;
	[SerializeField] protected float detectRadius;
	[SerializeField] protected float absoluteDetectRadius;
	
}

}