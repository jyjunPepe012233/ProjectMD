using System.Collections;
using MinD.Runtime.Entity;
using MinD.Runtime.UI;
using UnityEngine;
using UnityEngine.Playables;

namespace MinD.Runtime.Managers {

public class PlayerHUDManager : Singleton<PlayerHUDManager> {

	public Player player;

	public PlayerHUD playerHUD;



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



	public void PlayBurstPopup(PlayableDirector burstPopupDirector) {
		StartCoroutine(PlayYouDiedPopupCoroutine(burstPopupDirector));
	}

	private IEnumerator PlayYouDiedPopupCoroutine(PlayableDirector burstPopupDirector) {

		burstPopupDirector.gameObject.SetActive(true);
		burstPopupDirector.Play();

		yield return new WaitForSeconds((float)burstPopupDirector.duration);
		
		burstPopupDirector.gameObject.SetActive(false);

	}
	
}

}