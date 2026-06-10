using System;
using System.Collections.Generic;
using _Scripts.Data.EndingUI;
using _Scripts.Data.Sounds;
using _Scripts.Managers;
using _Scripts.SaveSystem;
using _Scripts.Utils.Event_Bus;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class EndingUI : UIPopup
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text endingText;
        [SerializeField] private Button rButton;
        [SerializeField] private Button lButton;
        [SerializeField] private GameObject coinsStack;
        [SerializeField] private GameObject coinUI;
        [SerializeField] private GameObject heartUI;
        [SerializeField] private Image mainPanel;

        public void InitEndingUI(EndingInfo info, Action rCallback, Action lCallback)
        {
            image.sprite = info.Image;
            endingText.text = info.EndingText;
            rButton.GetComponentInChildren<TextMeshProUGUI>().text = info.RText;
            lButton.GetComponentInChildren<TextMeshProUGUI>().text = info.LText;
            coinsStack.SetActive(info.HaveCoinsStack);
            coinUI.SetActive(info.HaveCoinsStack);
            heartUI.SetActive(!info.HaveCoinsStack);
            mainPanel.color = info.MainPanelColor;
            rButton.onClick.RemoveAllListeners();
            lButton.onClick.RemoveAllListeners();
            rButton.onClick.AddListener(() => rCallback?.Invoke());
            lButton.onClick.AddListener(() => lCallback?.Invoke());
        }

        private void OnDisable()
        {
            rButton.onClick.RemoveAllListeners();
            lButton.onClick.RemoveAllListeners();
        }

        public void CoinsAward()
        {
            RectTransform[] coins = coinsStack.GetComponentsInChildren<RectTransform>();
            RectTransform target = coinUI.GetComponent<RectTransform>();
            Vector3 originalScale = target.localScale;
            DOVirtual.DelayedCall(showDuration, () =>
            {
                Vector3 targetWorldPos = target.position;

                for (int i = 1; i < coins.Length; i++)
                {
                    int idx = i;
                    float delay = (i - 1) * 0.2f;

                    coins[i].DOMove(targetWorldPos, 0.4f).SetEase(Ease.InQuad).SetDelay(delay)
                        .OnComplete(() =>
                        {
                            coins[idx].gameObject.SetActive(false);

                            target.DOKill();
                            target.localScale = originalScale;
                            target.DOPunchScale(Vector3.one * 0.3f, 0.25f, 5, 0.5f).SetEase(Ease.OutQuad);
                            CurrencyManager.Instance.AddCoins(20 * DataSystem.LoadSelectedLevel());
                        });
                }
            });
        }
    }
}