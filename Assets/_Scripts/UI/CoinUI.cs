using System.Collections;
using _Scripts.Managers;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class CoinUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text coinText;
        private Coroutine _waitCoroutine;

        private void OnEnable()
        {
            if (CurrencyManager.Instance != null)
                CurrencyManager.Instance.OnCoinsChanged += UpdateText;
            else
                _waitCoroutine = StartCoroutine(WaitForManager());
        }

        private IEnumerator WaitForManager()
        {
            yield return new WaitUntil(() => CurrencyManager.Instance != null);
            CurrencyManager.Instance.OnCoinsChanged += UpdateText;
        }

        private void OnDisable()
        {
            if (_waitCoroutine != null)
            {
                StopCoroutine(_waitCoroutine);
                _waitCoroutine = null;
            }
            if (CurrencyManager.Instance != null)
                CurrencyManager.Instance.OnCoinsChanged -= UpdateText;
        }

        private void UpdateText(int coins) => coinText.text = coins.ToString();
    }
}