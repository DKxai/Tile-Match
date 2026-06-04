using System;
using _Scripts.Data;
using _Scripts.Utils;
using _Scripts.Utils.Event_Bus;
using UnityEngine;

namespace _Scripts.Managers
{
    public class CurrencyManager : PersistentSingleton<CurrencyManager>
    {
        public int Coins { get; private set; }

        private DataSystem _dataSystem;

        // Custom event accessor: bất kỳ CoinUI nào subscribe đều nhận giá trị hiện tại ngay lập tức
        private Action<int> _onCoinsChanged;

        public event Action<int> OnCoinsChanged
        {
            add
            {
                _onCoinsChanged += value;
                value?.Invoke(Coins); // gửi giá trị hiện tại ngay khi subscribe
            }
            remove { _onCoinsChanged -= value; }
        }

        private void OnEnable()
        {
            EventBus.Subscribe<PurchaseEvent>(SpendCoins);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<PurchaseEvent>(SpendCoins);
        }

        protected override void Awake()
        {
            base.Awake();
            _dataSystem = new DataSystem();
            Coins = _dataSystem.LoadCoins();
        }

        public bool HasEnoughCoins(int cost) => Coins >= cost;

        public void AddCoins(int amount)
        {
            Coins += amount;
            Save();
        }

        private void SpendCoins(PurchaseEvent evt)
        {
            int cost = evt.Cost;
            if (!HasEnoughCoins(cost))
            {
                EventBus.Publish(new NotEnoughCurrencyEvent());
                return;
            }

            Coins -= cost;
            Save();
        }

        private void Save()
        {
            _dataSystem.SaveCoins(Coins);
            _onCoinsChanged?.Invoke(Coins); // dùng backing field, không dùng event trực tiếp
        }
    }
}