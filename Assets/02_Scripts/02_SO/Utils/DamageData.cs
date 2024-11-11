using MinD.Structs;
using UnityEngine;

namespace MinD.SO.Utils {

[CreateAssetMenu(fileName = "Damage Data", menuName = "MinD/Utils/Damage Data")]
public class DamageData : ScriptableObject {
	
	public Damage damage; 
	public int totalDamage; // ONLY FOR SHOWING
	
	[Space(5)]
	[Range(0, 100)] public int poiseBreakDamage;

	[Space(10)]
	public int absorbMp;
	
	

	void OnValidate() {
		RefreshValue();
	}

	public void RefreshValue() {
		
		totalDamage = damage.physical + damage.magic + damage.fire + damage.frost + damage.lightning + damage.holy;
		
	}
	
}

}