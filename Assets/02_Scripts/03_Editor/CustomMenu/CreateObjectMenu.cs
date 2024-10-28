using MinD.Runtime.Entity;
using MinD.Runtime.Object;
using MinD.Runtime.Object.Interactables;
using MinD.Runtime.Object.Utils;
using UnityEditor;
using UnityEngine;

namespace MinD.Editor.CustomMenu {

public class CreateObjectMenu : MonoBehaviour {


	[MenuItem("GameObject/Create With MinD/Create Function Collider", false, int.MinValue + 0)]
	public static void CreateNewFunctionCollider() {

		// CREATE NEW ITEM
		GameObject newItem = new GameObject("Function Collider", typeof(FunctionColliderHandler), typeof(BoxCollider), typeof(VisibleCollider));

		GameObjectUtility.SetParentAndAlign(newItem, Selection.activeGameObject);

		Undo.RegisterCreatedObjectUndo(newItem, "Create Function Collider");
		Selection.activeGameObject = newItem;

		newItem.layer = LayerMask.NameToLayer("DamageCollider");

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

	[MenuItem("GameObject/Create With MinD/Create Target Option", false, int.MinValue + 2)]
	public static void CreateTargetOption() {

		if (Selection.activeGameObject == null) {
			foreach (SceneView sceneView in SceneView.sceneViews)
				sceneView.ShowNotification(new GUIContent("Target option should be child of some object"));
			return;
		}


		// COUNT TARGET OPTION ALREADY EXIST
		Transform root = Selection.activeGameObject.transform.root;
		Transform[] allChildren = root.GetComponentsInChildren<Transform>();
		int targetOptionCount = 1;

		foreach (Transform child in allChildren) {
			if (child.name.Contains("Target Option"))
				targetOptionCount++;
		}


		// CREATE NEW ITEM
		GameObject newItem = new GameObject("Target Option (" + targetOptionCount + ")");

		GameObjectUtility.SetParentAndAlign(newItem, Selection.activeGameObject);

		Undo.RegisterCreatedObjectUndo(newItem, "Target Option");
		Selection.activeGameObject = newItem;


		// BIND TARGET OPTION IN ENTITY
		BaseEntity entity = root.GetComponent<BaseEntity>();

		if (entity != null)
			entity.targetOptions.Add(newItem.transform);
		else
			foreach (SceneView sceneView in SceneView.sceneViews)
				sceneView.ShowNotification(new GUIContent("The new target option wasn't bound to entity"));
	}


}

}