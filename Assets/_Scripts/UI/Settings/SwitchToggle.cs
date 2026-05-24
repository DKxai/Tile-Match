using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.Settings
{
    public class SwitchToggle : MonoBehaviour
    {
        [SerializeField] private RectTransform handleRect;
        [SerializeField] private Color backgroundActiveColor;
        [SerializeField] private Color handleActiveColor;
        private Vector2 _handlePos;
        private Toggle _toggle;
        private Image _backgroundImage;
        private Image _handleImage;
        private Color _backgroundDefaultColor;
        private Color _handleDefaultColor;

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
            _handlePos = handleRect.anchoredPosition;
            _backgroundImage = handleRect.parent.GetComponent<Image>();
            _handleImage = handleRect.GetComponent<Image>();

            _backgroundDefaultColor = _backgroundImage.color;
            _handleDefaultColor = _handleImage.color;

            _toggle.onValueChanged.AddListener(OnToggleValueChanged);
            if (_toggle.isOn)
                OnToggleValueChanged(true);
        }

        private void OnDestroy() => _toggle.onValueChanged.RemoveListener(OnToggleValueChanged);

        private void OnToggleValueChanged(bool isOn)
        {
            handleRect.DOAnchorPos(isOn ? -_handlePos : _handlePos, 0.4f).SetEase(Ease.InOutBack);
            _backgroundImage.DOColor(isOn ? backgroundActiveColor : _backgroundDefaultColor, 0.6f);
            _handleImage.DOColor(isOn ? handleActiveColor : _handleDefaultColor, 0.4f);
        }
    }
}