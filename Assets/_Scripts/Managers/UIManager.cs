using System;
using System.Collections.Generic;
using _Scripts.Data;
using _Scripts.Data.Tool;
using _Scripts.Systems;
using _Scripts.UI.Settings;
using _Scripts.UI.Tools_Confirm;
using _Scripts.Utils.Event_Bus;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Managers
{
    public abstract class UIManager : MonoBehaviour
    {
        [Header("Shared")]
        [SerializeField] protected ConfirmPopup confirmPopup;
        [SerializeField] protected List<ConfirmPopupDataMapping> confirmPopupDataMappings;
        [SerializeField] protected SettingsPopup settingsPopup;
        [SerializeField] private Button settingsButton;

        protected  virtual void Awake()
        {
            if (settingsButton != null)
                settingsButton.onClick.AddListener(OnSettingsClicked);
        }

        protected virtual void OnDestroy()
        {
            if (settingsButton != null)
                settingsButton.onClick.RemoveListener(OnSettingsClicked);
        }

        protected void ShowConfirmPopup(ConfirmPopupData data, Action onConfirm)
        {
            if (data == null)
            {
                Debug.LogError("[UIManager] ConfirmPopupData is null.");
                return;
            }

            confirmPopup.Setup(data.title, data.description,data.amount, data.confirm,
                data.icon, onConfirm, data.isDisplayCoinUi, data.isDisplayHeartUI);
            confirmPopup.Show();
        }

        protected virtual void ConfirmPopupShow(OutOfToolUseEvent evt)
        {
            ToolType toolType = evt.ToolType;
            ConfirmPopupData data = GetConfirmPopupData(toolType);

            Action action = toolType == ToolType.Quit
                ? OnQuitConfirmed
                : () => OnBuyConfirmed(toolType, data.cost, data.amount);

            ShowConfirmPopup(data, action);
        }

        private ConfirmPopupData GetConfirmPopupData(ToolType toolType) =>
            confirmPopupDataMappings.Find(x => x.type == toolType)?.data;

        private void OnSettingsClicked() => settingsPopup.Show();

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
            EventBus.Publish(new LoadSceneEvent(SceneType.MapScene));
            Debug.Log("Quit");
        }
    }
}