using _Scripts.Data;
using _Scripts.UI.Tools_Confirm;
using _Scripts.Utils.Event_Bus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Managers
{
    public class GameUIManager : UIManager
    {
        [SerializeField] private Button homeButton;
        [SerializeField] private TMP_Text titleText;

        private int CurrentLevel => PlayerPrefs.GetInt("CurrentLevel", 1);

        protected override void Awake()      // <-- override + gọi base
        {
            base.Awake();
            homeButton.onClick.AddListener(OnHomeClicked);
        }

        protected override void OnDestroy()  // <-- override + gọi base
        {
            base.OnDestroy();
            homeButton.onClick.RemoveListener(OnHomeClicked);
        }

        private void OnEnable() =>
            EventBus.Subscribe<OutOfToolUseEvent>(ConfirmPopupShow);

        private void OnDisable() =>
            EventBus.Unsubscribe<OutOfToolUseEvent>(ConfirmPopupShow);

        private void Start() => titleText.text = "Level " + CurrentLevel;

        private void OnHomeClicked() =>
            ConfirmPopupShow(new OutOfToolUseEvent(ToolType.Quit));
    }
}