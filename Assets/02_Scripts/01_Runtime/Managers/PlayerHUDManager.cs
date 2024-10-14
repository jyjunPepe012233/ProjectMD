using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MinD.UI;

public class PlayerHUDManager : Singleton<PlayerHUDManager> {

	public Player player;
	
	public PlayerHUD playerHUD;

	
	private void OnEnable() {
		player = FindObjectOfType<Player>();
		playerHUD = FindObjectOfType<PlayerHUD>();
	}


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
		playerHUD.hpBar.SetValue(player.curHP);
	}
	public void RefreshMPBar() {
		playerHUD.mpBar.SetMaxValue(player.attribute.maxMp);
		playerHUD.mpBar.SetValue(player.curMp);
	}
	public void RefreshStaminaBar() {
		playerHUD.staminaBar.SetMaxValue(player.attribute.maxStamina);
		playerHUD.staminaBar.SetValue(player.curStamina);
	}
	
	

}
