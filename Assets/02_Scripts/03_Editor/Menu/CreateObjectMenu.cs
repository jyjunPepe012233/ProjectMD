using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;

public class CreateObjectMenu : MonoBehaviour {
	
	
	[MenuItem("GameObject/Create With MinD/Create Function Collider", false, int.MinValue + 0)]
	public static void CreateNewFunctionCollider() {
		
		// CREATE NEW ITEM
		GameObject newItem = new GameObject("Function Collider", typeof(FunctionColliderHandler), typeof(BoxCollider));
		
		GameObjectUtility.SetParentAndAlign(newItem, Selection.activeGameObject);
		
		Undo.RegisterCreatedObjectUndo(newItem, "Create Function Collider");
		Selection.activeGameObject = newItem;
		
		
		// SET PROPERTIES
		var component = newItem.GetComponent<FunctionColliderHandler>();
		component.LoadColliderBasicProperties();
		component.OpenEditorWindow();
	}
	
	[MenuItem("GameObject/Create With MinD/Create Dropped Item", false, int.MinValue + 1)]
	public static void CreateNewDroppedItem() {
		
		// CREATE NEW ITEM
		GameObject newItem = new GameObject("Dropped Item", typeof(DroppedItem), typeof(SphereCollider));
		
		GameObjectUtility.SetParentAndAlign(newItem, Selection.activeGameObject);
		
		Undo.RegisterCreatedObjectUndo(newItem, "Create Dropped Item");
		Selection.activeGameObject = newItem;
		
		
		// SET PROPERTIES
		Collider newItemCollider = newItem.GetComponent<Collider>();
		newItemCollider.isTrigger = true;
	}
	
}
