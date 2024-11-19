using System;
using System.Collections;
using MinD.Runtime.DataBase;
using MinD.Runtime.Managers;
using MinD.SO.Item;
using UnityEngine;


namespace MinD.Runtime.Entity {

public class PlayerCombatHandler : MonoBehaviour {

	[HideInInspector] public Player owner;
	
	// LOCKING ON ENTITY
	public BaseEntity target;
	
	[Header("[ Flags ]")]
	public bool usingMagic;
	public bool usingDefenseMagic;
	public bool isParrying;
	
	
	[HideInInspector] public PlayerDefenseMagic defenseMagic;
	[HideInInspector] public Magic currentCastingMagic;
	
	// WHEN PLAYER GET HIT, CALL THIS ACTION IN 'TakeHealthDamage'
	public Action getHitAction = new Action(()=>{});
	
	

	public void HandleAllCombatAction() {

		if (owner.isDeath) {
			return;
		}

		HandleUsingMagic();
		HandleDefenseMagic();
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

	private void HandleDefenseMagic() {
		
		if (!usingDefenseMagic && PlayerInputManager.Instance.defenseMagicInput) {
			
			if (owner.isPerformingAction) {
				return;
			}
			if (!owner.isGrounded) {
				return;
			}
			
			ActivateDefenseMagic();
		}
		
		
		
		if (usingDefenseMagic && !PlayerInputManager.Instance.defenseMagicInput) { 
			ReleaseDefenseMagic();
		}
		
	}

	private void ActivateDefenseMagic() {
		
		usingDefenseMagic = true; // IF THIS FLAG IS ENABLED, DAMAGE WILL CALCULATE SPECIAL

		
		
		// PLAY DEFENSE ANIMATION (LOOPING)
		owner.animation.PlayTargetAction("Defense_Action_Start", 0.2f, true, true, true, false);
		
		// ACTIVE DEFENSE MAGIC
		defenseMagic.EnableShield();
		

		
		
		
	}
	public void ReleaseDefenseMagic(bool playAnimation = true, bool parrying = true) {

		usingDefenseMagic = false;

		
		if (playAnimation) {
			if (parrying) {
				owner.animation.PlayTargetAction("Defense_Action_Parry", 0.2f, true, true, true, false);
			} else {
				owner.animation.PlayTargetAction("Default Movement", 0.3f, false);
			}
		}

		// DISABLE DEFENSE MAGIC
		defenseMagic.DisableShield();
	}

	public void StartParrying() => isParrying = true;
	public void EndParrying() => isParrying = false;
	
	
	
	
	
	
	public void ExitCurrentMagic() {

		if (currentCastingMagic == null) {
			return;
		}

		usingMagic = false;

		currentCastingMagic.OnExit();
		currentCastingMagic = null;
	}
	
	public void CancelMagicOnGetHit() {

		// CANCEL MAGIC
		if (currentCastingMagic != null) {
			currentCastingMagic.OnCancel();
		}
		
	}



	public void OnInstantiateWarmUpFx() {
		currentCastingMagic.InstantiateWarmupFX();
	}
	public void OnSuccessfullyCast() {
		currentCastingMagic.OnSuccessfullyCast();
	}

}

}