using MinD.Runtime.Object;
using UnityEditor;
using UnityEngine;

namespace MinD.Editor.Inspector {

[CustomEditor(typeof(FunctionColliderHandler))]
public class FunctionColliderHandlerEditor : UnityEditor.Editor {

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

}