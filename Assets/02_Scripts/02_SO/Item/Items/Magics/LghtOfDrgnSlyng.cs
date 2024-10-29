using MinD.Runtime.DataBase;
using MinD.Runtime.Object.Magics;
using MinD.SO.Utils;
using UnityEngine;
using UnityEngine.Rendering.UI;

namespace MinD.SO.Item.Items {

[CreateAssetMenu(menuName = "MinD/Item/Items/Magics/Light Of DragonSlaying")]
public class LghtOfDrgnSlyng : Magic {
	// 멸룡의 빛

	[Header("[ Settings ]")]
	[SerializeField] private DamageData damageData;
	[SerializeField] private float damageTick;
	
	[Header("[ Flags ]")]
	public bool isInputReleased;
	public bool isBlasting;

	private LghtOfDrgnSlyngMainObj magic;

	private float blastTimer;



	public override void OnUse() {

		// START CHARGING
		castPlayer.animation.PlayTargetAction("LghtOfDrgnSlyng_Start", true, true, false, false);


		// INSTANTIATE MAGIC OBJECT
		magic = ObjectDataBase.Instance.InstantiateMagic("LghtOfDrgnSlyng_MainObj").GetComponent<LghtOfDrgnSlyngMainObj>();
		magic.SetUp(this, damageData, damageTick);



		Vector3 pivot = castPlayer.targetOptions[0].position;

		// SET MAGIC POSITION AND DIRECTION
		if (castPlayer.combat.target != null) {

			// POSITION
			Vector3 targetDirx = (castPlayer.camera.currentTargetOption.position - pivot).normalized;
			magic.transform.position = pivot + Quaternion.LookRotation(targetDirx) * new Vector3(0, 0.5f, 2);


			// DIRECTION
			magic.transform.forward = targetDirx;
			Vector3 angle = magic.transform.eulerAngles;

			angle.x = Mathf.Clamp(angle.x, -20, 20);
			magic.transform.eulerAngles = angle;


		} else {
			magic.transform.position = pivot + (castPlayer.transform.rotation * new Vector3(0, 0.5f, 2));
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

			if (blastTimer > 0.3f) {
				// MINIMUM DURATION OF BLASTING
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

		magic.StartBlasting();
	}

	public override void OnCastIsEnd() {
	}


	public void EndBlasting() {

		isBlasting = false;
		magic.StartCoroutine(magic.EndBlastingCoroutine(2f));

		castPlayer.animation.PlayTargetAction("LghtOfDrgnSlyng_End", true, true, false, false);
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