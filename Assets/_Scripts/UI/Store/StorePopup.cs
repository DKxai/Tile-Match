using System;
using System.Collections.Generic;
using _Scripts.Data;
using _Scripts.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.Store
{
    public class StorePopup : UIPopup
    {
        [Header("Animation")] [SerializeField] private RectTransform panel;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float showDuration;
        [SerializeField] private float hideDuration;

        [Header("Buttons")] [SerializeField] private Button closeButton;

        [Header("Store")] [SerializeField] private StoreItemUI itemPrefab;
        [SerializeField] private StoreItemUI comboPrefab;
        [SerializeField] private Transform content;
        private readonly List<StoreItemUI> _spawnedItems = new();

        protected virtual void Awake()
        {
            closeButton.onClick.AddListener(Close);
        }

        protected virtual void OnDestroy()
        {
            closeButton.onClick.RemoveListener(Close);
        }

        public override void Show()
        {
            base.Show();
            RefreshStore();
        }

        private void RefreshStore()
        {
            Clear();

            foreach (StoreItemData data in StoreManager.Instance.Items)
            {
                StoreItemUI item;
                switch (data.Type)
                {
                    case StoreItemType.Coins:
                        item = Instantiate(itemPrefab, content);
                        break;
                    case StoreItemType.Combo:
                        item = Instantiate(comboPrefab, content);
                        break;
                    default:
                        Debug.LogError($"Unknown type {data.Type}");
                        continue;
                }

                item.Setup(data, BuyItem);
                _spawnedItems.Add(item);
            }
        }

        private void BuyItem(StoreItemData data)
        {
            bool sucess = StoreManager.Instance.Buy(data);
            if (sucess) Debug.Log("Purchased successfully");
            else Debug.LogError("Purchased failed");
        }

        private void Clear()
        {
            foreach (StoreItemUI item in _spawnedItems)
            {
                Destroy(item.gameObject);
            }

            _spawnedItems.Clear();
        }

        #region Animation

        protected override void PlayShowAnimation()
        {
            panel.DOKill();
            canvasGroup.DOKill();

            panel.localScale = Vector3.zero;
            canvasGroup.alpha = 0f;

            canvasGroup.DOFade(1f, showDuration);
            panel.DOScale(1f, showDuration).SetEase(Ease.OutBack);
        }

        protected override void PlayHideAnimation()
        {
            panel.DOKill();
            canvasGroup.DOKill();

            canvasGroup
                .DOFade(0f, hideDuration);

            panel
                .DOScale(0f, hideDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() => { gameObject.SetActive(false); });
        }

        #endregion

        private void Close()
        {
            UIManager.Instance.Close(this);
        }
    }
}