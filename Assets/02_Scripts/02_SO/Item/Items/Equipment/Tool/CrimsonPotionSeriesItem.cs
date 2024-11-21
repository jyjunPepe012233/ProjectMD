using MinD.Runtime.Entity;
using UnityEngine;

namespace MinD.SO.Item.Items {

[CreateAssetMenu(fileName = "Crimson Potion", menuName = "MinD/Item/Items/Equipment/Tool/Crimson Potion Series Item")]
public class CrimsonPotionSeriesItem : Tool {
	
	[Header("[ Custom Data ]")]
	[SerializeField] private int hpFillAmount;
	[SerializeField] private string healVfxName;



	public override void OnEquip(Player owner) {
	}

	public override void Execute(Player owner) {
	}

	public override void OnUnequip(Player owner) {
	}

	
	
	public override void OnUse(Player owner) {

		owner.CurHp += hpFillAmount;
		Debug.Log(29 + " : " + hpFillAmount);
		
		// VFX
		
	}
}

}