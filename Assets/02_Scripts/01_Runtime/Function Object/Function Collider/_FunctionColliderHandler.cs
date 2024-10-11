using System;
using System.Collections;
using System.Collections.Generic;
using MinD;
using UnityEngine;

public class FunctionColliderHandler : MonoBehaviour {

	public FunctionColliderWindow editorWindow;
	
	public ShowGizmosMode showGizmosMode;
	public DamageCollider damageCollider;



	public void OpenEditorWindow() {
		
		if (editorWindow == null)
			editorWindow = ScriptableObject.CreateInstance<FunctionColliderWindow>();
		
		editorWindow.Open(this);
	}

	public void SetBaseObjectProperties() {
		
		Collider newItemCollider = GetComponent<Collider>();
		newItemCollider.isTrigger = true;
		
	}
}
