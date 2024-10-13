using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MinD;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour {

	public Player owner;
	public float mouseSensitive = 1.5f;

	[Header("[ Runtime Value ]")]
	[SerializeField] private float modify_distance; // 카메라 거리를 일시적으로 변경할 수치
	
	[Header("[ Settings ]")]
	[SerializeField] private Vector3 cameraOffset;
	[SerializeField, Range(0, 90)] private float limitAngleAbove = 6 ;
	[SerializeField, Range(0, 90)] private float limitAngleBelow = 40;
	[SerializeField] private float cameraMaxDistance = 3.5f;
	[SerializeField] private float cameraFollowSpeed = 13;
	[SerializeField] private float cameraRadius = 0.3f;
	[SerializeField] private LayerMask cameraCollisionMask;
	[Space(10)]
	[SerializeField] private float lockOnAngle;
	[SerializeField] private float lockOnMaxRadius;

	public Transform currentTargetOption;
	public Transform leftTargetOption;
	public Transform rightTargetOption;
	

	private Vector3 targetCameraArm;
	private Vector3 cameraArm;
		// Camera Arm Position
	private float cameraDistance;
	
	

	public void HandleCamera() {
		
		HandleFollowTarget();
		HandleRotation();
		HandleCollision();
		HandleLockOnTarget();

	}
	
	

	void HandleFollowTarget() {

		Vector3 angle = transform.eulerAngles;
		angle.x = 0;
		
		targetCameraArm = owner.transform.position + Quaternion.Euler(angle) * cameraOffset;

		cameraArm = Vector3.Lerp(cameraArm, targetCameraArm, Time.deltaTime * cameraFollowSpeed);
	}

	void HandleRotation() {

		if (owner.isLockOn) { // AUTO ROTATION BY LOCK ON

			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(currentTargetOption.position - transform.position), 10 * Time.deltaTime);
			Vector3 angle = transform.eulerAngles;
			
			
			// LIMIT ANGLE
			if (angle.x > 180)
				angle.x = Mathf.Clamp(angle.x, 360 - limitAngleAbove, 370);
			else
				angle.x = Mathf.Clamp(angle.x, -10, limitAngleBelow);
			
			angle.z = 0;
			
			
			transform.eulerAngles = angle;
			
		}
		else {
			
			Vector2 rotationInput = PlayerInputManager.Instance.rotationInput;
			Vector3 angle = transform.eulerAngles + new Vector3(-rotationInput.y * 0.35f, rotationInput.x) * mouseSensitive;

			
			// LIMIT ANGLE
			if (angle.x > 180)
				angle.x = Mathf.Clamp(angle.x, 360 - limitAngleAbove, 370);
			else
				angle.x = Mathf.Clamp(angle.x, -10, limitAngleBelow);
			
			angle.z = 0;
			

			transform.eulerAngles = angle;
		}
		
		
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

		if (PlayerInputManager.Instance.lockOnInput) {
			PlayerInputManager.Instance.lockOnInput = false;
			
			if (owner.isLockOn)
				RemoveLockOnTarget();
			else 
				SetLockOnTarget();
		}
		

		if (currentTargetOption != null)
			if (Vector3.Angle(transform.forward, currentTargetOption.position - transform.position) > lockOnAngle)
				RemoveLockOnTarget();
	}
	

	void SetLockOnTarget() {
		
		// GET ENTITY COLLIDERS IN AVAILABLE RADIUS
		Collider[] colliders = Physics.OverlapSphere(transform.position, lockOnMaxRadius, PhysicLayerDataBase.Instance.entityLayer);

		if (colliders.Length == 0)
			return;
		
		
		// CHECK AVAILABLE TARGETS
		List<Transform> availableTargets = new List<Transform>();
		foreach (Collider collider in colliders) {

			// GET ENTITY
			BaseEntity targetEntity = null;
			targetEntity = collider.GetComponentInParent<BaseEntity>();
			if (targetEntity == null)
				targetEntity = collider.GetComponent<BaseEntity>();


			// CHECK OPTIONS
			List<Transform> options = targetEntity.targetOptions;
			for (int i = 0; i < options.Count; i++) {

				// CHECK TARGET IS SELF
				if (owner.targetOptions.Contains(options[i]))
					continue;

				// CHECK TARGET IS ALREADY EXIST IN LIST
				if (availableTargets.Contains(options[i]))
					continue;

				// CHECK ANGLE AVAILABLE
				if (Mathf.Abs(Vector3.SignedAngle(transform.forward, (options[i].position - transform.position), Vector3.up)) > lockOnAngle)
					continue;

				// CHECK OBSTACLE BETWEEN CAMERA AND TARGET
				if (Physics.Linecast(owner.targetOptions[0].position, options[i].position, PhysicLayerDataBase.Instance.environmentLayer))
					continue;

				availableTargets.Add(options[i]);
			}
		}
		

		// SORTING TARGET OPTIONS BY PROXIMITY
		for (int i = 0; i < availableTargets.Count; i++) {
			// SELECTION SORT

			Transform closest = availableTargets[i];

			for (int j = i; j < availableTargets.Count; j++) {

				Transform thisOption = availableTargets[j];

				if (Vector3.Distance(transform.position, thisOption.position) < Vector3.Distance(transform.position, closest.position))
					(availableTargets[i], availableTargets[j]) = (availableTargets[j], availableTargets[i]);
			}
		}

		
		if (availableTargets[0] != null) {
			
			currentTargetOption = availableTargets[0];
			owner.isLockOn = true;
			
		}

	}

	void RemoveLockOnTarget() {
		
		currentTargetOption = null;
		owner.isLockOn = false;
		
	}
}
