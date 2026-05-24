using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.Tools_Confirm
{
    public class ConfirmPopup : UIPopup
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image icon;
        [SerializeField] private Button confirmButton;
        [SerializeField] private TMP_Text confirmText;
        private Action _onConfirm;

        public void Setup(string title, string description,string confirm, Sprite sprite, Action onConfirm)
        {
            titleText.text = title;
            descriptionText.text = description;
            icon.sprite = sprite;
            confirmText.text = confirm;
            _onConfirm = onConfirm;
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(() => _onConfirm?.Invoke());
        }
    }
}