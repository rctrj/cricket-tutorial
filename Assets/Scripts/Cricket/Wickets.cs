using System.Collections.Generic;
using UnityEngine;

namespace Cricket
{
    public class Wickets : MonoBehaviour
    {
        [SerializeField] private List<GameObject> wicketItems;

        private List<(Vector3, Quaternion)> _initStates;

        private void Awake()
        {
            _initStates = new List<(Vector3, Quaternion)>();
            foreach (var wicketItem in wicketItems)
                _initStates.Add((wicketItem.transform.position, wicketItem.transform.rotation));
        }

        public void Reset()
        {
            for (var i = 0; i < wicketItems.Count; i++)
            {
                wicketItems[i].transform.position = _initStates[i].Item1;
                wicketItems[i].transform.rotation = _initStates[i].Item2;

                var rb = wicketItems[i].GetComponent<Rigidbody>();
                if (!rb) continue;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}