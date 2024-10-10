using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateObjectMenu : MonoBehaviour {
	
	
	[MenuItem("GameObject/Create MinD Object/Create Damage Collider", false, int.MinValue + 0)]
	public static void CreateNewDamageCollider() {
		
		// CREATE NEW DROPPED ITEM
		GameObject newCollider = new GameObject("Damage Collider", typeof(DamageCollider), typeof(BoxCollider));
		
		GameObjectUtility.SetParentAndAlign(newCollider, Selection.activeGameObject);
		
		Undo.RegisterCreatedObjectUndo(newCollider, "Create Damage Collider");
		Selection.activeGameObject = newCollider;
		
		
		// SET PROPERTIES
		Collider newItemCollider = newCollider.GetComponent<Collider>();
		newItemCollider.isTrigger = true;
	}
	
	[MenuItem("GameObject/Create MinD Object/Create Dropped Item", false, int.MinValue + 1)]
	public static void CreateNewDroppedItem() {
		
		// CREATE NEW DROPPED ITEM
		GameObject newItem = new GameObject("Dropped Item", typeof(DroppedItem), typeof(SphereCollider));
		
		GameObjectUtility.SetParentAndAlign(newItem, Selection.activeGameObject);
		
		Undo.RegisterCreatedObjectUndo(newItem, "Create Dropped Item");
		Selection.activeGameObject = newItem;
		
		
		// SET PROPERTIES
		Collider newItemCollider = newItem.GetComponent<Collider>();
		newItemCollider.isTrigger = true;
	}
	
}
