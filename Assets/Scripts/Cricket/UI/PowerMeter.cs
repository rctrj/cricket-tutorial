using UnityEngine;

namespace Cricket.UI
{
    public class PowerMeter : MonoBehaviour
    {
        [SerializeField] private RectTransform powerMeter;
        [SerializeField] private RectTransform safeArea;
        [SerializeField] private RectTransform movingPart;

        [SerializeField] private float frequency;

        private float _minSafeY;
        private float _maxSafeY;
        private float _meterHeight;

        private Vector2 _movingPartInitialPoint;

        private bool _isRunning = true;
        private float _elapsed;

        private void Awake()
        {
            var safeAreaRect = safeArea.rect;
            _minSafeY = safeAreaRect.yMin;
            _maxSafeY = safeAreaRect.yMax;

            _movingPartInitialPoint = movingPart.anchoredPosition;
            _meterHeight = powerMeter.rect.height;
        }

        private void Update()
        {
            if (!_isRunning) return;
            _elapsed += Time.deltaTime;

            movingPart.anchoredPosition = new Vector2(0f, Mathf.Sin(_elapsed * frequency)) * _meterHeight / 2f;
        }

        public void Reset()
        {
            _elapsed = 0;
            _isRunning = true;
            movingPart.anchoredPosition = _movingPartInitialPoint;
        }

        public void Stop() => _isRunning = false;

        public bool IsPowerShot()
        {
            var pos = movingPart.anchoredPosition.y;
            return pos >= _minSafeY && pos <= _maxSafeY;
        }
    }
}