using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using MinD;
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
	
	public FunctionColliderHandler ownedInfo;

	private float curComponentHeight;
	
		
	
	public void Open(FunctionColliderHandler ownedInfo) {

		this.ownedInfo = ownedInfo;

		// OPEN WINDOW
		FunctionColliderWindow window = GetWindow<FunctionColliderWindow>();

		window.titleContent = new GUIContent("Function Collider Editor");
		
		window.minSize = new Vector2(400, 150);
		window.maxSize = new Vector2(600, 450);
		
		window.Show();
		window.Focus();


		ownedInfo.LoadColliderBasicProperties();
	}


	public void OnGUI() {

		curComponentHeight = 0;

		GUILayout.BeginArea(new Rect(10, 5f, position.width - 20, 50));
		
		ownedInfo.showGizmoMode = (ShowGizmoMode)EditorGUILayout.EnumPopup("Gizmos Mode", ownedInfo.showGizmoMode);
		ownedInfo.showGizmosColor = EditorGUILayout.ColorField("Gizmos Color", ownedInfo.showGizmosColor);
		curComponentHeight += 50;
		
		GUILayout.EndArea();
		
		BeginComponent<DamageCollider>("Damage Collider", ref ownedInfo.damageCollider, 250);
		CreateDamageColliderOption();
		EndComponent();

	}

	private void BeginComponent<T>(string componentName, ref T component, float componentHeight) where T : MonoBehaviour {
		
		// START COMPONENT AREA
		GUILayout.BeginArea(new Rect(5, 5 + this.curComponentHeight, position.width-10, 250));
		this.curComponentHeight += componentHeight;
		
		
		var style = GUI.skin.GetStyle("Label");
		style.fontStyle = FontStyle.Bold;
		
		// DRAW COMPONENT CONTROL BAR
		GUILayout.BeginArea(new Rect(0, 0, position.width - 10, 25));
		EditorGUI.DrawRect(new Rect(0, 0, position.width - 10, 25), new Color(0.2f, 0.2f, 0.2f));
		EditorGUI.LabelField(new Rect(5, 0, position.width - 20, 25), componentName, style);
		
		GUILayout.BeginArea(new Rect(position.width - 55, 2.5f, 30, 20));
		if (component == null) {
			// ADD FUNCTION BUTTON
			if (GUILayout.Button(" + ", GUILayout.Width(30), GUILayout.Height(20))) {

				if (ownedInfo.GetComponent<T>() == null) {
					component = ownedInfo.AddComponent<T>();
					UnityEditorInternal.ComponentUtility.MoveComponentUp(component);
					// MOVE ABOVE COLLIDER COMPONENT
				}
			}

		} else {
			// REMOVE FUNCTION BUTTON
			if (GUILayout.Button(" - ", GUILayout.Width(30), GUILayout.Height(20))) {
				
				if (ownedInfo.GetComponent<T>() != null)
					DestroyImmediate(ownedInfo.GetComponent<T>());
				component = null;
				
			}
		} GUILayout.EndArea();
		
		// CLOSE COMPONENT CONTROL BAR
		GUILayout.EndArea();
		
	}

	private void EndComponent() {
		
		GUILayout.EndArea();
		
	}
	
	private void CreateDamageColliderOption() {
		

		if (ownedInfo.damageCollider != null) {
			
			GUILayout.BeginArea(new Rect(0, 30, position.width - 5, 200));
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
			
			CreateDamageColliderTypeSetting();
			
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


	private void CreateDamageColliderTypeSetting() {
		
		// CHECK CURRENT COLLIDER TYPE
		switch (ownedInfo.GetComponent<Collider>()) {
			case BoxCollider:
				ownedInfo.colliderType = FunctionColliderHandler.ColliderType.Box;
				break;
			case SphereCollider:
				ownedInfo.colliderType = FunctionColliderHandler.ColliderType.Sphere;
				break;
			case CapsuleCollider:
				ownedInfo.colliderType = FunctionColliderHandler.ColliderType.Capsule;
				break;
		}
			
		// SHOW GUI
		ownedInfo.colliderType = (FunctionColliderHandler.ColliderType)EditorGUILayout.EnumPopup("ColliderType", ownedInfo.colliderType);

		// CHANGE COLLIDER IF TYPE IS DIFFERENT
		Collider ownedCollider = ownedInfo.GetComponent<Collider>();
		switch (ownedInfo.colliderType) {
				
			case FunctionColliderHandler.ColliderType.Box:
					
				if (!(ownedCollider is BoxCollider)) {
						
					DestroyImmediate(ownedCollider);
					ownedInfo.AddComponent<BoxCollider>();
					ownedInfo.LoadColliderBasicProperties();
				}
				break;
				
			case FunctionColliderHandler.ColliderType.Sphere:
					
				if (!(ownedCollider is SphereCollider)) {
						
					DestroyImmediate(ownedCollider);
					ownedInfo.AddComponent<SphereCollider>();
					ownedInfo.LoadColliderBasicProperties();
				}
				break;
				
			case FunctionColliderHandler.ColliderType.Capsule:
					
				if (!(ownedCollider is CapsuleCollider)) {
						
					DestroyImmediate(ownedCollider);
					ownedInfo.AddComponent<CapsuleCollider>();
					ownedInfo.LoadColliderBasicProperties();
				}
				break;
		}
		
	}
}


