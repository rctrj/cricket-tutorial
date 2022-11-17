using System.Collections;
using Cricket.Balls;
using UnityEngine;

namespace Cricket.Behaviour
{
    public class Bowler : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private BallThrower ballThrower;

        private const string IdleStateName = "Idle";
        private const string BowlingStateName = "BowlingAnim";

        public void WaitForRunUp() => animator.Play(IdleStateName);

        public void Bowl(Vector3 target, float power)
        {
            animator.Play(BowlingStateName);
            StartCoroutine(ThrowBallCoroutine(target, power));
        }

        private IEnumerator ThrowBallCoroutine(Vector3 target, float power)
        {
            // state is updated in the animator when the frame is completed.
            // This is why I'm waiting for the end of the frame,
            // otherwise IsBowlingAnimationPlaying returns false for the first frame
            yield return new WaitForEndOfFrame();
            yield return new WaitWhile(IsBowlingAnimationPlaying);
            ballThrower.ThrowBall(target, power);
        }

        private bool IsBowlingAnimationPlaying()
        {
            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (!stateInfo.IsName(BowlingStateName)) return false;
            return stateInfo.length > stateInfo.normalizedTime;
        }

        #region debug

        // TODO: Remove
        [SerializeField] private float debugPower;
        [SerializeField] private Vector3 debugTarget;

        public void DebugThrowBall() => Bowl(debugTarget, debugPower);

        #endregion
    }
}