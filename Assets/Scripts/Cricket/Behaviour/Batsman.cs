using System;
using System.Collections;
using Cricket.Balls;
using UnityEngine;

namespace Cricket.Behaviour
{
    public class Batsman : MonoBehaviour
    {
        private const string SwingAnimName = "BattingAnim";

        [SerializeField] private float maxAllowedHitDistance;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform batTransform;
        [SerializeField] private Vector2 allowedAnglesToHit;
        [SerializeField] private float hitPower = 600;

        public Ball CurrentBall { get; set; }

        public Action OnBallHit;

        public void Swing() => StartCoroutine(SwingBatCoroutine());

        private IEnumerator SwingBatCoroutine()
        {
            animator.Play(SwingAnimName);

            //for animation
            yield return new WaitForSeconds(0.1f);
            if (!CurrentBall) yield break;

            var currentLocation = batTransform.position;
            currentLocation.y = 0; //doing this to raise the ball on hit
            var ballLocation = CurrentBall.transform.position;

            var distance = Vector3.Distance(currentLocation, ballLocation);
            if (distance > maxAllowedHitDistance) yield break;

            var dir = ballLocation - currentLocation;
            dir = dir.normalized;

            var angle = Vector3.Angle(dir, Vector3.forward);
            if (angle < allowedAnglesToHit.x || angle > allowedAnglesToHit.y) yield break;

            Debug.Log("Hitting Ball");
            var power = dir * hitPower;
            CurrentBall.Rigidbody.velocity = Vector3.one;
            CurrentBall.Rigidbody.AddForce(power);
            CurrentBall.Hit();

            OnBallHit?.Invoke();
        }
    }
}