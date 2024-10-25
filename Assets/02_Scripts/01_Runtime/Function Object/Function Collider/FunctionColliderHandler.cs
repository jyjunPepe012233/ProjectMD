using System;
using System.Collections;
using System.Collections.Generic;
using MinD;
using UnityEngine;
using UnityEngine.Serialization;

public class FunctionColliderHandler : MonoBehaviour {
	
	public enum ColliderType {
		Box,
		Sphere,
		Capsule
	}

	public FunctionColliderWindow editorWindow;
	
	public ShowGizmoMode showGizmoMode;
	public Color showGizmosColor = new Color(1f, 0.1f, 0.1f, 0.65f);
	public ColliderType colliderType;

	public DamageCollider damageCollider;


	public Collider collider;



	public void ResetTargetsInAllColliders() {
		
		if (damageCollider != null)
			damageCollider.ResetDamagedEntity();

		collider.enabled = false;
		collider.enabled = true;
	}

	public void SetCollisionActive(bool active) {
		collider.enabled = active;
	}


	public void OpenEditorWindow() {
		
		if (editorWindow == null)
			editorWindow = ScriptableObject.CreateInstance<FunctionColliderWindow>();
		
		editorWindow.Open(this);
	}
	public void LoadColliderBasicProperties() {
		
		collider = GetComponent<Collider>();
		collider.isTrigger = true;
		
	}


	public void OnDrawGizmos() {

		if (showGizmoMode != ShowGizmoMode.Always)
			return;

		Gizmos.color = showGizmosColor;

		switch (colliderType) {
			
			case ColliderType.Box:
				DrawBoxColliderGizmo();
				break;
			
			case ColliderType.Sphere:
				DrawSphereColliderGizmo();
				break;
			
			case ColliderType.Capsule:
				DrawCapsuleColliderGizmo();
				break;
		}

	}

	public void OnDrawGizmosSelected() {
		
		if (showGizmoMode != ShowGizmoMode.Selected)
			return;

		Gizmos.color = showGizmosColor;
		
		switch (colliderType) {
			
			case ColliderType.Box:
				DrawBoxColliderGizmo();
				break;
			
			case ColliderType.Sphere:
				DrawSphereColliderGizmo();
				break;
			
			case ColliderType.Capsule:
				DrawCapsuleColliderGizmo();
				break;
		}
	}

	private void DrawBoxColliderGizmo() {

		var c = (BoxCollider)collider;
		
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawCube(c.center, c.size);
		
	}
	private void DrawSphereColliderGizmo() {
		
		var c = (SphereCollider)collider;
		
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawSphere(c.center, c.radius);
		
	}
	private void DrawCapsuleColliderGizmo() {
		
		var c = (CapsuleCollider)collider;
		
		Gizmos.matrix = transform.localToWorldMatrix;
		
		// CAPSULE HEAD(ROUND PART)
		float capsuleHeadPosition = Mathf.Clamp((c.height / 2 - c.radius), 0, Mathf.Infinity);
		Gizmos.DrawSphere(Vector3.up * capsuleHeadPosition + c.center, c.radius);
		Gizmos.DrawSphere(Vector3.down * capsuleHeadPosition + c.center, c.radius);
		
		
		// BETWEEN LERP
		var color = Gizmos.color;
		color.a *= 0.5f;
		Gizmos.color = color;
		
		int lerpSphereCount = (int)(c.height / c.radius);
		for (int i = 1; i < lerpSphereCount; i++) {
			Vector3 position = Vector3.Lerp(Vector3.up * capsuleHeadPosition, Vector3.down * capsuleHeadPosition, 1/(float)lerpSphereCount * i);
			Gizmos.DrawSphere(position + c.center, c.radius);
		}
		
		
		// CAPSULE STEM
		Gizmos.color = Color.white;
		Gizmos.DrawLine(Vector3.up * capsuleHeadPosition + c.center, Vector3.down * capsuleHeadPosition + c.center);
		
	}
}
