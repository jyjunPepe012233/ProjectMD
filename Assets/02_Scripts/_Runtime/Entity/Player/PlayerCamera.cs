using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	public Transform player;
	public float mouseSensitive = 1.5f;

	[Header("[ Modify Value ]")]
	[SerializeField] private float modify_distance; // 카메라 거리를 일시적으로 변경할 수치
	[Header("[ Settings ]")]
	[SerializeField] private Vector3 cameraOffset;
	[SerializeField, Range(0, 90)] private float limitAngleAbove = 6 ;
	[SerializeField, Range(0, 90)] private float limitAngleBelow = 40;
	[SerializeField] private float cameraMaxDistance = 3.5f;
	[SerializeField] private float cameraFollowSpeed = 13;
	[SerializeField] private float cameraRadius = 0.3f;
	[SerializeField] private LayerMask cameraCollisionMask;

	private Vector3 targetCameraArm;
	private Vector3 cameraArm;
		// Camera Arm Position
	private float cameraDistance;
	
	

	private void Update() {
		
		HandleFollowTarget();
		HandleRotation();
		HandleCollision();
		HandleLockOnTarget();

	}
	
	

	void HandleFollowTarget() {

		Vector3 angle = transform.eulerAngles;
		angle.x = 0;
		
		targetCameraArm = player.position + Quaternion.Euler(angle) * cameraOffset;

		cameraArm = Vector3.Lerp(cameraArm, targetCameraArm, Time.deltaTime * cameraFollowSpeed);
	}

	void HandleRotation() {

		Vector2 rotationInput = PlayerInputManager.Instance.RotationInput;
		Vector3 angle = transform.eulerAngles + new Vector3(-rotationInput.y * 0.35f, rotationInput.x) * mouseSensitive;

		if (angle.x > 180) {
			angle.x = Mathf.Clamp(angle.x, 360 - limitAngleBelow, 370);
		} else
			angle.x = Mathf.Clamp(angle.x, -10, limitAngleAbove);
			// 각도 제한

		transform.eulerAngles = angle;
		
		transform.position = cameraArm + (transform.rotation * Vector3.back * cameraDistance);
	}

	void HandleCollision() {

		Ray ray = new Ray(cameraArm, transform.rotation * Vector3.back);
		if (Physics.SphereCast(ray, cameraRadius, out RaycastHit hitInfo, cameraMaxDistance, cameraCollisionMask)) {

			cameraDistance = Vector3.Distance(hitInfo.point, ray.origin);

		} else
			cameraDistance = cameraMaxDistance;
	}

	void HandleLockOnTarget() {
		
		// lock on
		
	}
}
