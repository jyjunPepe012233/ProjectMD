using MinD.Enums;
using MinD.Runtime.System;
using UnityEngine;

namespace MinD.Runtime.Object.Utils {

[RequireComponent(typeof(Collider))]
public class VisibleCollider : MonoBehaviour {
	
	[SerializeField] private ShowGizmoMode showGizmoMode;
	[SerializeField] private bool showChildCollider;
	
	[Space(10)]
	[SerializeField] private bool useCustomColor;
	[SerializeField] private Color color;
	
	

	public void OnDrawGizmos() {

		if (showGizmoMode != ShowGizmoMode.Always)
			return;
		
		DrawColliderGizmo();
	}
	
	public void OnDrawGizmosSelected() {

		if (showGizmoMode != ShowGizmoMode.Selected)
			return;

		DrawColliderGizmo();
	}

	private void DrawColliderGizmo() {
		
		if (showChildCollider) {

			Collider[] cols = GetComponentsInChildren<Collider>(true);
			
			if (useCustomColor) {
				foreach (Collider col in cols) {
					GizmosUtility.DrawColliderGizmo(col, color);
				}
			} else {
				foreach (Collider col in cols) {
					GizmosUtility.DrawColliderGizmo(col, null);
				}
			}
			
		} else {

			Collider col = GetComponent<Collider>();
			
			if (useCustomColor) {
				GizmosUtility.DrawColliderGizmo(col, color);
			} else {
				GizmosUtility.DrawColliderGizmo(col, null);
			}
			
		}
		
	}

}

}