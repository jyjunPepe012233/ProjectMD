using MinD.Runtime.Managers;
using MinD.Runtime.Utils;
using MinD.SO.Object;
using UnityEditor;
using UnityEngine;

namespace MinD.Editor.Inspector {

[CustomEditor(typeof(DamageCollider))]
public class GameManagerEditor : UnityEditor.Editor {

	private GameManager component;
	


	void OnEnable() {
		
		component = target as GameManager;

	}

	public override void OnInspectorGUI() {
		
		base.OnInspectorGUI();

		if (GUILayout.Button("Bake World")) {
//			component.BakeWorld();
		}

	}


}

}