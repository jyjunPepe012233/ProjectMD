using System.Collections;
using System.Collections.Generic;
using MinD;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCombatHandler : MonoBehaviour {

	[HideInInspector] public Player owner;

	
	public BaseEntity target;

	[FormerlySerializedAs("currentCastingSpell")] [HideInInspector] public Magic currentCastingMagic;
	private bool usingMagic;
	

	public void HandleAllCombatAction() {
		HandleUsingMagic();
	}
	
	private void HandleUsingMagic() {

		if (currentCastingMagic != null) {
			currentCastingMagic.Tick();
		}
		

		if (PlayerInputManager.Instance.useMagicInput) {

			// CHECK BASIC FLAGS
			if (usingMagic || owner.isPerformingAction) {
				return;
			}


			Magic useMagic = owner.inventory.magicSlots[owner.inventory.currentMagicSlot];

			if (useMagic == null) {
				return;
			}

			// CANCEL IF PLAYER HASN'T ENOUGH MP OR STAMINA
			if (owner.CurMp < useMagic.mpCost) {
				return;
			}
			if (owner.CurStamina < useMagic.staminaCost) {
				return;
			}

			
			usingMagic = true;
			
			owner.CurMp -= useMagic.mpCost;
			owner.CurStamina -= useMagic.staminaCost;

			useMagic.castPlayer = owner;
			
			useMagic.OnUse();
			currentCastingMagic = useMagic;
			
			
			
		} else if (currentCastingMagic != null) { 
			
			// IF INPUT IS NULL AND DURING CASTING => USE MAGIC INPUT IS END
			currentCastingMagic.OnReleaseInput();
			
		}
	}

	public void ExitCurrentMagic() {

		if (currentCastingMagic == null) {
			return;
		}

		usingMagic = false;
		
		currentCastingMagic.OnExit();
		currentCastingMagic = null;
	}

	

	public void InstantiateWarmUpFx() {
		currentCastingMagic.InstantiateWarmupFX();
	}

	public void SuccessfullyCast() {
		currentCastingMagic.OnSuccessfullyCast();
	}
	
	public void CastIsEnd() {
		currentCastingMagic.OnCastIsEnd();
	}
	
}
