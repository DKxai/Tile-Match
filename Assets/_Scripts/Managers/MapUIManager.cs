using System.Collections;
using _Scripts.Data;
using _Scripts.Data.Tool;
using _Scripts.SaveSystem;
using _Scripts.Systems;
using _Scripts.UI;
using _Scripts.UI.Tools_Confirm;
using _Scripts.Utils.Event_Bus;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using EventBus = _Scripts.Utils.Event_Bus.EventBus;

namespace _Scripts.Managers
{
    public class MapUIManager : UIManager
    {
        [Header("UI Buttons")] [SerializeField]
        private Button currentLevelBtn;

        [SerializeField] private Button coinsBtn;
        [SerializeField] private Button storeBtn;
        [SerializeField] private Button heartBtn;
        [SerializeField] private Button adsBtn;

        [Header("Heart Popup")] [SerializeField]
        private ConfirmPopupData addHeartPopupData;

        [SerializeField] private ConfirmPopupData adsPopupData;

        [Header("References")] [SerializeField]
        private RectTransform content;

        [SerializeField] private GameObject nodeParent;
        [SerializeField] private ScrollRect scrollRect;

        [Header("Entry Animation")] [SerializeField]
        private UIEntryConfig[] entryConfigs;

        [SerializeField] private float duration = 0.5f;
        [SerializeField] private Ease ease = Ease.OutBack;
        [SerializeField] private float offsetDistance = 300f;
        [SerializeField] private float delay = 0.35f;
        private Vector2[] _targetPositions;
        private LevelNode[] nodes;
        private int CurrentLevel => DataSystem.LoadCurrentLevel();

        protected override void Awake()
        {
            base.Awake();
            RegisterEvents();
            PreAnimations();
            nodes = nodeParent.GetComponentsInChildren<LevelNode>();
            ScrollToNode(nodes[CurrentLevel - 1].GetComponent<RectTransform>(), 1f);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            UnregisterEvents();
        }

        private void OnEnable()
        {
            EventBus.Subscribe<OnAddHeartButtonClickedEvent>(OpenHeartPopUp);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<OnAddHeartButtonClickedEvent>(OpenHeartPopUp);
        }

        private void Start()
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                int levelIndex = i + 1;
                if (levelIndex < CurrentLevel)
                    nodes[i].SetState(LevelNode.State.Complete);
                else if (levelIndex == CurrentLevel)
                    nodes[i].SetState(LevelNode.State.Current);
                else
                    nodes[i].SetState(LevelNode.State.Locked);

                nodes[i].Setup(levelIndex);
            }

            adsBtn.gameObject.SetActive(DataSystem.LoadAds());
            currentLevelBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"Level {CurrentLevel}";
            PlayEntryAnimations();
        }

        #region EntryAnimation

        private void PreAnimations()
        {
            _targetPositions = new Vector2[entryConfigs.Length];
            for (int i = 0; i < entryConfigs.Length; i++)
            {
                var config = entryConfigs[i];
                if (config.target == null) continue; 

                _targetPositions[i] = config.target.anchoredPosition;
                config.target.anchoredPosition += config.direction * offsetDistance;
            }
        }

        private void PlayEntryAnimations()
        {
            for (int i = 0; i < entryConfigs.Length; i++)
            {
                var config = entryConfigs[i];
                if (config.target == null) continue; 

                config.target.DOAnchorPos(_targetPositions[i], duration)
                    .SetEase(ease)
                    .SetDelay(i * delay);
            }
        }

        #endregion

        #region Register & unregister events

        private void RegisterEvents()
        {
            currentLevelBtn.onClick.AddListener(CurrentLevelClicked);
            coinsBtn.onClick.AddListener(OpenStorePopup);
            storeBtn.onClick.AddListener(OpenStorePopup);
            heartBtn.onClick.AddListener(OpenHeartPopUp);
            adsBtn.onClick.AddListener(OpenAdsUI);
        }

        private void UnregisterEvents()
        {
            currentLevelBtn.onClick.RemoveListener(CurrentLevelClicked);
            coinsBtn.onClick.RemoveListener(OpenStorePopup);
            storeBtn.onClick.RemoveListener(OpenStorePopup);
            heartBtn.onClick.RemoveListener(OpenHeartPopUp);
            adsBtn.onClick.RemoveListener(OpenAdsUI);
        }

        #endregion

        private void CurrentLevelClicked()
        {
            if (DataSystem.LoadHearts() == 0)
            {
                EventBus.Publish(new OnAddHeartButtonClickedEvent(ToolType.Heart));
                return;
            }

            DataSystem.SaveSelectedLevel(CurrentLevel);

            ScrollToNode(nodes[CurrentLevel - 1].GetComponent<RectTransform>(), 1f);
            StartCoroutine(LoadScene());
        }

        private IEnumerator LoadScene()
        {
            yield return new WaitForSeconds(1.5f);
            HeartManager.Instance.SpendHeart();
            EventBus.Publish(new LoadSceneEvent(SceneType.GameScene));
        }

        #region Scrolling

        private void ScrollToNode(RectTransform nodeRect, float duration) =>
            StartCoroutine(ScrollToMiddle(nodeRect, duration));

        private IEnumerator ScrollToMiddle(RectTransform nodeRect, float duration)
        {
            Canvas.ForceUpdateCanvases();

            RectTransform viewport = scrollRect.viewport != null
                ? scrollRect.viewport
                : (RectTransform)scrollRect.transform;

            float viewportHeight = viewport.rect.height;
            float contentHeight  = content.rect.height;
            float scrollable     = contentHeight - viewportHeight;

            if (scrollable <= 0f) yield break;

            Vector3 worldCenter = nodeRect.TransformPoint(nodeRect.rect.center);
            Vector3 localCenter = content.InverseTransformPoint(worldCenter);

            float distFromTop = content.rect.yMax - localCenter.y;

            float offset = distFromTop - viewportHeight * 0.5f;

            float targetNorm = 1f - Mathf.Clamp01(offset / scrollable);

            float start = scrollRect.verticalNormalizedPosition;
            float elapse = 0f;

            while (elapse < duration)
            {
                elapse += Time.deltaTime;
                float t = elapse / duration;
                t = t * t * (3f - 2f * t); 
                scrollRect.verticalNormalizedPosition = Mathf.Lerp(start, targetNorm, t);
                yield return null;
            }

            scrollRect.verticalNormalizedPosition = targetNorm; 
        }

        #endregion

        #region Opening UI

        private void OpenStorePopup() => StoreManager.Instance.ShowStore();

        private void OpenHeartPopUp(OnAddHeartButtonClickedEvent evt) => OpenHeartPopUp();

        private void OpenHeartPopUp() =>
            ShowConfirmPopup(addHeartPopupData, OnAddHeartConfirmed);


        private void OnAddHeartConfirmed()
        {
            HeartManager.Instance.AddHeart();
            EventBus.Publish(new PurchaseEvent(ToolType.Heart, addHeartPopupData.amount, addHeartPopupData.cost));
            if (DataSystem.LoadHearts() == 5)
            {
                confirmPopup.PlayHideAnimation();
            }
        }

        private void OpenAdsUI()
        {
            ShowConfirmPopup(adsPopupData, OnAdsBuyConfirm);
        }

        private void OnAdsBuyConfirm()
        {
            Debug.Log("No more Ads");
            EventBus.Publish(new PurchaseEvent(ToolType.Ads, adsPopupData.amount, adsPopupData.cost));
            DataSystem.SaveAds(false);
            adsBtn.gameObject.SetActive(DataSystem.LoadAds());
        }

        #endregion
    }
}