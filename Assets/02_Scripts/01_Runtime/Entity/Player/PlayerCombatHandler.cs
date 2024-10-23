using System.Collections;
using System.Collections.Generic;
using MinD;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCombatHandler : MonoBehaviour {

	[HideInInspector] public Player owner;

	public BaseEntity target;

	[HideInInspector] public Magic currentCastingSpell;
	
	

	public void HandleAllCombatAction() {
		HandleUseMagic();
	}
	
	private void HandleUseMagic() {

		if (PlayerInputManager.Instance.useMagicInput) {
			PlayerInputManager.Instance.useMagicInput = false;

			
			Magic useMagic = owner.inventory.selectedMagic;

			// CANCEL IF PLAYER HASN'T ENOUGH MP OR STAMINA
			if (owner.CurMp < useMagic.mpCost)
				return;
			
			if (owner.CurStamina < useMagic.staminaCost)
				return;

			
			// USE MAGIC
			owner.CurMp -= useMagic.mpCost;
			owner.CurStamina -= useMagic.staminaCost;
			
			
			useMagic.OnUse(owner);
			currentCastingSpell = useMagic;
			
		}
	}



	public void InstantiateMagicObject() {
		currentCastingSpell.InstantiateMagicObject(owner);
	}

	public void InstantiateWarmUpFX() {
		currentCastingSpell.InstantiateWarmupFX(owner);
	}
	
}
