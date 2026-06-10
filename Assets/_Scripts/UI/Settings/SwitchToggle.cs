using _Scripts.Data;
using _Scripts.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.Settings
{
    public class SwitchToggle : MonoBehaviour
    {
        [SerializeField] private RectTransform handleRect;
        [SerializeField] private Color backgroundActiveColor;

        public Toggle Toggle => _toggle;

        private Toggle _toggle;
        private Image _backgroundImage;
        private Color _backgroundDefaultColor;
        private Vector2 _handleOnPos;
        private Vector2 _handleOffPos;
        private bool _initialized;

        private void Awake()
        {
            EnsureInitialized();
            _toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        private void OnDestroy() => _toggle.onValueChanged.RemoveListener(OnToggleValueChanged);

        public void Init()
        {
            EnsureInitialized();
        }

        private void EnsureInitialized()
        {
            if (_initialized) return;
            _initialized = true;

            _toggle = GetComponent<Toggle>();
            _backgroundImage = handleRect.parent.GetComponent<Image>();
            _backgroundDefaultColor = _backgroundImage.color;
            _handleOnPos = handleRect.anchoredPosition;
            _handleOffPos = new Vector2(-handleRect.anchoredPosition.x,
                handleRect.anchoredPosition.y);
            SyncVisualImmediate(_toggle.isOn);
        }

        private void OnToggleValueChanged(bool isOn)
        {
            handleRect.DOKill(true);
            _backgroundImage.DOKill(true);

            handleRect.DOAnchorPos(isOn ? _handleOnPos : _handleOffPos, 0.4f)
                .SetEase(Ease.InOutBack);
            _backgroundImage.DOColor(isOn ? backgroundActiveColor : _backgroundDefaultColor, 0.6f);
        }

        public void SetOnWithoutNotify(bool isOn)
        {
            EnsureInitialized();
            handleRect.DOKill(true);
            _backgroundImage.DOKill(true);
            _toggle.SetIsOnWithoutNotify(isOn);
            SyncVisualImmediate(isOn);
        }

        private void SyncVisualImmediate(bool isOn)
        {
            handleRect.anchoredPosition = isOn ? _handleOnPos : _handleOffPos;
            _backgroundImage.color = isOn ? backgroundActiveColor : _backgroundDefaultColor;
        }
    }
}