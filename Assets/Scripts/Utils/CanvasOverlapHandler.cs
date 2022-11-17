using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    public class CanvasOverlapHandler : MonoBehaviour
    {
        [SerializeField] private RectTransform destination;
        [SerializeField] private RectTransform source;
        [SerializeField] private UnityEvent onComplete;

        private bool _isRunning = true;

        public void Reset() => _isRunning = true;

        private void Update()
        {
            if (!_isRunning) return;

            var destinationPos = destination.anchoredPosition;
            var sourcePos = source.anchoredPosition;

            var distance = Vector2.Distance(destinationPos, sourcePos);
            if (distance >= 1 / 2f) return;
            _isRunning = false;
            onComplete.Invoke();
        }
    }
}