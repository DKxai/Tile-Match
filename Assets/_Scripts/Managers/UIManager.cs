using System;
using System.Collections.Generic;
using _Scripts.Data;
using _Scripts.Systems;
using _Scripts.UI.Settings;
using _Scripts.UI.Tools_Confirm;
using _Scripts.Utils.Event_Bus;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Managers
{
    public abstract class UIManager : Singleton<UIManager>
    {
        [Header("Shared")]
        [SerializeField] protected ConfirmPopup confirmPopup;
        [SerializeField] protected List<ConfirmPopupDataMapping> confirmPopupDataMappings;
        [SerializeField] protected SettingsPopup settingsPopup;
        [SerializeField] private Button settingsButton;

        protected override void Awake()
        {
            base.Awake();
            if (settingsButton != null)
                settingsButton.onClick.AddListener(OnSettingsClicked);
        }

        protected virtual void OnDestroy()
        {
            if (settingsButton != null)
                settingsButton.onClick.RemoveListener(OnSettingsClicked);
        }

        // ---- Lõi tái dùng được cho mọi confirm popup ----
        protected void ShowConfirmPopup(ConfirmPopupData data, Action onConfirm)
        {
            if (data == null)
            {
                Debug.LogError("[UIManager] ConfirmPopupData is null.");
                return;
            }

            confirmPopup.Setup(data.title, data.description, data.confirm,
                data.icon, onConfirm, data.isDisplayCoinUi, data.isDisplayHeartUI);
            confirmPopup.Show();
        }

        // Handler cho sự kiện hết tool / quit
        protected virtual void ConfirmPopupShow(OutOfToolUseEvent evt)
        {
            ToolType toolType = evt.ToolType;
            ConfirmPopupData data = GetConfirmPopupData(toolType);

            Action action = toolType == ToolType.Quit
                ? OnQuitConfirmed
                : () => OnBuyConfirmed(toolType, data.cost, data.amount);

            ShowConfirmPopup(data, action);
        }

        protected ConfirmPopupData GetConfirmPopupData(ToolType toolType) =>
            confirmPopupDataMappings.Find(x => x.type == toolType)?.data;

        protected void OnSettingsClicked() => settingsPopup.Show();

        private void OnBuyConfirmed(ToolType toolType, int cost, int amount)
        {
            if (!CurrencyManager.Instance.HasEnoughCoins(cost))
            {
                EventBus.Publish(new NotEnoughCurrencyEvent());
                return;
            }
            EventBus.Publish(new PurchaseEvent(toolType, amount, cost));
        }

        private void OnQuitConfirmed()
        {
            HeartManager.Instance.SpendHeart();
            SceneLevelManager.Instance.LoadScene("LevelMap");
            Debug.Log("Quit");
        }
    }
}