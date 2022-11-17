using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class ButtonSpamPrevention : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private float minDelay;

        public void OnClick()
        {
            button.interactable = false;
            DelayedRunner.Instance.RunWithDelay(minDelay, () => button.interactable = true);
        }
    }
}