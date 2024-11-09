using System.Collections.Generic;
using MinD.Runtime.Entity;

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
	
	private List<BaseEntity> worldEnemies = new List<BaseEntity>();



	public void RegisteringEnemyOnWorld(Enemy registeringEnemy) {

		// ENEMY REGISTERING ON AWAKE
		worldEnemies.Add(registeringEnemy);

	}

}

}