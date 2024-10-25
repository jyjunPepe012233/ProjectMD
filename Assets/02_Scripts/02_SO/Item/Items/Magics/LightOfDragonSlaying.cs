using System.Collections;
using System.Runtime.Serialization;
using MinD.Combat;
using UnityEngine;
using UnityEngine.Serialization;

namespace MinD.Magics {

	[CreateAssetMenu(menuName="MinD/Item/Items/Magics/Light Of DragonSlaying")]
	public class LightOfDragonSlaying : Magic {
		// 멸룡의 빛

		[Header("[ Setting ]")]
		[SerializeField] private Damage damage;
		[SerializeField] private float damageTick;
		
		[FormerlySerializedAs("isInputEnded")]
		[Header("[ Flags ]")] 
		public bool isInputReleased;
		public bool isBlasting;
		
		private Object.Magics.LightOfDragonSlaying magic;

		private float blastTimer;
		
		
		
		public override void OnUse() {
			
			// START CHARGING
			castPlayer.animation.PlayTargetAction("Light_Of_DragonSlaying_Start", true, true, false, false);

			
			// INSTANTIATE MAGIC OBJECT AND START WARMUP VFX
			magic = ObjectDataBase.Instance.InstantiateMagic("Light_Of_DragonSlaying").GetComponent<Object.Magics.LightOfDragonSlaying>();

			magic.transform.position = castPlayer.transform.TransformPoint(0, 1.5f, 2);
			
			if (castPlayer.combat.target != null) {
				magic.transform.forward = (castPlayer.camera.currentTargetOption.position - magic.transform.position);
			} else {
				magic.transform.forward = castPlayer.transform.forward;
			}

			
			magic.PlayWarmUpVfx();
		}

		public override void Tick() {
			
			// ON ANIMATION IS END, END THE MAGIC
			if (castPlayer.isPerformingAction == false) {
				castPlayer.combat.ExitCurrentMagic();
				return;
			}
			
			
			// INPUT IS RELEASED DURING BLASTING
			if (isBlasting && isInputReleased) {

				blastTimer += Time.deltaTime;

				if (blastTimer > 0.3f) { // MINIMUM DURATION OF BLASTING
					EndBlasting();
				}
			}
			
		}
		

		public override void OnReleaseInput() {
			isInputReleased = true;
		}

		public override void OnExit() {
			
			magic = null;
			
			// RESET FLAG
			isInputReleased = false;
			isBlasting = false;
			blastTimer = 0;
		}
		
		
		
		public override void OnSuccessfullyCast() {
			// WILL CALlED THIS FUNCTION WHEN WARM-UP ANIMATION IS ENDED

			// CAUSE THIS METHOD CALLED BY ANIMATION EVENT IN LOOP ANIMATION EVERY TIME
			if (isBlasting)
				return;

			// SET FLAGS
			isBlasting = true;
			
			magic.SetUp(this, damage, damageTick);
			magic.StartBlasting();
		}

		public override void OnCastIsEnd() {
		}


		public void EndBlasting() {

			isBlasting = false;
			magic.StartCoroutine(magic.EndBlastingCoroutine(2f));
			
			castPlayer.animation.PlayTargetAction("Light_Of_DragonSlaying_End", true, true, false, false);
			// AND WILL CALL OnCastIsEnd METHOD VIA ANIMATION EVENT
		}


		public bool TryDrainMpAndStaminaDuringBlasting() {

			// IF PLAYER HASN'T ENOUGH STATS, RETURN FALSE
			if (castPlayer.CurMp < mpCostDuring ||
			    castPlayer.CurStamina < staminaCostDuring) {
				return false;
			}
			
			// SUCCESSFULLY DRAIN MP AND STAMINA, RETURN TRUE
			castPlayer.CurMp -= mpCostDuring;
			castPlayer.CurStamina -= staminaCostDuring;

			return true;
		}

	}

}