using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Scene
{
    public class DelayedSceneLoader : MonoBehaviour
    {
        [SerializeField] private float loadDelay;
        [SerializeField] private string sceneName;

        private void Awake() => DelayedRunner.Instance.RunWithDelay(loadDelay, () => SceneManager.LoadScene(sceneName));
    }
}