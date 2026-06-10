using System;
using _Scripts.Data;
using _Scripts.Data.Sounds;
using _Scripts.Utils.Event_Bus;
using UnityEngine;

namespace _Scripts.Managers
{
    public class SettingsManager : PersistentSingleton<SettingsManager>
    {
        private SettingData _data;

        public float MusicValue => _data.MusicValue;
        public float SoundValue => _data.SoundValue;
        public bool VibrationEnable => _data.VibrationEnable;

        public event Action<bool> OnVibrationChanged;

        protected override void Awake()
        {
            base.Awake();
            Load();
        }

        private void Load()
        {
            _data = new SettingData
            {
                MusicValue = PlayerPrefs.GetFloat("Music", 1f),
                SoundValue = PlayerPrefs.GetFloat("Sound", 1f),
                VibrationEnable = PlayerPrefs.GetInt("Vibration", 1) == 1
            };
        }

        public void Save()
        {
            PlayerPrefs.SetFloat("Music", _data.MusicValue);
            PlayerPrefs.SetFloat("Sound", _data.SoundValue);
            PlayerPrefs.SetInt("Vibration", _data.VibrationEnable ? 1 : 0);

            PlayerPrefs.Save();
        }

        public void SetMusic(float value)
        {
            _data.MusicValue = value;
            Save();
            EventBus.Publish(
                new SetVolumeEvent(MixerType.MusicVolume, value)
            );
        }

        public void SetSound(float value)
        {
            _data.SoundValue = value;
            Save();
            EventBus.Publish(
                new SetVolumeEvent(MixerType.SFXVolume, value)
            );
        }

        public void SetVibration(bool value)
        {
            _data.VibrationEnable = value;
            Save();
            OnVibrationChanged?.Invoke(value);
        }
    }
}