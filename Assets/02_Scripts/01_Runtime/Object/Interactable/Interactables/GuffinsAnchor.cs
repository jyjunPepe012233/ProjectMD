using System.Collections;
using MinD.Runtime.Entity;
using MinD.Runtime.Managers;
using MinD.Runtime.System;
using MinD.Runtime.UI;
using MinD.SO.Object;
using UnityEngine;
using UnityEngine.AI;

namespace MinD.Runtime.Object.Interactables {

public class GuffinsAnchor : Interactable {
	
	private static Vector3 playerPosition = new Vector3(0, 0f, 1.2f);
	
	
	[HideInInspector] public int anchorId;


	[Header("[ Anchor Setting ]")]
	public GuffinsAnchorInformation anchorInfo;
	
	[Space(5)]
	public bool isDiscovered;
	
	

	
	public override void Interact(Player interactor) {
		
		// CHECK FLAGS TO MAKE SURE USING ANCHOR
		if (!interactor.isGrounded) {
			return;
		}
		if (interactor.isPerformingAction) {
			return;
		}
		

		if (isDiscovered) {
			StartCoroutine(UseGuffinsAnchor(interactor));
			
		} else {
			DiscoverGuffinsAnchor(interactor);
		}
		
	}



	private IEnumerator UseGuffinsAnchor(Player interactor) {
		
		// DISABLE ALL DAMAGEABLE COLLIDER IN PLAYER 
		PhysicUtility.SetActiveChildrenColliders(interactor.transform, false, LayerMask.GetMask("Damageable Entity"));
		
		interactor.animation.PlayTargetAction("Anchor_Start", 0.2f, true, true, false, false);

		PlayerHUDManager.Instance.FadeInToBlack(1.5f);
		yield return new WaitForSeconds(2f);
		
		PlayerHUDManager.Instance.FadeOutFromBlack(0.5f);
		
		// PLACE PLAYER TO RIGHT POSITION(WHERE IN FRONT OF ANCHOR)
		if (NavMesh.SamplePosition(transform.TransformPoint(playerPosition), out NavMeshHit hitInfo, 1f, NavMesh.AllAreas)) {

			// DISABLE CHARACTER CONTROLLER TO SETTING POSITION BY TRANSFORM ASSIGN 
			interactor.cc.enabled = false;
			interactor.transform.position = hitInfo.position;
			interactor.cc.enabled = true;

			Vector3 playerDirection = transform.position - interactor.transform.position;
			playerDirection.y = 0;
			interactor.transform.forward = playerDirection;
		}
		
		
		
		WorldRefreshToGuffinsAnchor();
		
	}
	
	

	private void DiscoverGuffinsAnchor(Player interactor) {
		
		isDiscovered = true;
			
		var discoverPopup = PlayerHUDManager.Instance.playerHUD.anchorDiscoveredPopup;
		PlayerHUDManager.Instance.PlayBurstPopup(discoverPopup);
			
		interactor.animation.PlayTargetAction("Anchor_Discover", 0.2f, true, true, false, false);
	}
	
	
	
	private void WorldRefreshToGuffinsAnchor() {
		
		PlayerManager.Instance.RefreshPlayer();
		WorldEnemyManager.Instance.ResetAllEnemyOnWorld();
		
	}
}

}