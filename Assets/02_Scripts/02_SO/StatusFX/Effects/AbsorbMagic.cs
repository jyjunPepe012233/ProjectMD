using MinD.Enums;
using MinD.Runtime.Entity;
using MinD.Structs;
using UnityEngine;

namespace MinD.SO.StatusFX.Effects {

[CreateAssetMenu(fileName = "AbsorbMagic", menuName = "MinD/Status Effect/Effects/TakeAbsorbMagic")]
public class AbsorbMagic : InstantEffect {

	public int absorbMp;
	public Vector3 localDefensePoint;
	

	
	public AbsorbMagic(int absorbMp, Vector3 localDefensePoint) {
		this.absorbMp = absorbMp;
		this.localDefensePoint = localDefensePoint;
	}

	
	protected override void OnInstantiateAs(Player player) {

		player.CurMp += absorbMp;
		
		Debug.Log("Successfully Parryed");

	}

	protected override void OnInstantiateAs(Enemy enemy) {
	}	

}

}