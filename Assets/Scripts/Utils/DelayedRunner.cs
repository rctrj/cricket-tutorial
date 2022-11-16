using System;
using System.Collections;
using UnityEngine;

namespace Utils
{
    public class DelayedRunner : MonoBehaviour
    {
        private static DelayedRunner _instance;

        public static DelayedRunner Instance
        {
            get
            {
                if (_instance != null) return _instance;
                var obj = new GameObject("DelayedRunnerInstance");
                var runner = obj.AddComponent<DelayedRunner>();
                _instance = runner;
                return _instance;
            }
            private set => _instance = value;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public Coroutine RunWithDelay(float time, Action ac) => StartCoroutine(RunWithDelayCoroutine(time, ac));

        private IEnumerator RunWithDelayCoroutine(float time, Action ac)
        {
            yield return new WaitForSeconds(time);
            ac.Invoke();
        }
    }
}