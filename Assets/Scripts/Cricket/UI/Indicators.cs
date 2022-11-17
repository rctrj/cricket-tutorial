using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cricket.UI
{
    public class Indicators : MonoBehaviour
    {
        [SerializeField] private List<Button> allIndicators;

        public int TotalIndicators => allIndicators.Count;

        private void Awake() => Reset();

        public void Reset()
        {
            foreach (var indicator in allIndicators) indicator.interactable = false;
        }

        public void SetActiveCount(int count)
        {
            Reset();
            for (var i = 0; i < count; i++) allIndicators[i].interactable = true;
        }
    }
}