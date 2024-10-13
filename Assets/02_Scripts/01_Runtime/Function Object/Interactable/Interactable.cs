using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {

	[Header("[ Parameter ]")]
	public bool canInteraction = true;
	public string interactionText;
	
	protected virtual void OnTriggerEnter(Collider other) {

		if (!canInteraction)
			return;
		
		Player player = other.GetComponentInParent<Player>();

		// COLLIDER HASN'T ENTITY COMPONENT 
		if (player == null) {
			
			player = other.GetComponent<Player>();

			if (player == null)
				return;
		}
		
		player.interaction.AddInteractableInList(this);
		player.interaction.RefreshInteractableList();
	}

	protected virtual void OnTriggerExit(Collider other) {

		Player player = other.GetComponentInParent<Player>();
		
		// COLLIDER HASN'T ENTITY COMPONENT
		if (player == null) {
			
			player = other.GetComponent<Player>();
			
			if (player == null) 
				return;
		}
		
		player.interaction.RemoveInteractableInList(this);
		player.interaction.RefreshInteractableList();
	}
	
	public abstract void Interact(Player interactor);

}