using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using _Scripts.Data;
using _Scripts.Systems;
using _Scripts.UI;
using _Scripts.UI.Settings;
using _Scripts.UI.Store;
using _Scripts.UI.Tools_Confirm;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace _Scripts.Managers
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("UI Buttons")] [SerializeField]
        private Button currentLevelBtn;

        [SerializeField] private Button settingsBtn;
        [SerializeField] private Button coinsBtn;
        [SerializeField] private Button storeBtn;
        [SerializeField] private Button heartBtn;
        [SerializeField] private Button adsBtn;


        [SerializeField] private LevelNode[] nodes;

        [SerializeField] private RectTransform content;
        [SerializeField] private ScrollRect scrollRect;
        private readonly string _preSceneName = "Level";
        private int CurrentLevel => PlayerPrefs.GetInt("CurrentLevel", 1);

        [SerializeField] private SettingsPopup settingsPopup;
        [SerializeField] private StorePopup storePopup;

        [SerializeField] private ConfirmPopup confirmPopup;


        // [SerializeField] private HeartUI heartUI;
        // [SerializeField] private AdsUI adsUI;
        // [SerializeField] private WinUi winUI;
        protected override void Awake()
        {
            base.Awake();
            RegisterEvents();
        }

        private void OnDestroy()
        {
            UnregisterEvents();
        }

        private void Start()
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                int levelIndex = i + 1;

                if (levelIndex < CurrentLevel)
                {
                    nodes[i].SetState(LevelNode.State.Complete);
                }
                else if (levelIndex == CurrentLevel)
                {
                    nodes[i].SetState(LevelNode.State.Current);
                }
                else
                {
                    nodes[i].SetState(LevelNode.State.Locked);
                }

                nodes[i].Setup(levelIndex);
            }


            currentLevelBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"Level {CurrentLevel}";
        }

        #region Register & unregister  events

        private void RegisterEvents()
        {
            currentLevelBtn.onClick.AddListener(CurrentLevelClicked);
            settingsBtn.onClick.AddListener(OpenSettingsUI);
            coinsBtn.onClick.AddListener(OpenStorePopup);
            storeBtn.onClick.AddListener(OpenStorePopup);
            // adsBtn.onClick.AddListener(OpenAdsUI);
            // quitBtn.onClick.AddListener(CurrentLevelClicked);
        }

        private void UnregisterEvents()
        {
            currentLevelBtn.onClick.RemoveListener(CurrentLevelClicked);
            settingsBtn.onClick.RemoveListener(OpenSettingsUI);
            coinsBtn.onClick.RemoveListener(OpenStorePopup);
            storeBtn.onClick.RemoveListener(OpenStorePopup);
            // adsBtn.onClick.RemoveListener(OpenAdsUI);
            //quitBtn.onClick.RemoveListener(CurrentLevelClicked);
        }

        #endregion

        private void CurrentLevelClicked()
        {
            ScrollToNode(nodes[CurrentLevel - 1].GetComponent<RectTransform>(), 1f);
            StartCoroutine(LoadScene(_preSceneName + CurrentLevel));
        }

        private IEnumerator LoadScene(string sceneName)
        {
            yield return new WaitForSeconds(1f);
            SceneLevelManager.Instance.LoadScene(_preSceneName + CurrentLevel);
        }


        #region Scrolling

        private void ScrollToNode(RectTransform nodeRect, float duration)
        {
            StartCoroutine(ScrollToMiddle(nodeRect, duration));
        }

        private IEnumerator ScrollToMiddle(RectTransform nodeRect, float duration)
        {
            float contentHeight = content.rect.height;
            float nodeLocalY = nodeRect.anchoredPosition.y;

            float target = Mathf.Clamp01(nodeLocalY / contentHeight);
            float start = scrollRect.verticalNormalizedPosition;
            float elapse = 0f;

            while (elapse < duration)
            {
                elapse += Time.deltaTime;
                float t = elapse / duration;
                t = t * t * (3f - 2f * t);
                scrollRect.verticalNormalizedPosition = Mathf.Lerp(start, target, t);
                yield return null;
            }
        }

        #endregion

        #region Opening UI

        private void Open(UIPopup popup)
        {
            popup.Show();
        }

        public void OpenSettingsUI() => Open(settingsPopup);

        public void OpenStorePopup() => Open(storePopup);

        // public void OpenHeartUI() => Open(heartUI);
        // public void OpenQuitUI() => Open(quitUI);
        // public void OpenWinUI() => Open(winUI);
        // public void OpenToolsUI() => Open(toolsUI);
        // public void OpenAdsUI() => Open(adsUI);

        #endregion

        public void Close(UIPopup popup)
        {
            popup.Hide();
        }

    }
}