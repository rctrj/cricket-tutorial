using Cricket.GameManagers;
using UnityEngine;

namespace Cricket.AIGame
{
    public class WicketsAIGame : Wickets
    {
        [SerializeField] private LayerMask ballLayerMask;
        [SerializeField] private GameManagerAIGame gameManager;

        private void OnTriggerEnter(Collider collision)
        {
            var collisionObjectLayerMask = 1 << collision.gameObject.layer;
            if ((ballLayerMask & collisionObjectLayerMask) == 0) return;
            gameManager.Out();
        }
    }
}