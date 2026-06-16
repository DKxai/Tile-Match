using _Scripts.Data;
using _Scripts.Data.Tool;
using _Scripts.Managers;
using _Scripts.SaveSystem;
using _Scripts.Utils;
using _Scripts.Utils.Event_Bus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class LevelNode : MonoBehaviour
    {
        public enum State
        {
            Locked,
            Current,
            Complete
        }

        [Header("Colors")] [SerializeField] private Color lockedColor = Color.gray;
        [SerializeField] private Color currentColor = new Color(1f, 0.8f, 0f);
        [SerializeField] private Color completeColor = new Color(0.2f, 0.8f, 0.3f);

        private Button _button;
        private Image _nodeImage;
        private TextMeshProUGUI _levelText;

        private int _levelIndex;
        private State _currentState;

        private void Awake()
        {
            _button = GetComponent<Button>();

            _nodeImage = GetComponent<Image>();

            _levelText = GetComponentInChildren<TextMeshProUGUI>();

            if (_button == null)
            {
                Debug.LogError("Button component missing!", this);
                return;
            }

            if (_nodeImage == null)
            {
                Debug.LogError("Image component missing!", this);
                return;
            }

            if (_levelText == null)
            {
                Debug.LogError("TMP Text missing!", this);
                return;
            }

            _button.onClick.AddListener(OnClicked);
        }

        public void Setup(int index)
        {
            _levelIndex = index;

            _levelText.text = _levelIndex.ToString();

            UpdateVisual();
        }

        public void SetState(State state)
        {
            _currentState = state;

            UpdateVisual();
        }

        private void UpdateVisual()
        {
            switch (_currentState)
            {
                case State.Current:
                    _nodeImage.color = currentColor;
                    _button.interactable = true;
                    break;

                case State.Complete:
                    _nodeImage.color = completeColor;
                    _button.interactable = true;
                    break;

                default:
                    _nodeImage.color = lockedColor;
                    _button.interactable = false;
                    break;
            }
        }

        private void OnClicked()
        {
            if(DataSystem.LoadHearts() == 0)
            {
                EventBus.Publish(new OnAddHeartButtonClickedEvent(ToolType.Heart));
                return;
            }
            HeartManager.Instance.SpendHeart();
            DataSystem.SaveSelectedLevel(_levelIndex);
            EventBus.Publish(new LoadSceneEvent(SceneType.GameScene));
        }
    }
}