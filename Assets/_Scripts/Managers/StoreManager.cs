using System.Collections.Generic;
using _Scripts.Data;
using UnityEngine;

namespace _Scripts.Managers
{
    public class StoreManager:Singleton<StoreManager>
    {
        [SerializeField] private List<StoreItemData> items;
        public  IReadOnlyList<StoreItemData> Items => items;

        public bool Buy(StoreItemData item)
        {
            if (!CurrencyManager.Instance.HasEnoughCoins(item.Price))
            {
                Debug.LogError($"Not enough coins for item {item}");
                return false;
            }
             
            Debug.Log($"Buying item {item}");
            return true;

        }
    }
}