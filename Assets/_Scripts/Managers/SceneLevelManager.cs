using System.Collections;
using _Scripts.Data;
using _Scripts.Utils.Event_Bus;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Scripts.Managers
{
    public class SceneLevelManager : PersistentSingleton<SceneLevelManager>
    {
        [SerializeField] private CanvasGroup loaderCanvas;
        [SerializeField] private Scrollbar loadingBar;
        private float _target;

        private void OnEnable()
        {
            EventBus.Subscribe<LoadSceneEvent>(OnLoadScene);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<LoadSceneEvent>(OnLoadScene);
            
        }

        private void OnLoadScene(LoadSceneEvent evt)
        {
            LoadScene(evt.SceneType);
        }
        public void LoadScene(SceneType sceneType)
        {
            StartCoroutine(Load(sceneType.ToString()));
        }

        IEnumerator Load(string sceneName)
        {
            loadingBar.value = 0f;
            loaderCanvas.gameObject.SetActive(true);

            yield return Fade(0f, 1f);

            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

            op.allowSceneActivation = false;
            do
            {
                loadingBar.value = Mathf.Lerp(loadingBar.value, op.progress, Time.deltaTime * 8);
                yield return null;
            } while (op.progress < 0.9f);

            loadingBar.value = 1f;

            yield return new WaitForSeconds(0.3f);
            op.allowSceneActivation = true;

            while (!op.isDone) yield return null;
            
            yield return Fade(1f, 0f);
            
            loaderCanvas.gameObject.SetActive(false);
        }

        private IEnumerator Fade(float f, float f1)
        {
            float time = 0f;
            while (time < 0.25f)
            {
                time += Time.deltaTime;
                loaderCanvas.alpha = Mathf.Lerp(f, f1, time / 0.25f);
                yield return null;
            }
            loaderCanvas.alpha = f1;
            
            
        }
    }
}