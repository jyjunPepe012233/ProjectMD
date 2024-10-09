using System.Collections.Generic;
using UnityEngine;

namespace MinD {

	public class WorldEntityManager : Singleton<WorldEntityManager> {

		public int entityLayerMask;
		
		private List<BaseEntity> worldEnemies = new List<BaseEntity>();



		private void Awake() {
			entityLayerMask = LayerMask.GetMask("Body Target");
		}
		
		public void RegisteringEnemyOnWorld(Enemy registeringEnemy) {
			
			// ENEMY REGISTERING ON AWAKE
			worldEnemies.Add(registeringEnemy);

		}

	}

}