using Cricket.Balls;
using UnityEngine;

namespace Cricket.Behaviour
{
    public class AIBowler : Bowler
    {
        [SerializeField] private Vector2 xBounds;
        [SerializeField] private Vector2 zBounds;

        [SerializeField] private float yPointToBowl = 0.2f;
        [SerializeField] private float power = 20;

        public Ball Bowl()
        {
            var x = Random.Range(xBounds.x, xBounds.y);
            var z = Random.Range(zBounds.x, zBounds.y);

            var target = new Vector3(x, yPointToBowl, z);
            return Bowl(target, power);
        }
    }
}