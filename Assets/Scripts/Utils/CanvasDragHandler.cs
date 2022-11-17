using UnityEngine;
using UnityEngine.EventSystems;

namespace Utils
{
    public class CanvasDragHandler : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private bool isEnabled = true;

        public bool IsEnabled
        {
            get => isEnabled;
            set => isEnabled = value;
        }

        public void OnDrag(BaseEventData data)
        {
            if (!isEnabled) return;

            var pointerData = (PointerEventData)data;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)canvas.transform,
                pointerData.position,
                canvas.worldCamera,
                out var position
            );
            transform.position = canvas.transform.TransformPoint(position);
        }
    }
}