using UnityEngine;

namespace Cricket.UI
{
    public class Toast : MonoBehaviour
    {
        [SerializeField] private string entryAnimName = "ToastEntryAnimation";
        [SerializeField] private Animator animator;

        private void OnEnable() => animator.Play(entryAnimName);
    }
}