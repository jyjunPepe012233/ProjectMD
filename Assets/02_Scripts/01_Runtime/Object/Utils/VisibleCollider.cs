using System.Linq;
using MinD.Enums;
using MinD.Runtime.System;
using UnityEngine;

namespace MinD.Runtime.Object.Utils {

public class VisibleCollider : MonoBehaviour {
	
	[SerializeField] private ShowGizmoMode showGizmoMode;
	[SerializeField] private bool showChildCollider;
	
	[Space(10)] 
	public bool useCustomColor;
	public Color color;
	
	

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
			if (cols.Length == 0) {
				return;
			}
			
			if (useCustomColor) {
				foreach (Collider col in cols)
					GizmosUtility.DrawColliderGizmo(col, color);
			} else {
				foreach (Collider col in cols)
					GizmosUtility.DrawColliderGizmo(col, null);
			}
			
		} else {

			Collider col = GetComponent<Collider>();
			if (col == null) {
				return;
			}
			
			if (useCustomColor) {
				GizmosUtility.DrawColliderGizmo(col, color);
			} else {
				GizmosUtility.DrawColliderGizmo(col, null);
			}
			
		}
		
	}

}

}