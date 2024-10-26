using System.Collections;
using MinD.SO.Item.Items;
using MinD.Structs;
using UnityEngine;

namespace MinD.Runtime.Object.Magics {

public class LghtOfDrgnSlyngMainObj : MonoBehaviour {

	[Header("[ VFX Settings ]")]
	[SerializeField] private ParticleSystem circleSystem;
	
	[Space(3)]
	[SerializeField] private float circleScale;
	
	[Space(3)]
	[SerializeField] private Color circleStartColor;
	[SerializeField] private Color circleEndColor;
	
	[Space(10)]
	[SerializeField] private ParticleSystem blastSystem;

	
	[Header("[ Other ]")]
	[SerializeField] private GameObject projectile;


	private WaitForSeconds damageYieldTick;
	private Coroutine blastingCoroutine;

	private Vector3 blastPosition = new Vector3(0, 0, 2.5f);
	private Damage blastDamage;

	private LghtOfDrgnSlyng magicSO;



	public void SetUp(LghtOfDrgnSlyng magicSO, Damage damage, float damageTick) {

		this.magicSO = magicSO;
		blastDamage = damage;
		damageYieldTick = new WaitForSeconds(Mathf.Max(damageTick, 0.1f)); // MINIMUM TIME OF DAMAGE TICK IS 0.1 SECOND 
	}



	public void PlayWarmUpVfx() {
		StartCoroutine(PlayMagicCircleVFX(1.5f));
	}

	private IEnumerator PlayMagicCircleVFX(float duration) {

		circleSystem.Play();
		var renderer = circleSystem.GetComponent<Renderer>();

		float elapsedTime = 0;
		while (elapsedTime < duration) {

			// SET MAGIC CIRCLE FADE IN
			Color fadingColor = Color.Lerp(circleStartColor, circleEndColor, elapsedTime / duration);
			renderer.material.SetColor("_TintColor", fadingColor);

			elapsedTime += Time.deltaTime;
			yield return null;
		}
	}



	public void StartBlasting() {
		blastingCoroutine = StartCoroutine(BlastingCoroutine());
	}

	private IEnumerator BlastingCoroutine() {

		blastSystem.Play();

		// DAMAGING TARGET IN EVERY DAMAGE TICK
		// AND TYR DRAIN THE PLAYER STATS(MP, STAMINA)
		while (true) {

			if (magicSO.TryDrainMpAndStaminaDuringBlasting()) {

				ThrowDamageCollider();
				yield return damageYieldTick;

			} else {
				magicSO.EndBlasting();
				yield break;
			}
		}
	}

	private void ThrowDamageCollider() {

		var newProjectile = Instantiate(projectile).GetComponent<LghtOfDrgnSlyngProjectile>(); // NEED TO CHANGE POOLING
		newProjectile.Shoot(transform.TransformPoint(blastPosition), transform.forward, 25, 32.5f, blastDamage);
	}



	// CALL BY EndBlasting() METHOD IN MAGIC ITEM SO
	public IEnumerator EndBlastingCoroutine(float duration) {

		StopCoroutine(blastingCoroutine);


		// OFF BLASTING FX
		blastSystem.Stop();

		// FADE OUT MAGIC CIRCLE
		var renderer = circleSystem.GetComponent<Renderer>();

		float elapsedTime = 0;
		while (elapsedTime < duration) {

			// SET MAGIC CIRCLE FADE IN
			Color fadingColor = Color.Lerp(circleEndColor, circleStartColor, elapsedTime / duration);
			renderer.material.SetColor("_TintColor", fadingColor);

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		Destroy(gameObject);
	}

}

}