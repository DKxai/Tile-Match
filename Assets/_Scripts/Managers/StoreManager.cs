using System;
using System.Collections.Generic;
using _Scripts.Data;
using _Scripts.Data.Tool;
using _Scripts.SaveSystem;
using _Scripts.UI.Store;
using _Scripts.Utils;
using _Scripts.Utils.Event_Bus;
using UnityEngine;

namespace _Scripts.Managers
{
    public class StoreManager : PersistentSingleton<StoreManager>
    {
        [SerializeField] private StorePopup storePopupPrefab;
        [SerializeField] private Transform persistentUIRoot;
        [SerializeField] private List<StoreItemData> items;
        public IReadOnlyList<StoreItemData> Items => items;
        private StorePopup _storePopup;

        private StorePopup StorePopup
        {
            get
            {
                if (!_storePopup)
                {
                    _storePopup = Instantiate(storePopupPrefab, persistentUIRoot);
                    _storePopup.gameObject.SetActive(false);
                }

                return _storePopup;
            }
        }

        private void OnEnable()
        {
            EventBus.Subscribe<NotEnoughCurrencyEvent>(NotEnoughCurrency);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<NotEnoughCurrencyEvent>(NotEnoughCurrency);
        }

        private void NotEnoughCurrency(NotEnoughCurrencyEvent evt)
        {
            ShowStore();
        }

        public bool Buy(StoreItemData item)
        {
            switch (item.Type)
            {
                case StoreItemType.Coins:
                    CurrencyManager.Instance.AddCoins(item.RewardAmount);
                    break;
                case StoreItemType.Combo:
                    DataSystem.SaveToolUse(ToolType.AddSlot,
                        DataSystem.LoadToolUse(ToolType.AddSlot) + item.RewardAmount);
                    DataSystem.SaveToolUse(ToolType.Shuffle,
                        DataSystem.LoadToolUse(ToolType.Shuffle) + item.RewardAmount);
                    DataSystem.SaveToolUse(ToolType.Return,
                        DataSystem.LoadToolUse(ToolType.Return) + item.RewardAmount);
                    break;
            }

            return true;
        }

        public void ShowStore()
        {
            StorePopup.Show();
        }
    }
}