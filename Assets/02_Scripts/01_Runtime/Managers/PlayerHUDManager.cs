using System.Collections;
using MinD.Runtime.Entity;
using MinD.Runtime.UI;
using UnityEngine;
using UnityEngine.Playables;

namespace MinD.Runtime.Managers {

public class PlayerHUDManager : Singleton<PlayerHUDManager> {

	public Player player;
	public PlayerHUD playerHUD;

	public bool isPlayingBurstPopup;
	
	public bool isFadingWithBlack;
	private Coroutine fadingBlackScreenCoroutine; 
	


	public void Update() {

		if (player == null)
			return;

		HandleStatusBar();
	}
	
	
	
	private void HandleStatusBar() {

		playerHUD.hpBar.HandleTrailFollowing();
		playerHUD.mpBar.HandleTrailFollowing();
		playerHUD.staminaBar.HandleTrailFollowing();

	}
	
	public void RefreshAllStatusBar() {
		RefreshHPBar();
		RefreshMPBar();
		RefreshStaminaBar();
	}
	
	public void RefreshHPBar() {
		playerHUD.hpBar.SetMaxValue(player.attribute.maxHp);
		playerHUD.hpBar.SetValue(player.CurHp);
	}
	public void RefreshMPBar() {
		playerHUD.mpBar.SetMaxValue(player.attribute.maxMp);
		playerHUD.mpBar.SetValue(player.CurMp);
	}
	public void RefreshStaminaBar() {
		playerHUD.staminaBar.SetMaxValue(player.attribute.maxStamina);
		playerHUD.staminaBar.SetValue(player.CurStamina);
	}



	public void PlayBurstPopup(PlayableDirector burstPopupDirector, bool playWithForce = false) {

		if (isPlayingBurstPopup) {
			
			if (playWithForce) {
				StartCoroutine(PlayYouDiedPopupCoroutine(burstPopupDirector));
			} else {
				throw new UnityException("!! BURST POPUP IS ALREADY PLAYING!");
			}
			
		} else {
			StartCoroutine(PlayYouDiedPopupCoroutine(burstPopupDirector));
			
		}
		
	}

	private IEnumerator PlayYouDiedPopupCoroutine(PlayableDirector burstPopupDirector) {

		burstPopupDirector.gameObject.SetActive(true);
		burstPopupDirector.Play();

		yield return new WaitForSeconds((float)burstPopupDirector.duration);
		
		burstPopupDirector.gameObject.SetActive(false);

	}



	public void FadeInToBlack(float duration) {

		if (isFadingWithBlack) {
			StopCoroutine(fadingBlackScreenCoroutine);
		}

		fadingBlackScreenCoroutine = StartCoroutine(FadeBlackScreen(duration, true));
	}

	public void FadeOutFromBlack(float duration) {
		
		if (isFadingWithBlack) {
			StopCoroutine(fadingBlackScreenCoroutine);
		}
		
		fadingBlackScreenCoroutine = StartCoroutine(FadeBlackScreen(duration, false));
	}

	private IEnumerator FadeBlackScreen(float duration, bool fadeDirection) {
		
		isFadingWithBlack = true;
		playerHUD.blackScreen.gameObject.SetActive(true);
		
		playerHUD.blackScreen.color = new Color(0, 0, 0, fadeDirection ? 0 : 1);
		
		
		float elapsedTime = 0;
		while (true) {

			elapsedTime += Time.deltaTime;
			
			playerHUD.blackScreen.color = new Color(0, 0, 0, (fadeDirection ? (elapsedTime / duration) : (1-elapsedTime / duration))); 
			yield return null;

			if (elapsedTime > duration) { 
				break;
			}
		}
		
		
		isFadingWithBlack = false;
		playerHUD.blackScreen.gameObject.SetActive(fadeDirection);
		
		playerHUD.blackScreen.color = new Color(0, 0, 0, fadeDirection ? 1 : 0);
		
	}

	
}

}