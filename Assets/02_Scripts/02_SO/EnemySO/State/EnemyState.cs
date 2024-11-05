using MinD.Runtime.Entity;
using UnityEngine;

namespace MinD.SO.EnemySO.State {

public abstract class EnemyState : ScriptableObject {

	public Enemy enemy;

	public abstract void Enter();

	public abstract void Tick();

	public abstract void Exit();

}

}