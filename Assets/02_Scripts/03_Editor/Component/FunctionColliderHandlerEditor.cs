using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FunctionColliderHandler))]
public class FunctionColliderHandlerEditor : Editor {

	private FunctionColliderHandler owner;
	
	
	
	void OnEnable() {
		
		owner = target as FunctionColliderHandler;
		
	}

	public override void OnInspectorGUI() {

		// WINDOW BUTTON
		if (GUILayout.Button("Open Function Collider Editor", GUILayout.Height(30)))
			owner.OpenEditorWindow();

	}


}
