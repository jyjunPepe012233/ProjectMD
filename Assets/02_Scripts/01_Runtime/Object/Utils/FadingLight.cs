using System.Collections;
using UnityEngine;


namespace MinD.Runtime.Object.Utils {

[RequireComponent(typeof(Light))]
public class FadingLight : MonoBehaviour {

	public float intensityValue;
	
	[HideInInspector] public Light light;
	private Coroutine currentFade;


	
	public void OnEnable() {
		light = GetComponent<Light>();

		light.intensity = 0;
		light.enabled = false;
	}

	public void FadeIn(float duration) {

		if (currentFade != null) {
			StopCoroutine(currentFade);
		}
		
		StartCoroutine(FadeInCoroutine(duration));
	}

	public void FadeOut(float duration, bool destroyWithEnd = false) {
		
		if (currentFade != null) {
			StopCoroutine(currentFade);
		}
		
		StartCoroutine(FadeOutCoroutine(duration, destroyWithEnd));
	}
	
	private IEnumerator FadeInCoroutine(float duration) {

		light.intensity = 0;
		light.enabled = true;
		
		while (true) {
			
			light.intensity += Time.deltaTime / duration * intensityValue; // BLAST LIGHT IS GOING BRIGHTNESS IN DURATION

			if (light.intensity >= intensityValue) {
				light.intensity = intensityValue; // CLAMP
				yield break;
			}
			
			yield return null;
		}
	}
	private IEnumerator FadeOutCoroutine(float duration, bool destroyWithEnd) {
		
		light.intensity = intensityValue;
		light.enabled = true;
		
		while (true) {

			light.intensity -= Time.deltaTime / duration * intensityValue; // BLAST LIGHT IS GOING BRIGHT DURING 0.5 SECOND
			yield return null;

			if (light.intensity <= 0) {
				break;
			}
		}
		
		// CHECK THIS OBJECT IS POOLED,
		// IF THIS IS POOLED OBJECT, DESTROY WITH OBJECT POOLING

		if (destroyWithEnd) {
			Destroy(gameObject);
		} else {
			light.intensity = 0;
			light.enabled = false;
		}
		
	}
}

}