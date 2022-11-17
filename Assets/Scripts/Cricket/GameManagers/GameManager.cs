using Scene;
using TMPro;
using UnityEngine;

namespace Cricket.GameManagers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text toast;
        [SerializeField] protected DelayedSceneLoader sceneLoader;

        protected void ShowToast(string message)
        {
            toast.gameObject.SetActive(false);
            toast.text = message;
            toast.gameObject.SetActive(true);
        }
    }
}