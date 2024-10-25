using System;
using System.Collections;
using MinD.Combat;
using UnityEngine;

namespace MinD.Object.Magics {

	public class LightOfDragonSlaying : MonoBehaviour {
		
		[Header("[ Magic Circle (WarmUp VFX) ]")]
		[SerializeField] private ParticleSystem circleSystem;
		[Space(5)]
		[SerializeField] private float circleScale;
		[Space(5)]
		[SerializeField] private Color circleStartColor;
		[SerializeField] private Color circleEndColor;

		[Header("[ Blast ]")]
		[SerializeField] private ParticleSystem blastSystem;
		[SerializeField] private FunctionColliderHandler fCollider;

		private WaitForSeconds damageYieldTick;
		private Coroutine blastingCoroutine;

		private MinD.Magics.LightOfDragonSlaying magicSO;
		
		

		public void OnEnable() {
			blastSystem.Stop();
			fCollider.SetCollisionActive(false);
		}

		public void SetUp(MinD.Magics.LightOfDragonSlaying magicSO, Damage damage, float damageTick) {
			
			fCollider.damageCollider.damage = damage;
			damageYieldTick = new WaitForSeconds(Mathf.Max(damageTick, 0.1f)); // MINIMUM TIME OF DAMAGE TICK IS 0.1 SECOND 

			this.magicSO = magicSO;
		}

		public void PlayWarmUpVfx() {
			StartCoroutine(PlayMagicCircleVfx(1.5f));
		}

		public void StartBlasting() {
			blastingCoroutine = StartCoroutine(BlastingCoroutine());
		}
			
			
		private IEnumerator PlayMagicCircleVfx(float duration) {
			
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
		
		private IEnumerator BlastingCoroutine() {
			
			blastSystem.Play();
			fCollider.SetCollisionActive(true);

			// DAMAGING TARGET IN EVERY DAMAGE TICK
			// AND TYR DRAIN THE PLAYER STATS(MP, STAMINA)
			while (true) {

				if (magicSO.TryDrainMpAndStaminaDuringBlasting()) {
					
					fCollider.ResetTargetsInAllColliders();
					yield return damageYieldTick;
					
				} else {
					magicSO.EndBlasting();
					yield break;
				}
			}
		}

		
		// CALL BY EndBlasting() METHOD IN MAGIC ITEM SO
		public IEnumerator EndBlastingCoroutine(float duration) {

			StopCoroutine(blastingCoroutine);
			fCollider.SetCollisionActive(false);
			
			
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