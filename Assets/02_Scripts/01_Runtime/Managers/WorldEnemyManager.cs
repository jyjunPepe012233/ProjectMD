using System.Collections.Generic;
using MinD.Runtime.Entity;
using UnityEngine;

namespace MinD.Runtime.Managers {

public class WorldEnemyManager : Singleton<WorldEnemyManager> {
	
	[SerializeField] private List<Enemy> worldEnemies = new List<Enemy>();



	public void RegisteringEnemyOnWorld(Enemy registeringEnemy) {

		// A ENEMY WILL REGISTER ON SCENE AWAKE
		worldEnemies.Add(registeringEnemy);
	}


	public void ResetAllEnemyOnWorld() {
		for (int i = 0; i < worldEnemies.Count; i++) {

			worldEnemies[i].Reload();

		}
	}

}

}