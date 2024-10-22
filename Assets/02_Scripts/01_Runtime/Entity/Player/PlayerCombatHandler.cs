using System.Collections;
using System.Collections.Generic;
using MinD;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCombatHandler : MonoBehaviour {

	[HideInInspector] public Player owner;
	

	public void HandleAllCombatAction() {
		
		HandleUseMagic();

	}
	
	
	private void HandleUseMagic() {

		if (PlayerInputManager.Instance.useMagicInput) {
			PlayerInputManager.Instance.useMagicInput = false;


			Magic useMagic = owner.inventory.selectedMagic;

			// CANCEL IF PLAYER HASN'T ENOUGH MP
			if (owner.curMp < useMagic.mpCost)
				return;

			owner.curMp -= useMagic.mpCost;
			useMagic.OnUse();
		}
	}
	
}
