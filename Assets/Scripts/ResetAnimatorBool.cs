using UnityEngine;

namespace PG
{
    public class ResetAnimatorBool : StateMachineBehaviour
    {
        [SerializeField] private bool grenade = false;
        [SerializeField] private bool shoot = false;
        [SerializeField] private bool moving = false;
        [SerializeField] private float delay = 2f;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (grenade)
            {
                animator.SetBool("AimGrenade", false);
            }
            if (shoot)
            {
                RoundManager.Instance.unitTakingTurn_UnitController.UnitActions.BulletTrail.SetActive(false);
            }
            if (grenade || shoot || moving)
            {
                RoundManager.Instance.Invoke(nameof(RoundManager.Instance.TransitionCameraToMain), delay);
                RoundManager.Instance.StartCoroutine(RoundManager.Instance.unitTakingTurn_UnitController.UnitActions.UnlockInput(delay));
            }
        }

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
}
