using System.Collections.Generic;
using UnityEngine;

namespace MinD {

	public class WorldEntityManager : Singleton<WorldEntityManager> {
		
		private List<BaseEntity> worldEnemies = new List<BaseEntity>();


		
		public void RegisteringEnemyOnWorld(Enemy registeringEnemy) {
			
			// ENEMY REGISTERING ON AWAKE
			worldEnemies.Add(registeringEnemy);

		}

	}

}