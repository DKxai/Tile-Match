using System.Collections;
using _Scripts.Data;
using _Scripts.Systems;
using _Scripts.UI;
using _Scripts.UI.Tools_Confirm;
using _Scripts.Utils.Event_Bus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        private ConfirmPopupData addHeartPopupData; // data riêng cho popup thêm tim

        [SerializeField] private ConfirmPopupData adsPopupData;

        [Header("References")] [SerializeField]
        private RectTransform content;

        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private LevelNode[] nodes;

        private const string PreSceneName = "Level";
        private int CurrentLevel => PlayerPrefs.GetInt("CurrentLevel", 1);

        protected override void Awake()
        {
            base.Awake();
            RegisterEvents();
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

            currentLevelBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"Level {CurrentLevel}";
        }

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
            ScrollToNode(nodes[CurrentLevel - 1].GetComponent<RectTransform>(), 1f);
            StartCoroutine(LoadScene(PreSceneName + CurrentLevel));
        }

        private IEnumerator LoadScene(string sceneName)
        {
            yield return new WaitForSeconds(1f);
            SceneLevelManager.Instance.LoadScene(sceneName); // dùng tham số truyền vào
        }

        #region Scrolling

        private void ScrollToNode(RectTransform nodeRect, float duration) =>
            StartCoroutine(ScrollToMiddle(nodeRect, duration));

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

        private void OpenStorePopup() => StoreManager.Instance.ShowStore();

        private void OpenHeartPopUp() =>
            ShowConfirmPopup(addHeartPopupData, OnAddHeartConfirmed);

        private void OpenHeartPopUp(OnAddHeartButtonClickedEvent evt) => OpenHeartPopUp();

        private void OnAddHeartConfirmed()
        {
            // TODO: chọn 1 trong các hành vi tùy game design của bạn:
            // 1) Xem quảng cáo thưởng rồi cộng tim (gọi AddHeart trong callback thành công)
            // 2) Trừ coin: if (CurrencyManager.Instance.HasEnoughCoins(cost)) { ...; HeartManager.Instance.AddHeart(); }
            // 3) Cộng thẳng:
            HeartManager.Instance.AddHeart();
            EventBus.Publish(new PurchaseEvent(ToolType.Heart,addHeartPopupData.amount,addHeartPopupData.cost));
        }

        private void OpenAdsUI()
        {
            ShowConfirmPopup(adsPopupData,OnAdsBuyConfirm);
        }

        private void OnAdsBuyConfirm()
        {
            Debug.Log("No more Ads");
            EventBus.Publish(new PurchaseEvent(ToolType.Ads,adsPopupData.amount,adsPopupData.cost));
            adsBtn.gameObject.SetActive(false);
        }

        #endregion
    }
}