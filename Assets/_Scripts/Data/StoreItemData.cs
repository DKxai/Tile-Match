using System;
using System.Collections.Generic;
using _Scripts.UI.Store;
using UnityEngine;

namespace _Scripts.Data
{
    [Serializable]
    public class StoreItemData
    {
        public string Id;
        public StoreItemType Type;
        public List<Sprite> Icons;
        public int Price;
        public int RewardAmount;
    }
}