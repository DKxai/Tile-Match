using System;
using _Scripts.Data;
using _Scripts.Data.Tool;
using _Scripts.Managers;
using _Scripts.SaveSystem;
using _Scripts.Utils;
using _Scripts.Utils.Event_Bus;
using Grid_Map;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class ToolBarUI : MonoBehaviour
    {
        [Header("Buttons")] [SerializeField] private Button shuffleBtn;
        [SerializeField] private Button addSlotBtn;
        [SerializeField] private Button returnBtn;

        [Header("Use Left TMP")] [SerializeField]
        private TMP_Text shuffleText;

        [SerializeField] private TMP_Text addSlotText;
        [SerializeField] private TMP_Text returnText;

        private void OnEnable()
        {
            EventBus.Subscribe<ToolUseChangeEvent>(UpdateUseLeftText);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<ToolUseChangeEvent>(UpdateUseLeftText);
        }

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            UpdateTextButton();
        }

        private void UpdateTextButton()
        {
            shuffleText.text = DataSystem.LoadToolUse(ToolType.Shuffle).ToString();
            addSlotText.text = DataSystem.LoadToolUse(ToolType.AddSlot).ToString();
            returnText.text = DataSystem.LoadToolUse(ToolType.Return).ToString();
        }

        void Init()
        {
            shuffleBtn.onClick.AddListener(OnShuffleClicked);
            addSlotBtn.onClick.AddListener(OnAddSlotClicked);
            returnBtn.onClick.AddListener(OnReturnClicked);
        }

        private void OnDestroy()
        {
            shuffleBtn.onClick.RemoveListener(OnShuffleClicked);
            addSlotBtn.onClick.RemoveListener(OnAddSlotClicked);
            returnBtn.onClick.RemoveListener(OnReturnClicked);
        }

        void OnShuffleClicked() => ToolManager.Instance.UseShuffle();
        void OnAddSlotClicked() => ToolManager.Instance.UseAddSlot();
        void OnReturnClicked() => ToolManager.Instance.UseReturn();

        private void UpdateUseLeftText(ToolUseChangeEvent evt)
        {
            switch (evt.ToolType)
            {
                case ToolType.Shuffle:
                    UpdateLabel(shuffleText, evt.UseLeft);
                    break;
                case ToolType.AddSlot:
                    UpdateLabel(addSlotText, evt.UseLeft);
                    break;
                case ToolType.Return:
                    UpdateLabel(returnText, evt.UseLeft);
                    break;
            }
        }

        private void UpdateLabel(TMP_Text text, int useLeft)
        {
            if (text == null) return;
            text.text = useLeft > 0 ? useLeft.ToString() : "0";
        }
    }
}