using MinD.Editor.Window;
using MinD.Enums;
using MinD.Runtime.Entity;
using MinD.Runtime.System;
using UnityEngine;

namespace MinD.Runtime.Object {

public class FunctionColliderHandler : MonoBehaviour {

	public enum ColliderType {
		Box,
		Sphere,
		Capsule
	}
	public ColliderType colliderType;
	public Collider collider;
	
	// WINDOW INSTANCE
	public FunctionColliderWindow editorWindow;

	public DamageCollider damageCollider;


	
	

	public void OpenEditorWindow() {

		if (editorWindow == null)
			editorWindow = ScriptableObject.CreateInstance<FunctionColliderWindow>();

		editorWindow.Open(this);
	}

	public void LoadColliderBasicProperties() {

		collider = GetComponent<Collider>();
		collider.isTrigger = true;

	}
}

}