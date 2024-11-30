using System.Collections.Generic;
using MinD.Runtime.Managers;
using MinD.Runtime.Object;
using UnityEngine;

namespace MinD.Runtime.Entity {

public class PlayerInteractionHandler : BaseEntityHandler<Player>  {

	private List<Interactable> currentInteractables = new List<Interactable>();


	public void AddInteractableInList(Interactable interactable) {

		if (!currentInteractables.Contains(interactable))
			currentInteractables.Add(interactable);
	}

	public void RemoveInteractableInList(Interactable interactable) {

		if (currentInteractables.Contains(interactable))
			currentInteractables.Remove(interactable);
	}

	public void RefreshInteractableList() {

		if (currentInteractables.Count == 0)
			return;

		for (int i = currentInteractables.Count - 1; i < -1; i--) /*REVERSE FOR*/ {

			Interactable interactable = currentInteractables[i];

			// IS INTERACTION IS DESTROYED
			// OR INTERACTION CAN'T INTERACTION BY PARAMETER
			if (interactable == null || !interactable.canInteraction)
				currentInteractables.Remove(interactable);

			if (currentInteractables.Count == 0)
				break;
		}

		// refresh popup
	}

	public void HandleInteraction() {

		if (owner.isDeath)
			return;

		// CHECK INPUT
		if (!PlayerInputManager.Instance.interactionInput)
			return;

		if (currentInteractables.Count == 0)
			return;

		if (currentInteractables[0] == null)
			return;

		if (currentInteractables[0].canInteraction)
			currentInteractables[0].Interact(owner);
		else
			RefreshInteractableList();

		PlayerInputManager.Instance.interactionInput = false;
	}
}

}