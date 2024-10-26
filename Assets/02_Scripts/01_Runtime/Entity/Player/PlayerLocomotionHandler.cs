using MinD.Runtime.Managers;
using UnityEngine;

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
	
	[Header("[ Flags ]")]
	public bool isSprinting;

	
	private int groundLayerMask;
	private Vector3 yVelocity;
	private float inAirTimer;
	private bool fallVelocityHasSet;

	private Vector3 moveDirx;
	private Vector3 jumpDirx;


	void OnEnable() {
		groundLayerMask = LayerMask.GetMask("Default");
	}

	public void HandleAllLocomotion() {

		HandleRotation();

		HandleMovement();
		HandleSprint();

		HandleGroundedCheck();
		HandleJump();
		HandleGravity();

		HandleBlink();

	}

	void HandleRotation() {

		if (!owner.canRotate)
			return;


		if (owner.isMoving) {

			Vector3 camDirx = owner.camera.transform.forward;
			camDirx.y = 0;
			camDirx.Normalize();


			// DEGREASE ROTATION SPEED WHEN PLAYER IS NOT GROUNDED
			float rotationSpeedTemp = rotationSpeed;
			if (!owner.isGrounded)
				rotationSpeedTemp *= 0.13f;


			if (isSprinting)
				transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveDirx), rotationSpeedTemp);
			else
				transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(camDirx), rotationSpeedTemp);
		}

	}


	void HandleMovement() {

		moveDirx = owner.camera.transform.forward;
		moveDirx.y = 0;
		moveDirx.Normalize();


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


		#region Update_Animation_BlendTree

		if (isSprinting && owner.isMoving)
			owner.animation.LerpMovementBlendTree(0, inputDirx.magnitude * 2);
		else
			owner.animation.LerpMovementBlendTree(inputDirx.x, inputDirx.z);

		#endregion
	}

	void HandleSprint() {
		isSprinting = PlayerInputManager.Instance.sprintInput;
	}


	void HandleGroundedCheck() {

		owner.isGrounded = Physics.CheckSphere(transform.position, groundedCheckRadius, groundLayerMask);
		owner.animator.SetBool("IsGrounded", owner.isGrounded);

	}

	void HandleJump() {

		// JUMP MOVEMENT
		if (owner.isJumping && !owner.isGrounded)
			owner.cc.Move(jumpDirx * Time.deltaTime);


		// ATTEMPT JUMP

		if (PlayerInputManager.Instance.jumpInput == false)
			return;

		PlayerInputManager.Instance.jumpInput = false;

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

		// TODO: move character via root motion in dodge animation

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