using System.Collections.Generic;
using MinD.Runtime.Entity;
using UnityEngine;

namespace MinD.Runtime.Managers {

public class WorldEntityManager : Singleton<WorldEntityManager> {

	public Player player {
		get {
			if (player_ == null)
				player_ = FindObjectOfType<Player>();
			return player_;
		}
	}
	private Player player_;
	
	[SerializeField] private List<Enemy> worldEnemies;



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