using UnityEngine;

namespace MinD.Utility {

public static class WorldUtility {

	public const string LAYERNAME_environment = "Default";
	public const string LAYERNAME_damageable = "Damageable Entity";
	
	public static LayerMask environmentLayerMask => LayerMask.GetMask(LAYERNAME_environment);
	public static LayerMask damageableLayerMask => LayerMask.GetMask(LAYERNAME_damageable);



}

}