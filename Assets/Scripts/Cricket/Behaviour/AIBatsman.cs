using UnityEngine;
using Utils;

namespace Cricket.Behaviour
{
    public class AIBatsman : Batsman
    {
        [SerializeField] private float minTime;
        [SerializeField] private float maxTime;

        public override void Swing()
        {
            var delay = Random.Range(minTime, maxTime);
            DelayedRunner.Instance.RunWithDelay(delay, base.Swing);
        }
    }
}