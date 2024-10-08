using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerFlags : StateMachineBehaviour {

    [Header("[ Flags Set Value ]")]
    [SerializeField] private bool isPerformingAction = false;
    [SerializeField] private bool applyRootMotion = false;
    [SerializeField] private bool canRotate = true;
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool isJumping = false;

    [SerializeField] private bool setIsPerformingAction, setApplyRootMotion, setCanRotate, setCanMove, setIsJumping;
    
    
    
    public Player owner;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        if (owner == null)
            owner = animator.GetComponent<Player>();

        animator.applyRootMotion = applyRootMotion;
		
        owner.isPerformingAction = isPerformingAction;
        owner.isJumping = isJumping;
        owner.canRotate = canRotate;
        owner.canMove = canMove;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}