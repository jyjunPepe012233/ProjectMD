using MinD.Runtime.Entity;
using UnityEngine;
using UnityEngine.Serialization;

namespace MinD.SO.EnemySO.State {

public abstract class EnemyState : ScriptableObject {
	
	public Enemy self;

	public abstract void Enter();

	public abstract void Tick();

	public abstract void Exit();

}

}