using System;
using System.Collections;
using System.Linq;
using MinD.Runtime.DataBase;
using MinD.Runtime.Managers;
using MinD.SO.Item;
using UnityEditor;
using UnityEngine;
using Tool = MinD.SO.Item.Tool;


namespace MinD.Runtime.Entity {

public class PlayerCombatHandler : BaseEntityHandler<Player> {
	
	// LOCKING ON ENTITY
	public BaseEntity target;
	
	[Header("[ Flags ]")]
	public bool usingMagic;
	public bool usingDefenseMagic;
	public bool isParrying;
	
	
	[HideInInspector] public PlayerDefenseMagic defenseMagic;
	[HideInInspector] public Magic currentCastingMagic;
	
	

	public void HandleAllCombatAction() {

		if (owner.isDeath) {
			return;
		}

		HandleUsingMagic();
		HandleUsingTool();
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



		} else if (currentCastingMagic != null && !currentCastingMagic.isInputReleased) {

			// IF INPUT IS NULL AND DURING CASTING => USE MAGIC INPUT IS END
			currentCastingMagic.OnReleaseInput();
			currentCastingMagic.isInputReleased = true;

		}
	}

	private void HandleUsingTool() {

		// HANDLE TOOL USING COOL TIME
		Tool[] tools = owner.inventory.toolSlots.Where(i => i != null && i.remainingCoolTime > 0).ToArray();
		for (int i = 0; i < tools.Length; i++) {
			tools[i].remainingCoolTime = Mathf.Max((tools[i].remainingCoolTime - Time.deltaTime), 0); // SET MINIMUM TO 0
		}
		
		
		// USING TOOL
		if (PlayerInputManager.Instance.useToolInput) {

			if (owner.isPerformingAction) {
				return;
			}
			if (!owner.isGrounded) {
				return;
			}

			Tool useTool = owner.inventory.toolSlots[owner.inventory.currentToolSlot];

			if (useTool.remainingCoolTime > 0) {
				return;
			}
			

			useTool.OnUse(owner);
			useTool.remainingCoolTime = useTool.usingCoolTime;
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