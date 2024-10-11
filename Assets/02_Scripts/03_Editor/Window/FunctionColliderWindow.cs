using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
using Label = UnityEngine.UIElements.Label;
using PopupWindow = UnityEditor.PopupWindow;

public class FunctionColliderWindow : EditorWindow {
	
	public enum ColliderType {
		Box,
		Sphere,
		Capsule
	}

	public FunctionColliderHandler ownedInfo;
	
	public ColliderType colliderType;
		
	
	public void Open(FunctionColliderHandler ownedInfo) {

		this.ownedInfo = ownedInfo;

		// OPEN WINDOW
		FunctionColliderWindow window = GetWindow<FunctionColliderWindow>();

		window.titleContent = new GUIContent("Function Collider Editor");
		
		window.minSize = new Vector2(400, 150);
		window.maxSize = new Vector2(600, 450);
		
		window.Show();
		window.Focus();
	}


	public void OnGUI() {

		CreateDamageColliderOption();

	}
	
	private void CreateDamageColliderOption() {
		
		#region OptionUpperBar
		var style = GUI.skin.GetStyle("Label");
		style.fontStyle = FontStyle.Bold;
		
		
		GUILayout.BeginArea(new Rect(5, 5, position.width - 10, 25));
		EditorGUI.DrawRect(new Rect(0, 0, position.width - 10, 25), new Color(0.2f, 0.2f, 0.2f));
		EditorGUI.LabelField(new Rect(5, 0, position.width - 20, 25), "Damage Collider", style);
		
		GUILayout.BeginArea(new Rect(position.width - 55, 2.5f, 30, 20));
		if (ownedInfo.damageCollider == null) {
			// ADD FUNCTION BUTTON
			if (GUILayout.Button(" + ", GUILayout.Width(30), GUILayout.Height(20))) {

				if (ownedInfo.GetComponent<DamageCollider>() == null) {
					ownedInfo.damageCollider = ownedInfo.AddComponent<DamageCollider>();
					UnityEditorInternal.ComponentUtility.MoveComponentUp(ownedInfo.damageCollider);
						// MOVE ABOVE COLLIDER COMPONENT
				}
			}

		} else {
			// REMOVE FUNCTION BUTTON
			if (GUILayout.Button(" - ", GUILayout.Width(30), GUILayout.Height(20))) {
				
				if (ownedInfo.GetComponent<DamageCollider>() != null)
					DestroyImmediate(ownedInfo.GetComponent<DamageCollider>());
				ownedInfo.damageCollider = null;
				
			}
		}
		GUILayout.EndArea();
		
		GUILayout.EndArea();
		#endregion

		if (ownedInfo.damageCollider != null) {
			
			GUILayout.BeginArea(new Rect(5, 35, position.width - 5, 200));
			EditorGUI.DrawRect(new Rect(0, 0, position.width - 10, 200), new Color(0.2f, 0.2f, 0.2f));
			
			
			#region Damage Setting
			GUILayout.BeginArea(new Rect(10, 5, (position.width / 2) - 20, 200));
			GUILayout.Label("Damage Setting");
			EditorGUI.indentLevel++;
			
			ownedInfo.damageCollider.damage.physical = EditorGUILayout.IntField("Physical", ownedInfo.damageCollider.damage.physical);
			ownedInfo.damageCollider.damage.magic = EditorGUILayout.IntField("Magic", ownedInfo.damageCollider.damage.magic);
			ownedInfo.damageCollider.damage.fire = EditorGUILayout.IntField("Fire", ownedInfo.damageCollider.damage.fire);
			ownedInfo.damageCollider.damage.frost = EditorGUILayout.IntField("Frost", ownedInfo.damageCollider.damage.frost);
			ownedInfo.damageCollider.damage.lightning = EditorGUILayout.IntField("Lightning", ownedInfo.damageCollider.damage.lightning);
			ownedInfo.damageCollider.damage.holy = EditorGUILayout.IntField("Holy", ownedInfo.damageCollider.damage.holy);
			
			EditorGUI.indentLevel--;
			GUILayout.EndArea();
			#endregion
			
			#region Collider Setting
			GUILayout.BeginArea(new Rect(position.width / 2, 5, (position.width / 2) - 20, 200));
			GUILayout.Label("Collider Setting");
			EditorGUI.indentLevel++;
			
			#region ColliderShapeSetting

			switch (ownedInfo.GetComponent<Collider>()) {
				case BoxCollider:
					colliderType = ColliderType.Box;
					break;
				case SphereCollider:
					colliderType = ColliderType.Sphere;
					break;
				case CapsuleCollider:
					colliderType = ColliderType.Capsule;
					break;
			}
			
			colliderType = (ColliderType)EditorGUILayout.EnumPopup("ColliderType", colliderType);

			Collider ownedCollider = ownedInfo.GetComponent<Collider>();
			switch (colliderType) {
				
				case ColliderType.Box:
					if (!(ownedCollider is BoxCollider)) {
						DestroyImmediate(ownedCollider);
						ownedInfo.AddComponent<BoxCollider>();
					}
					break;
				
				case ColliderType.Sphere:
					if (!(ownedCollider is SphereCollider)) {
						DestroyImmediate(ownedCollider);
						ownedInfo.AddComponent<SphereCollider>();
					}
					break;
				
				case ColliderType.Capsule:
					if (!(ownedCollider is CapsuleCollider)) {
						DestroyImmediate(ownedCollider);
						ownedInfo.AddComponent<CapsuleCollider>();
					}
					break;
				
			}
				
			#endregion
			
			GUILayout.Space(10);
			
			ownedInfo.damageCollider.againWhenExit =
				EditorGUILayout.Toggle(
					new GUIContent("Dynamic Target Set", "if entity is exit, can damage again without reactivated"), 
					ownedInfo.damageCollider.againWhenExit
					);
			
			EditorGUI.indentLevel--;
			GUILayout.EndArea();
			#endregion
			

			GUILayout.EndArea();
		}
	}
}


