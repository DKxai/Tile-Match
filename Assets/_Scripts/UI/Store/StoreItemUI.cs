using System;
using System.Collections.Generic;
using _Scripts.Data;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace _Scripts.UI.Store
{
    public class StoreItemUI : MonoBehaviour
    {
        [SerializeField] private List<Image> iconImages;
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private TMP_Text rewardText;

        [SerializeField] private Button buyButton;
        private StoreItemData _storeItemData;
        private Action<StoreItemData> _onBuyClicked;

        public void Setup(StoreItemData storeItemData, Action<StoreItemData> onBuyClicked)
        {
            _storeItemData = storeItemData;
            _onBuyClicked = onBuyClicked;
            if (storeItemData.Icons.Count != iconImages.Count)
            {
                Debug.Log("Not enough icon images");
                return;
            }

            for (int i = 0; i < iconImages.Count; i++)
            {
                iconImages[i].sprite = storeItemData.Icons[i];
            }

            priceText.text = "USD " + storeItemData.Price.ToString();
            if (storeItemData.Type == StoreItemType.Combo)
            {
                rewardText.text = "POWER PACK";
            }
            else
            {
                rewardText.text = "x" + storeItemData.RewardAmount.ToString();
            }

            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(() => _onBuyClicked?.Invoke(_storeItemData));
        }
    }
}