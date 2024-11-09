using System.Collections;
using MinD.Runtime.DataBase;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using PlayerInputManager = MinD.Runtime.Managers.PlayerInputManager;

namespace MinD.Runtime.Entity {

public class PlayerLocomotionHandler : MonoBehaviour {

	[HideInInspector] public Player owner;
	

	[Header("[ Setting ]")]
	[SerializeField] private float walkSpeed = 4;
	[SerializeField] private float runningSpeed = 6;
	[SerializeField] private float jumpForce = 3.5f;
	[SerializeField] private float jumpSpeedMultiplier = 0.75f;
	[SerializeField] private float rotationSpeed = 15;
	
	[Space(5)]
	[SerializeField] private float gravityForce = 9.8f;
	[SerializeField] private float groundedGravityForce = 20;
	[SerializeField] private float groundedCheckRadius = 0.2f;

	[Space(5)]
	[SerializeField] private float blinkDistance;
	
	[Header("[ Flags ]")]
	public bool isSprinting;
	[FormerlySerializedAs("isBlinking")] public bool duringBlink;

	
	private Vector3 yVelocity;
	private float inAirTimer;
	private bool fallVelocityHasSet;

	private Vector3 moveDirx; // move direction on world
	private Vector3 jumpDirx;

	private Vector3 blinkDirx; // blink direction on world



	public void HandleAllLocomotion() {
		
		HandleGroundedCheck();
		HandleGravity();
		
		HandleRotation();

		HandleMovement();
		HandleSprint();
		HandleJump();

		HandleBlink();

	}

	void HandleRotation() {

		if (!owner.canRotate)
			return;

		if (owner.isDeath) {
			return;
		}
		

		if (owner.isMoving) {

			Vector3 camDirx = owner.camera.transform.forward;
			camDirx.y = 0;
			camDirx.Normalize();


			// DEGREASE ROTATION SPEED WHEN PLAYER IS NOT GROUNDED
			float rotationSpeedTemp = rotationSpeed;
			if (!owner.isGrounded)
				rotationSpeedTemp *= 0.13f;

			if (owner.isLockOn) {
				if (isSprinting)
					transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveDirx), rotationSpeedTemp);
				else
					transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(camDirx), rotationSpeedTemp);
				
			} else {
				transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveDirx), rotationSpeedTemp);
				
			}
		}

	}

	void HandleMovement() {

		moveDirx = owner.camera.transform.forward;
		moveDirx.y = 0;
		moveDirx.Normalize();

		if (owner.isDeath) {
			return;
		}


		Vector3 inputDirx = PlayerInputManager.Instance.movementInput;
		inputDirx = new Vector3(inputDirx.x, 0, inputDirx.y);
		// MOVEMENT INPUT IS 2D, SO MAKE
		// INPUT DIRECTION = (MOVEMENT INPUT X, 0, MOVEMENT INPUT Y

		owner.isMoving = inputDirx.magnitude != 0;
		owner.animator.SetBool("IsMoving", owner.isMoving);


		if (owner.isMoving) {

			// SET MOVE DIRECTION BASED ON ROTATION
			moveDirx = Quaternion.LookRotation(moveDirx) * inputDirx;
			moveDirx *= Time.deltaTime;

			// CHECK FLAGS
			if (!owner.canMove)
				return;

			// IF PLAYER IS IN AIR, SET SPEED 1/4
			if (!owner.isGrounded)
				moveDirx *= 0.25f;

			if (isSprinting)
				owner.cc.Move(moveDirx * runningSpeed);
			else owner.cc.Move(moveDirx * walkSpeed);

		}


		// DON'T SETTING MOVE BLEND ANIMATION PARAMETER DURING BLINK
		if (duringBlink) {
			return;
		}

		if (owner.isLockOn) {
			if (isSprinting && owner.isMoving) {
				owner.animation.LerpMovementBlendTree(0, inputDirx.magnitude * 2);
			} else {
				owner.animation.LerpMovementBlendTree(inputDirx.x, inputDirx.z);
			}
		} else {
			if (isSprinting) {
				owner.animation.LerpMovementBlendTree(0, inputDirx.magnitude * 2);
			} else {
				owner.animation.LerpMovementBlendTree(0, inputDirx.magnitude);
			}
		}	

		
	}

	void HandleSprint() {
		
		if (owner.isDeath) {
			isSprinting = false;
			return;
		}
		
		isSprinting = PlayerInputManager.Instance.sprintInput;
	}

	void HandleGroundedCheck() {

		owner.isGrounded = Physics.CheckSphere(transform.position, groundedCheckRadius, PhysicLayerDataBase.Instance.environmentLayer);
		owner.animator.SetBool("IsGrounded", owner.isGrounded);

	}

	void HandleJump() {

		// JUMP MOVEMENT
		if (owner.isJumping && !owner.isGrounded)
			owner.cc.Move(jumpDirx * Time.deltaTime);


		// HANDLE INPUT
		if (PlayerInputManager.Instance.jumpInput == false)
			return;
		PlayerInputManager.Instance.jumpInput = false;
		
		
		if (owner.isDeath) {
			return;
		}

		if (owner.isPerformingAction)
			return;

		if (!owner.isGrounded)
			return;

		owner.isJumping = true;

		yVelocity.y = jumpForce;
		owner.animation.PlayTargetAction("Jump_Start", true, false, true, false);

		jumpDirx = moveDirx.normalized * (isSprinting ? runningSpeed : walkSpeed);
		jumpDirx *= jumpSpeedMultiplier;
		if (!owner.isMoving)
			jumpDirx *= 0;


	}

	void HandleGravity() {

		if (owner.isGrounded) {

			if (yVelocity.y < 0) {

				fallVelocityHasSet = false;

				yVelocity.y = -groundedGravityForce; // stickGroundForce
				inAirTimer = 0;

			}
		} else if (!fallVelocityHasSet && !owner.isJumping) {
			// SET BASE FALL SPEED

			fallVelocityHasSet = true;
			yVelocity.y = -5f;

		}

		inAirTimer += Time.deltaTime;
		owner.animator.SetFloat("InAirTimer", inAirTimer);

		yVelocity.y -= gravityForce * Time.deltaTime;

		owner.cc.Move(yVelocity * Time.deltaTime);
	}


	void HandleBlink() {

		// HANDLE MOVE DIRECTION PARAMETER DURING BLINK
		// AND RESET FLAGS WHEN BLINK IS END
		if (duringBlink) {
			
			// GET BLINK DIRECTION VECTOR BASED ON CHARACTER DIRECTION
			Vector3 localBlinkDirx = transform.InverseTransformDirection(blinkDirx);
			
			owner.animator.SetFloat("MoveHorizontal", localBlinkDirx.x);
			owner.animator.SetFloat("MoveVertical", localBlinkDirx.z);

			
			if (!owner.isPerformingAction) {
				duringBlink = false;
			}
			
		}
		
		
		// CHECK INPUT TO ATTEMPT BLINK
		if (PlayerInputManager.Instance.blinkInput) {
			PlayerInputManager.Instance.blinkInput = false;
			
		} else {
			return;
		}

		// CHECK FLAGS TO MAKE SURE ATTEMPT BLINK
		if (PlayerInputManager.Instance.movementInput.magnitude == 0) {
			return;
		}
		
		if (owner.CurStamina < owner.attribute.blinkCostStamina) {
			return;
		}
		if (!owner.isGrounded) {
			return;
		}
		if (!owner.canMove) {
			return;
		}
		if (owner.isPerformingAction) {
			return;
		}

		
		// ATTEMPT BLINK
		duringBlink = true;
		owner.CurStamina -= owner.attribute.blinkCostStamina;
		
		StartCoroutine(AttemptBlink());
	}

	IEnumerator AttemptBlink() {

		Vector3 camDirx = owner.camera.transform.forward;
		camDirx.y = 0;
		
		blinkDirx = Quaternion.LookRotation(camDirx) * new Vector3(PlayerInputManager.Instance.movementInput.x, 0, PlayerInputManager.Instance.movementInput.y);
		Vector3 targetPosition = transform.position + (blinkDirx * blinkDistance);
		
		NavMeshHit hitInfo;
		if (!NavMesh.SamplePosition(targetPosition, out hitInfo, 3f, NavMesh.AllAreas)) {
			// CAN'T FIND CLOSEST NAVMESH SURFACE(navmesh surface is available area to blink)
			yield break;
		}
		

		// CHECK DIRECTION OF MOVE HEIGHT
		Vector3 blinkAngle = Quaternion.LookRotation(targetPosition - transform.position).eulerAngles;
		if (blinkAngle.x < 180) {
			if (blinkAngle.x > 12.5)
				yield break; // blink canceled
		} else {
			if (blinkAngle.x > 347.5)
				yield break; // blink canceled
		}


		// CHECK OBSTACLE BETWEEN CURRENT CORE(player's main target option) POSITION AND NEW(after move) CORE POSITION 
		// FOR BEING PLAYER CAN'T PASS THROUGH THE WALL
		Vector3 playerCoreLocalPosition = owner.targetOptions[0].position - transform.position;
		if (Physics.Linecast(hitInfo.position + playerCoreLocalPosition, owner.targetOptions[0].position, PhysicLayerDataBase.Instance.environmentLayer)) {
			yield break; // blink canceled
		}

		
		// FINALLY ATTEMPT BLINK
		owner.animation.PlayTargetAction("Blink_Direction_Tree", true, true, false, false);
		
		yield return new WaitForSeconds(0.2f);
		
		GameObject vfx = Instantiate(VfxDataBase.Instance.blinkVfx);
		vfx.transform.position = owner.targetOptions[0].position;
		vfx.transform.forward = hitInfo.position - transform.position;
		
		owner.cc.Move(hitInfo.position - transform.position);
	}

	

	private void OnDrawGizmosSelected() {

		// Ground Check Sphere
		Gizmos.DrawSphere(transform.position, groundedCheckRadius);

		Gizmos.DrawRay(transform.position, transform.forward);
		Gizmos.DrawWireSphere(transform.position + transform.forward, 0.17f);


		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, moveDirx.normalized);
		Gizmos.DrawSphere(transform.position + moveDirx.normalized, 0.15f);

	}
}

}