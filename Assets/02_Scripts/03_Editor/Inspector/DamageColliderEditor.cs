using MinD.Runtime.Utils;
using MinD.SO.Utils;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace MinD.Editor.Inspector {

[CustomEditor(typeof(DamageCollider))]
public class DamageColliderEditor : UnityEditor.Editor {

	private DamageCollider component;



	void OnEnable() {
		
		component = target as DamageCollider;

	}

	public override void OnInspectorGUI() {
		
		GUILayout.Space(10);
		
		component.basedOnSO = EditorGUILayout.ToggleLeft("Based On Scriptable Object", component.basedOnSO);
		EditorGUILayout.Space(5);
		EditorGUI.indentLevel++;
		
		if (component.basedOnSO) {
			component.referenceData = (DamageColliderData)EditorGUILayout.ObjectField("Reference Data", component.referenceData, typeof(DamageColliderData), false);

		} else {
			
			var temp = component.damageEffect;

			EditorGUILayout.BeginFoldoutHeaderGroup(true, "Damage", EditorStyles.foldoutHeader);
			temp.damage.physical = EditorGUILayout.IntField("Physical", temp.damage.physical);
			temp.damage.magic = EditorGUILayout.IntField("Magic", temp.damage.magic);
			temp.damage.fire = EditorGUILayout.IntField("Fire", temp.damage.fire);
			temp.damage.frost = EditorGUILayout.IntField("Frost", temp.damage.frost);
			temp.damage.lightning = EditorGUILayout.IntField("Lightning", temp.damage.lightning);
			temp.damage.holy = EditorGUILayout.IntField("Holy", temp.damage.holy);
			EditorGUILayout.EndFoldoutHeaderGroup();

			EditorGUI.indentLevel--;
			
			int totalDamage = temp.physical + component.damage.magic + component.damage.fire + component.damage.frost + component.damage.lightning + component.damage.holy;
			EditorGUILayout.IntField("Total Damage", totalDamage);
			
			
			
			EditorGUILayout.Space(15);
			component.poiseBreakAmount = EditorGUILayout.IntSlider("Poise Break Damage", component.poiseBreakAmount, 0, 100);
			
		}

	}


}

}