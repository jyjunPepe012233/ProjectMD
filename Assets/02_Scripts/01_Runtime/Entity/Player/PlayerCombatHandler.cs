using System;
using System.Collections;
using MinD.Runtime.Managers;
using MinD.SO.Item;
using UnityEngine;


namespace MinD.Runtime.Entity {

public class PlayerCombatHandler : MonoBehaviour {

	[HideInInspector] public Player owner;
	
	// LOCKING ON ENTITY
	public BaseEntity target;

	[Header("[ Defense Magic Collider ]")]
	public GameObject defenseMagicCollider;
	
	
	[HideInInspector] public Magic currentCastingMagic;
	[SerializeField] private bool usingMagic;

	private Coroutine defenseMagicCoroutine; 
	public bool usingDefenseMagic;
	public bool isParrying;
	
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
			
			StartCoroutine(ActivateDefenseMagic());
		}
		
		
		
		if (usingDefenseMagic && !PlayerInputManager.Instance.defenseMagicInput) {
			StartCoroutine(ReleaseDefenseMagic());
		}
		
	}

	public IEnumerator ActivateDefenseMagic(bool playAnimation = true) {
		
		// PLAY DEFENSE ANIMATION (LOOPING)
		if (playAnimation) {
			owner.animation.PlayTargetAction("Defense_Action_Start", 0.1f, true, true, true, false);
		}

		usingDefenseMagic = true; // IF THIS FLAG IS ENABLED, DAMAGE WILL CALCULATE SPECIAL
		
		// IF PLAYER DOESN'T EQUIP PROTECTION, JUST PERFORMING ACTION IN GUARD 
		if (owner.inventory.protectionSlot != null) {
			
			// set negation
			owner.attribute.damageNegation *= owner.inventory.protectionSlot.negationBoost;
			
			defenseMagicCollider.SetActive(true);
		}

		yield break;
	}

	public IEnumerator ReleaseDefenseMagic(bool playAnimation = true, bool releaseWithParrying = true) {

		if (playAnimation) {
			owner.animation.PlayTargetAction("Default Movement", 0.35f, false);
		}

		usingDefenseMagic = false;

		// ACTIVE DEFENSE MAGIC IF PLAYER EQUIPPING PROTECTION
		if (owner.inventory.protectionSlot != null) {
			
			// set negation
			owner.attribute.damageNegation /= owner.inventory.protectionSlot.negationBoost;
			
			defenseMagicCollider.SetActive(false);
			
		}


		if (releaseWithParrying) {
			isParrying = true;

			yield return new WaitForSeconds(0.3f);

			isParrying = false;
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