using UnityEngine;
using UnityEngine.EventSystems;

namespace Utils
{
    public class CanvasDragHandler : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;

        public void OnDrag(BaseEventData data)
        {
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