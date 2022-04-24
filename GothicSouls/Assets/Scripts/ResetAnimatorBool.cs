using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class ResetAnimatorBool : StateMachineBehaviour
    {
        public string isInteractingBool = "isInteracting";
        public bool isInteractingStatus = false;

        public string isFiringSpellBool = "isFiringSpell";
        public bool isFiringSpellStatus = false;

        public string isRotatingWithRootMotion = "isRotatingWithRootMotion";
        public bool isrotatingWithRootMotionStatus = false;

        public string canRotate = "canRotate";
        public bool canRotateStatus = true;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(isInteractingBool, isInteractingStatus);
            animator.SetBool(isFiringSpellBool, isFiringSpellStatus);
            animator.SetBool(isRotatingWithRootMotion, isrotatingWithRootMotionStatus);
            animator.SetBool(canRotate, canRotateStatus);
        }
    }
}
