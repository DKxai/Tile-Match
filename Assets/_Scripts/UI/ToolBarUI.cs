using System;
using _Scripts.Managers;
using Grid_Map;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace _Scripts.UI
{
    public class ToolBarUI : MonoBehaviour
    {
        [Header("Buttons")] [SerializeField] private Button shuffleBtn;

        [SerializeField] private Button addSlotBtn;

        //   [SerializeField] private Button hintBtn;
        [SerializeField] private Button returnBtn;

        [Header("Use Left TMP")] [SerializeField]
        private TMP_Text shuffleText;

        [SerializeField] private TMP_Text addSlotText;

        //  [SerializeField] private TMP_Text hintText;
        [SerializeField] private TMP_Text returnText;

        // [Header("References")] [SerializeField]
        // private ToolData _toolData;

        private void Awake()
        {
            Init();
            TileEventBus.OnToolUsed += OnToolUsed;
        }

        void Init()
        {
            shuffleBtn.onClick.AddListener(() => ToolManager.Instance.UseShuffle());
            addSlotBtn.onClick.AddListener(() => ToolManager.Instance.UseAddSlot());
//            hintBtn.onClick.AddListener(() => ToolManager.Instance.UseHint());
            returnBtn.onClick.AddListener(() => ToolManager.Instance.UseReturn());
        }

        private void OnDestroy()
        {
            TileEventBus.OnToolUsed -= OnToolUsed;
        }

        private void OnToolUsed(string commandStr, int useLeft)
        {
            switch (commandStr)
            {
                case "Shuffle":
                    UpdateLabel(shuffleText, useLeft);
                    shuffleBtn.interactable = useLeft != 0;
                    break;
                case "AddSlot":
                    UpdateLabel(addSlotText, useLeft);
                    addSlotBtn.interactable = useLeft != 0;
                    break;
                // case "Hint":
                //     UpdateLabel(hintText, useLeft);
                //     hintBtn.interactable = useLeft != 0;
                //     break;
                case "Return":
                    UpdateLabel(returnText, useLeft);
                    returnBtn.interactable = useLeft != 0;
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