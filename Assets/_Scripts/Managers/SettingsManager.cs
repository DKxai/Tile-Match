using _Scripts.Data;
using UnityEngine;

namespace _Scripts.Managers
{
    public class SettingsManager : StaticInstance<SettingsManager>
    {
        public SettingData SettingData { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Load();
        }

        private void Load()
        {
            SettingData = new SettingData();

            SettingData.SoundEnable = PlayerPrefs.GetInt("Sound", 1) == 1;
            SettingData.MusicEnable = PlayerPrefs.GetInt("Music", 1) == 1;
            SettingData.VibrationEnable = PlayerPrefs.GetInt("Vibration", 1) == 1;
        }

        private void Save()
        {
            PlayerPrefs.SetInt("Music", SettingData.MusicEnable ? 1 : 0);
            PlayerPrefs.SetInt("Sound", SettingData.SoundEnable ? 1 : 0);
            PlayerPrefs.SetInt("Vibration", SettingData.VibrationEnable ? 1 : 0);
            PlayerPrefs.Save();
        }
        public void SetMusic(bool value)
        {
            SettingData.MusicEnable = value;
            PlayerPrefs.SetInt("Music", value ? 1 : 0);
            Save();
            // set music
        }

        public void SetSound(bool value)
        {
            SettingData.SoundEnable = value;
            PlayerPrefs.SetInt("Sound", value ? 1 : 0);
            Save();
        }

        public void SetVibration(bool value)
        {
            SettingData.VibrationEnable = value;
            PlayerPrefs.SetInt("Vibration", value ? 1 : 0);
            Save();
        }
    }
}