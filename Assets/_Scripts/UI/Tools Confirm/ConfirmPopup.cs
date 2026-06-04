using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.Tools_Confirm
{
    public class ConfirmPopup : UIPopup
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image icon;
        [SerializeField] private Button confirmButton;
        [SerializeField] private TMP_Text confirmText;
        [SerializeField] private Button closeButton;
        [SerializeField] private CoinUI coinUI;
        [SerializeField] private HeartUI heartUI;
        private Action _onConfirm;

        public void Setup(string title, string description,string confirm, Sprite sprite, Action onConfirm, bool isDisplayCoinUI,bool isDisplayHeartUI)
        {
            titleText.text = title;
            descriptionText.text = description;
            icon.sprite = sprite;
            confirmText.text = confirm;
            _onConfirm = onConfirm;
            confirmButton.onClick.RemoveAllListeners();
            closeButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(() => _onConfirm?.Invoke());
            closeButton.onClick.AddListener(Hide);
            coinUI.gameObject.SetActive(isDisplayCoinUI);
            heartUI.gameObject.SetActive(isDisplayHeartUI);
        }

        private void OnDestroy()
        {
            confirmButton.onClick.RemoveAllListeners();
            closeButton.onClick.RemoveAllListeners();
        }

        public override void Show()
        {
            base.Show();
        }

        protected override void PlayHideAnimation()
        {
            base.PlayHideAnimation();
        }
    }
}