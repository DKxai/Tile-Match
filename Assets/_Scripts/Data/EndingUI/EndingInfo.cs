using System;
using UnityEngine;

namespace _Scripts.Data.EndingUI
{
    [Serializable]
    public class EndingInfo
    {
        public Sprite Image;
        public string EndingText;
        public string RText;
        public string LText;
        public Color MainPanelColor;
        public bool HaveCoinsStack;
        public bool IsNeedCoinsUI;
    }
}