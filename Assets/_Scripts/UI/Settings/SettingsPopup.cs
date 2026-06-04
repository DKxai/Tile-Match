using System;
using _Scripts.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace _Scripts.UI.Settings
{
    public class SettingsPopup : UIPopup
    {
        [Header("Animation")] [SerializeField] private RectTransform panel;
        [SerializeField] private CanvasGroup canvasGroup;

        [SerializeField] private float hiddenY = -100f;

        [SerializeField] private float showDuration = 0.35f;
        [SerializeField] private float hideDuration = 0.35f;

        private Vector2 _shownPosition;

        [Header("Toggles")] [SerializeField] private Toggle musicToggle;
        [SerializeField] private Toggle soundToggle;
        [SerializeField] private Toggle vibrationToggle;

        [Header("Close")] [SerializeField] private Button closeButton;

        protected virtual void Awake()
        {
            _shownPosition = panel.anchoredPosition;
            RegisterEvents();
        }

        protected virtual void OnDestroy()
        {
            UnregisterEvents();
        }

        private void Start()
        {
            LoadUI();
        }

        private void LoadUI()
        {
            var data = SettingsManager.Instance.SettingData;

            musicToggle.SetIsOnWithoutNotify(data.MusicEnable);
            soundToggle.SetIsOnWithoutNotify(data.SoundEnable);
            vibrationToggle.SetIsOnWithoutNotify(data.VibrationEnable);
        }


        #region Register & Unregister Events

        private void RegisterEvents()
        {
            musicToggle.onValueChanged.AddListener(OnMusicChanged);
            soundToggle.onValueChanged.AddListener(OnSoundChanged);
            vibrationToggle.onValueChanged.AddListener(OnVibrationChanged);
            closeButton.onClick.AddListener(Hide);
        }

        private void UnregisterEvents()
        {
            musicToggle.onValueChanged.RemoveListener(OnMusicChanged);
            soundToggle.onValueChanged.RemoveListener(OnSoundChanged);
            vibrationToggle.onValueChanged.RemoveListener(OnVibrationChanged);

            closeButton.onClick.RemoveListener(Hide);
        }

        #endregion

        #region Event Handlers

        private void OnMusicChanged(bool value)
        {
            SettingsManager.Instance.SetMusic(value);
        }

        private void OnSoundChanged(bool value)
        {
            SettingsManager.Instance.SetSound(value);
        }

        private void OnVibrationChanged(bool value)
        {
            SettingsManager.Instance.SetVibration(value);
        }
        

        #endregion

        #region Animation

        protected override void PlayShowAnimation()
        {
            panel.DOKill();
            canvasGroup.DOKill();

            canvasGroup.alpha = 0f;

            panel.anchoredPosition = new Vector2(_shownPosition.x, hiddenY);

            canvasGroup.DOFade(1f, showDuration);

            panel.DOAnchorPos(_shownPosition, showDuration).SetEase(Ease.OutBack);
        }

        protected override void PlayHideAnimation()
        {
            panel.DOKill();
            canvasGroup.DOKill();
            canvasGroup.alpha = 1f;

            canvasGroup.DOFade(0f, hideDuration);
            panel.DOAnchorPosY(hiddenY, hideDuration).SetEase(Ease.InBack)
                .OnComplete(() => { gameObject.SetActive(false); });
        }

        #endregion
    }
}