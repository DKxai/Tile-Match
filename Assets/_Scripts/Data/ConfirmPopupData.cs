using System;
using UnityEngine;

namespace _Scripts.Data
{
    [Serializable]
    public class ConfirmPopupData
    {
        public string title;
        public string description;
        public Sprite icon;
        public string confirm;
        public int cost;
        public int amount;
        public bool isDisplayCoinUi;
        public bool isDisplayHeartUI;
    }
}