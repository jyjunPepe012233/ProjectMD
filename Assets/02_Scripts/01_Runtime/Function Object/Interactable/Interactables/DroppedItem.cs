using System.Collections;
using System.Collections.Generic;
using MinD;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class DroppedItem : Interactable {
		
	[Header("[ Item Settings ]")]
	[SerializeField] private Item item;
	[SerializeField] private int itemCount;


	public void Awake() {
		
		if (item == null)
			Destroy(gameObject);

		switch (item.itemRarity) {
			
			case (ItemRarityEnum.Common): 
				Instantiate(VfxDataBase.Instance.droppedItemCommon, transform);
				break;

			case (ItemRarityEnum.Rare): 
				Instantiate(VfxDataBase.Instance.droppedItemRare, transform);
				break;
				
			case (ItemRarityEnum.Legendary): 
				Instantiate(VfxDataBase.Instance.droppedItemLegendary, transform);
				break;
		}
		
	}
	
	
	
	public override void Interact(Player interactor) {

		if (interactor.inventory.AddItem(item.itemId, itemCount, false)) {
			// ADD ITEM IS CLEARLY WORK ELSE ITEM IS EXCEEDED
			
			interactor.interaction.RemoveInteractableInList(this);
			interactor.interaction.RefreshInteractableList();
			
			canInteraction = false;
			GetComponentInChildren<ParticleSystem>().Stop();
			
			Destroy(gameObject, 1f);

		} else { // IF ADD ITEM IS CANCELED CAUSE ITEM IS EXCEEDED MAX COUNT OF ITEM
			
			// function when item count is exceeded

		}
		
	}
}
