using System;
using System.Collections.Generic;
using _Scripts.Data;
using _Scripts.UI.Store;
using _Scripts.Utils;
using _Scripts.Utils.Event_Bus;
using UnityEngine;
using Utils;

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
                    BuyToolUses(new PurchaseEvent(ToolType.AddSlot,item.RewardAmount,0));
                    BuyToolUses(new PurchaseEvent(ToolType.Return,item.RewardAmount,0));
                    BuyToolUses(new PurchaseEvent(ToolType.Shuffle,item.RewardAmount,0));
                    break;
            }

            Debug.Log($"Buying item {item}");
            return true;
        }

        private void BuyToolUses(PurchaseEvent evt)
        {
            if (!CurrencyManager.Instance.HasEnoughCoins(evt.Cost))
            {
                EventBus.Publish(new NotEnoughCurrencyEvent());
                return;
            }

            ToolManager.Instance.AddUse(evt);
        }

        public void ShowStore()
        {
            StorePopup.Show();
        }
    }
}