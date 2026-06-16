using System;
using System.Collections;
using _Scripts.Data;
using _Scripts.SaveSystem;
using _Scripts.Utils.Event_Bus;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Scripts.Managers
{
    public class SceneLevelManager : PersistentSingleton<SceneLevelManager>
    {
        [SerializeField] private CanvasGroup loaderCanvas;
        [SerializeField] private Image loadingBar;
        private float _target;

        private void OnEnable()
        {
            EventBus.Subscribe<LoadSceneEvent>(OnLoadScene);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<LoadSceneEvent>(OnLoadScene);
            
        }

        private void Start()
        {
            if (!DataSystem.IsTutorialDone)
            {
                LoadScene(SceneType.TutorialScene);
            }
            else
            {
                LoadScene(SceneType.MapScene);
            }
        }

        private void OnLoadScene(LoadSceneEvent evt)
        {
            LoadScene(evt.SceneType);
        }
        public void LoadScene(SceneType sceneType)
        {
            StartCoroutine(Load(sceneType.ToString()));
        }

        private IEnumerator Load(string sceneName)
        {
            loadingBar.fillAmount = 0f;
            loaderCanvas.gameObject.SetActive(true);

            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            op.allowSceneActivation = false;

            while (op.progress < 0.9f)
            {
                loadingBar.fillAmount = op.progress / 0.9f;
                yield return null;
            }

            while (loadingBar.fillAmount < 1f)
            {
                loadingBar.fillAmount = Mathf.MoveTowards(
                    loadingBar.fillAmount,
                    1f,
                    Time.deltaTime * 2f);

                yield return null;
            }

            yield return new WaitForSeconds(0.2f);

            op.allowSceneActivation = true;

            while (!op.isDone)
                yield return null;

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