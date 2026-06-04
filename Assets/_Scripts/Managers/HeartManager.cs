using System;
using _Scripts.Data;
using _Scripts.Utils.Event_Bus;
using UnityEngine;
using UnityEngine.Profiling;

namespace _Scripts.Managers
{
    public class HeartManager : PersistentSingleton<HeartManager>
    {
        public int Hearts { get; private set; }
        public float RemainingSeconds { get; private set; }

        private const int MaxHearts = 5;
        private const float RechargeSeconds = 300f;

        private readonly DataSystem _dataSystem = new();
        private DateTime _lastHeartTime;
        private Action<int> _onHeartsChanged;

        public event Action<int> OnHeartsChanged
        {
            add
            {
                _onHeartsChanged += value;
                value?.Invoke(Hearts);
            }
            remove => _onHeartsChanged -= value;
        }

        private Action<float> _onTimerTick;

        public event Action<float> OnTimerTick
        {
            add
            {
                _onTimerTick += value;
                value?.Invoke(RemainingSeconds);
            }
            remove => _onTimerTick -= value;
        }
        

        protected override void Awake()
        {
            base.Awake();
            Hearts = _dataSystem.LoadHearts();
            _lastHeartTime = _dataSystem.LoadLastHeartTime();
            RecoverOfflineHearts();
        }

        private void Update()
        {
            if (Hearts >= MaxHearts) return;

            TimeSpan elapse = DateTime.Now - _lastHeartTime;
            TimeSpan remaining = TimeSpan.FromSeconds(RechargeSeconds) - elapse;

            if (remaining.TotalSeconds <= 0)
            {
                AddHeart();
            }
            else
            {
                RemainingSeconds = (float)remaining.TotalSeconds;
                _onTimerTick?.Invoke(RemainingSeconds);
            }
        }

        private void RecoverOfflineHearts()
        {
            if (Hearts >= MaxHearts) return;
            TimeSpan timeSpan = DateTime.Now - _lastHeartTime;
            int heartsToAdd = (int)(timeSpan.TotalSeconds / RechargeSeconds);

            if (heartsToAdd <= 0) return;
            Hearts = Mathf.Min(Hearts + heartsToAdd, MaxHearts);
            _lastHeartTime = _lastHeartTime.AddSeconds(heartsToAdd * RechargeSeconds);

            _dataSystem.SaveHearts(Hearts);
            _dataSystem.SaveLastHeartTime(_lastHeartTime);
        }

        public void AddHeart()
        {
            Hearts = Mathf.Min(Hearts + 1, MaxHearts);
            _lastHeartTime = DateTime.Now;

            _dataSystem.SaveHearts(Hearts);
            _dataSystem.SaveLastHeartTime(_lastHeartTime);

            _onHeartsChanged?.Invoke(Hearts);
        }

        public void SpendHeart()
        {
            if (Hearts <= 0) return;

            if (Hearts == MaxHearts)
            {
                _lastHeartTime = DateTime.Now;
                _dataSystem.SaveLastHeartTime(_lastHeartTime);
            }

            Hearts--;
            _dataSystem.SaveHearts(Hearts);
            _onHeartsChanged?.Invoke(Hearts);
        }
    }
}