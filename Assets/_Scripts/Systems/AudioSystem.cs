using System.Collections.Generic;
using _Scripts.Data.Sounds;
using _Scripts.Managers;
using _Scripts.Utils.Event_Bus;
using UnityEngine;
using UnityEngine.Audio;

namespace _Scripts.Systems
{
    public class AudioSystem : PersistentSingleton<AudioSystem>
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("Audio Mixer")]
        [SerializeField] private AudioMixer audioMixer;

        [Header("Sounds")]
        [SerializeField] private List<SoundInfo> sounds;

        private Dictionary<SoundType, SoundInfo> _soundLookup;

        protected override void Awake()
        {
            base.Awake();

            _soundLookup = new Dictionary<SoundType, SoundInfo>();

            foreach (var sound in sounds)
            {
                _soundLookup[sound.SoundType] = sound;
            }
        }

        private void OnEnable()
        {
            EventBus.Subscribe<PlaySoundEvent>(PlaySound);
            EventBus.Subscribe<SetVolumeEvent>(SetVolume);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<PlaySoundEvent>(PlaySound);
            EventBus.Unsubscribe<SetVolumeEvent>(SetVolume);
        }

        private void Start()
        {
            var settings = SettingsManager.Instance;

            SetVolume(new SetVolumeEvent(
                MixerType.MusicVolume,
                settings.MusicValue));

            SetVolume(new SetVolumeEvent(
                MixerType.SFXVolume,
                settings.SoundValue));

            PlayThemeMusic();
        }

        #region Music

        private void PlayThemeMusic()
        {
            if (!_soundLookup.TryGetValue(SoundType.ThemeSound, out var soundInfo))
            {
                Debug.LogWarning("ThemeSound not found.");
                return;
            }

            if (soundInfo.SoundClip == null)
            {
                Debug.LogWarning("ThemeSound clip is null.");
                return;
            }

            musicSource.clip = soundInfo.SoundClip;
            musicSource.loop = true;
            musicSource.Play();
        }

        public void StopThemeMusic()
        {
            musicSource.Stop();
        }

        #endregion

        #region SFX

        private void PlaySound(PlaySoundEvent evt)
        {
            if (!_soundLookup.TryGetValue(evt.Type, out var soundInfo))
            {
                Debug.LogWarning($"Missing sound config: {evt.Type}");
                return;
            }

            if (soundInfo.SoundClip == null)
            {
                Debug.LogWarning($"Missing clip for: {evt.Type}");
                return;
            }

            // Theme nhạc không phát bằng PlayOneShot
            if (evt.Type == SoundType.ThemeSound)
                return;

            sfxSource.PlayOneShot(soundInfo.SoundClip);
        }

        #endregion

        #region Volume

        private void SetVolume(SetVolumeEvent evt)
        {
            audioMixer.SetFloat(
                evt.MixerType.ToString(),
                LinearToDb(evt.Value));
        }

        private static float LinearToDb(float value)
        {
            if (value <= 0.0001f)
                return -80f;

            return Mathf.Log10(value) * 20f;
        }

        #endregion
    }
}