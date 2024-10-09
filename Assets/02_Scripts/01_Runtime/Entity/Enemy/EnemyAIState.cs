using UnityEngine;

namespace MinD {
	public abstract class EnemyState : MonoBehaviour {

		public abstract void Enter(Enemy enemy);

		public abstract void Tick(Enemy enemy);

		public abstract void Exit(Enemy enemy);

	}
}