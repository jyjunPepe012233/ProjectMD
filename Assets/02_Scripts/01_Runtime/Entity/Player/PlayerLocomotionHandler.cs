using System.Collections;
using MinD.Runtime.DataBase;
using MinD.Runtime.Managers;
using MinD.SO.StatusFX.Effects;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using PlayerInputManager = MinD.Runtime.Managers.PlayerInputManager;

namespace MinD.Runtime.Entity {

public class PlayerLocomotionHandler : BaseEntityHandler<Player> {

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
	public bool isJumping;
	
	
	private Vector3 yVelocity;
	private float inAirTimer;
	private bool fallVelocityHasSet;

	[HideInInspector] public Vector3 moveDirx; // move direction on world
	private Vector3 jumpDirx;

	private Vector3 blinkDirx; // blink direction on world

	private Coroutine blinkCoroutine;
	
	

	public void HandleAllLocomotion() {
		
		HandleGroundedCheck();
		HandleGravity();
		
		HandleRotation();

		HandleMovement();
		HandleSprint();
		HandleJump();

	}

	void HandleRotation() {

		if (!owner.canRotate)
			return;

		if (owner.isDeath) {
			return;
		}
		
		
		
		float rotationSpeedTemp = rotationSpeed;
		
		Vector3 camDirx = owner.camera.transform.forward;
		camDirx.y = 0;
		camDirx.Normalize();
		
		if (owner.isMoving) {

			// DEGREASE ROTATION SPEED WHEN PLAYER IS NOT GROUNDED
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

		if (owner.combat.usingDefenseMagic) {

			if (owner.isLockOn) {
				transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(camDirx), rotationSpeedTemp);
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

		owner.isMoving = inputDirx.magnitude != 0 && owner.canMove;
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
			
			
			moveDirx.Normalize();
		}
		
	}

	void HandleSprint() {

		isSprinting = owner.isMoving && PlayerInputManager.Instance.sprintInput;

	}

	void HandleGroundedCheck() {

		owner.isGrounded = Physics.CheckSphere(transform.position, groundedCheckRadius, WorldUtilityManager.environmentLayerMask);
		owner.animator.SetBool("IsGrounded", owner.isGrounded);

	}

	void HandleJump() {

		// JUMP MOVEMENT
		if (isJumping && !owner.isGrounded)
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

		isJumping = true;

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
		} else if (!fallVelocityHasSet && !isJumping) {
			// SET BASE FALL SPEED

			fallVelocityHasSet = true;
			yVelocity.y = -5f;

		}

		inAirTimer += Time.deltaTime;
		owner.animator.SetFloat("InAirTimer", inAirTimer);

		yVelocity.y -= gravityForce * Time.deltaTime;

		owner.cc.Move(yVelocity * Time.deltaTime);
	}

	
	
	

	// CALL BY PLAYER INPUT MANAGER
	public void AttemptBlink() { 
		
		// CHECK FLAGS TO MAKE SURE ATTEMPT BLINK
		if (owner.CurStamina < owner.attribute.blinkCostStamina) {
			return;
		}
		if (!owner.isMoving || isSprinting) {
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

		
		// GET BLINK DIRECTION
		Vector3 camDirx = owner.camera.transform.forward;
		camDirx.y = 0;
		
		blinkDirx = Quaternion.LookRotation(camDirx) * new Vector3(PlayerInputManager.Instance.movementInput.x, 0, PlayerInputManager.Instance.movementInput.y);
		Vector3 targetPosition = transform.position + (blinkDirx * blinkDistance);
		
		
		// CHECK DESTINATION BASED ON BLINK DIRECTION
		NavMeshHit hitInfo;
		if (!NavMesh.SamplePosition(targetPosition, out hitInfo, 3f, NavMesh.AllAreas)) {
			// CAN'T FIND CLOSEST NAVMESH SURFACE(navmesh surface is available area to blink)
			return;
		}
		

		// CHECK ANGLE TO DESTINATION
		Vector3 blinkAngle = Quaternion.LookRotation(targetPosition - transform.position).eulerAngles;
		if (blinkAngle.x < 180) {
			if (blinkAngle.x > 12.5)
				return;
		} else {
			if (blinkAngle.x > 347.5)
				return;
		}


		// CHECK OBSTACLE BETWEEN CURRENT CORE(player's main target option) POSITION AND NEW(after move) CORE POSITION 
		// FOR BEING PLAYER CAN'T PASS THROUGH THE WALL
		Vector3 playerCoreLocalPosition = owner.targetOptions[0].position - transform.position;
		if (Physics.Linecast(hitInfo.position + playerCoreLocalPosition, owner.targetOptions[0].position, WorldUtilityManager.environmentLayerMask)) {
			return;
		}
		
		blinkCoroutine = StartCoroutine(Blink(hitInfo.position - transform.position));
	}

	public void CancelBlink() {
		if (blinkCoroutine != null) {
			StopCoroutine(blinkCoroutine);
		}
	}
	
	IEnumerator Blink(Vector3 blinkPoint) {
		
		owner.animation.PlayTargetAction("Blink_Direction_Tree", true, true, false, false);
		
		// HANDLE MOVE DIRECTION PARAMETER IN ANIMATOR DURING BLINK
		Vector3 localBlinkDirx = transform.InverseTransformDirection(blinkPoint);
		owner.animator.SetFloat("MoveHorizontal", localBlinkDirx.x);
		owner.animator.SetFloat("MoveVertical", localBlinkDirx.z);
		
		
		// BEFORE BLINK DELAY
		yield return new WaitForSeconds(0.2f);
		
		
		owner.CurStamina -= owner.attribute.blinkCostStamina;
		
		// INSTANTIATE VFX AT OLD POSITION
		GameObject vfx = Instantiate(VfxDataBase.Instance.blinkVfx);
		vfx.transform.position = owner.targetOptions[0].position;
		vfx.transform.forward = blinkPoint.normalized;
		
		// MOVE AFTER INSTANTIATE VFX
		owner.cc.Move(blinkPoint);


		blinkCoroutine = null;
	}

	

	private void OnDrawGizmosSelected() {

		// Ground Check Sphere
		Gizmos.DrawSphere(transform.position, groundedCheckRadius);

	}
}

}