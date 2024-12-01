using UnityEngine;

namespace MinD.Runtime.Managers {

public class WorldUtilityManager : Singleton<WorldUtilityManager> {

	public static LayerMask environmentLayerMask = LayerMask.GetMask("Default");
	public static LayerMask damageableLayerMask = LayerMask.GetMask("Damageable Entity");

	
}

}