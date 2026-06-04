using System.Collections;
using _Scripts.Managers;
using _Scripts.Utils.Event_Bus;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace _Scripts.UI
{
    public class HeartUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text heartNumText;
        [SerializeField] private TMP_Text countText;
        [SerializeField] private Button addHeartButton;

        private const int MaxHearts = 5;
        private Coroutine _waitCoroutine;

        private void OnEnable()
        {
            if (HeartManager.Instance != null)
                Subscribe();
            else
                _waitCoroutine = StartCoroutine(WaitForManager());
        }

        private void OnDisable()
        {
            // Hủy coroutine nếu đang chờ
            if (_waitCoroutine != null)
            {
                StopCoroutine(_waitCoroutine);
                _waitCoroutine = null;
            }

            if (HeartManager.Instance == null) return;
            HeartManager.Instance.OnHeartsChanged -= UpdateHeartText;
            HeartManager.Instance.OnTimerTick -= UpdateTimerText;
        }

        private IEnumerator WaitForManager()
        {
            yield return new WaitUntil(() => HeartManager.Instance != null);
            Subscribe();
        }

        private void Subscribe()
        {
            HeartManager.Instance.OnHeartsChanged += UpdateHeartText;
            HeartManager.Instance.OnTimerTick += UpdateTimerText;

            // Sync trạng thái hiện tại ngay khi subscribe
            if (HeartManager.Instance.Hearts < MaxHearts)
            {
                UpdateHeartText(HeartManager.Instance.Hearts);
                UpdateTimerText(HeartManager.Instance.RemainingSeconds);
            }
            else
            {
                UpdateHeartText(HeartManager.Instance.Hearts);
            }
        }

        private void UpdateHeartText(int hearts)
        {
            heartNumText.text = hearts.ToString();
            countText.text = hearts >= MaxHearts ? "Full" : countText.text;
            addHeartButton.gameObject.SetActive(hearts < MaxHearts);
        }

        private void UpdateTimerText(float remainingSeconds)
        {
            int minutes = (int)(remainingSeconds / 60);
            int seconds = (int)(remainingSeconds % 60);
            countText.text = $"{minutes:D2}:{seconds:D2}";
        }
    }
}