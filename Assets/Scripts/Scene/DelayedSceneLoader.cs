using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Scene
{
    public class DelayedSceneLoader : MonoBehaviour
    {
        [SerializeField] private float loadDelay;
        [SerializeField] private string sceneName;

        [SerializeField] private bool runOnAwake;

        private void Awake()
        {
            if (runOnAwake) LoadNextScene();
        }

        public void LoadNextScene() =>
            DelayedRunner.Instance.RunWithDelay(loadDelay, () => SceneManager.LoadScene(sceneName));
    }
}