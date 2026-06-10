using System;
using _Scripts.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.Settings
{
    public sealed class SettingsPopup : UIPopup
    {
        [Header("Toggles")] [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider soundSlider;
        [SerializeField] private SwitchToggle vibrationToggle;

        [Header("Close")] [SerializeField] private Button closeButton;


        protected override void Awake()
        {
            base.Awake();
            vibrationToggle.Init();
        }

        private void Start()
        {
            LoadUI();
            IsInitialized = true;
        }

        private void OnEnable()
        {
            if (IsInitialized)
                LoadUI();

            if (SettingsManager.Instance != null)
                RegisterEvents();
        }

        private void OnDisable()
        {
            if (SettingsManager.Instance != null)
                UnregisterEvents();
        }


        private void LoadUI()
        {
            var m = SettingsManager.Instance;
            if (m == null) return;

            musicSlider.SetValueWithoutNotify(m.MusicValue);
            soundSlider.SetValueWithoutNotify(m.SoundValue);
            vibrationToggle.SetOnWithoutNotify(m.VibrationEnable);
        }

        private void SyncVibrationToggle(bool value)
        {
            vibrationToggle.SetOnWithoutNotify(value);
        }


        private void RegisterEvents()
        {
            musicSlider.onValueChanged.AddListener(OnMusicChanged);
            soundSlider.onValueChanged.AddListener(OnSoundChanged);
            vibrationToggle.Toggle.onValueChanged.AddListener(OnVibrationChanged);
            closeButton.onClick.AddListener(Hide);
            SettingsManager.Instance.OnVibrationChanged += SyncVibrationToggle;
        }

        private void UnregisterEvents()
        {
            musicSlider.onValueChanged.RemoveListener(OnMusicChanged);
            soundSlider.onValueChanged.RemoveListener(OnSoundChanged);
            vibrationToggle.Toggle.onValueChanged.RemoveListener(OnVibrationChanged);
            closeButton.onClick.RemoveListener(Hide);
            SettingsManager.Instance.OnVibrationChanged -= SyncVibrationToggle;
        }


        private void OnMusicChanged(float value) => SettingsManager.Instance.SetMusic(value);
        private void OnSoundChanged(float value) => SettingsManager.Instance.SetSound(value);

        private void OnVibrationChanged(bool value)
        {
            SettingsManager.Instance.SetVibration(value);
            if (value) Handheld.Vibrate();
        }
    }
}