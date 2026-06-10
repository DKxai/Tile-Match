using System;
using System.Collections.Generic;
using _Scripts.Data;
using _Scripts.Managers;
using _Scripts.Utils.Event_Bus;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.Store
{
    public sealed class StorePopup : UIPopup
    {
        [Header("Buttons")] [SerializeField] private Button closeButton;

        [Header("Store")] [SerializeField] private StoreItemUI itemPrefab;
        [SerializeField] private StoreItemUI comboPrefab;
        [SerializeField] private Transform content;
        private readonly List<StoreItemUI> _spawnedItems = new();


        protected override void Awake()
        {
            base.Awake();
            closeButton.onClick.AddListener(Hide);
        }

        private void OnDestroy()
        {
            closeButton.onClick.RemoveListener(Hide);
        }

       
        public override void Show()
        {
            gameObject.SetActive(true);
            RefreshStore();
            PlayShowAnimation();
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
            if (sucess)
            {
                Debug.Log("Purchased successfully");
                
            }
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

            canvasGroup.alpha = 1f; 
            panel.localScale = Vector3.one;

            int childCount = content.childCount;
            float stagger = 0.05f;

            for (int i = 0; i < childCount; i++)
            {
                RectTransform row = content.GetChild(i) as RectTransform;
                if (row == null) continue;

                row.localScale = Vector3.zero;

                CanvasGroup rowCG = row.GetComponent<CanvasGroup>();
                if (rowCG == null) rowCG = row.gameObject.AddComponent<CanvasGroup>();
                rowCG.alpha = 0f;

                row.DOScale(Vector3.one, 0.35f)
                    .SetEase(Ease.OutBack)
                    .SetDelay(i * stagger);

                rowCG.DOFade(1f, 0.2f)
                    .SetDelay(i * stagger);
            }
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
    }
}