using MinD.Runtime.Entity;
using UnityEngine;

namespace MinD.SO.EnemySO {

public abstract class EnemyState : ScriptableObject {

	public abstract void Enter(Enemy enemy);

	public abstract void Tick(Enemy enemy);

	public abstract void Exit(Enemy enemy);

}

}