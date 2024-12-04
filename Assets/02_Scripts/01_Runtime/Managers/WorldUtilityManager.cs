using UnityEngine;

namespace MinD.Runtime.Managers {

public class WorldUtilityManager : Singleton<WorldUtilityManager> {

	public static LayerMask environmentLayerMask {
		get {
			if (_environmentLayerMask == default) {
				_environmentLayerMask = LayerMask.GetMask("Default");
			}
			return _environmentLayerMask;
		}
	}
	private static LayerMask _environmentLayerMask;
	
	
	public static LayerMask damageableLayerMask {
		get {
			if (_damageableLayerMask == default) {
				_damageableLayerMask = LayerMask.GetMask("Damageable Entity");
			}
			return _damageableLayerMask;
		}
	}
	private static LayerMask _damageableLayerMask;


	
}

}