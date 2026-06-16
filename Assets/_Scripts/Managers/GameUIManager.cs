using System;
using System.Collections.Generic;
using _Scripts.Core.Tile;
using _Scripts.Data;
using _Scripts.Data.EndingUI;
using _Scripts.Data.Sounds;
using _Scripts.Data.Tool;
using _Scripts.SaveSystem;
using _Scripts.Systems;
using _Scripts.UI;
using _Scripts.UI.Tools_Confirm;
using _Scripts.Utils.Event_Bus;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EventBus = _Scripts.Utils.Event_Bus.EventBus;

namespace _Scripts.Managers
{
    public class GameUIManager : UIManager
    {
        [SerializeField] private Button homeButton;
        [SerializeField] private TMP_Text titleText;

        [SerializeField] private List<EndingInfoMapping> _endingData;
        [SerializeField] private EndingUI _endingUI;

        private int CurrentLevel => DataSystem.LoadSelectedLevel();

        protected override void Awake() 
        {
            base.Awake();
            homeButton.onClick.AddListener(OnHomeClicked);
        }

        protected override void OnDestroy() 
        {
            base.OnDestroy();
            homeButton.onClick.RemoveListener(OnHomeClicked);
        }

        private void OnEnable()
        {
            EventBus.Subscribe<OutOfToolUseEvent>(ConfirmPopupShow);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<OutOfToolUseEvent>(ConfirmPopupShow);
            if (ShellManager.Instance != null)
                ShellManager.Instance.openEndingUI -= OpenEndingUI;
        }


        private void Start()
        {
            titleText.text = "Level " + CurrentLevel;
            ShellManager.Instance.openEndingUI += OpenEndingUI;
        }

        private void OnHomeClicked() =>
            ConfirmPopupShow(new OutOfToolUseEvent(ToolType.Quit));

        #region EndingUI

        private void OpenEndingUI(EndingType endingType)
        {
            Action rAction = endingType == EndingType.Lose ? OnQuit : OnContinue;
            Action lAction = endingType == EndingType.Lose ? OnRetry : OnQuit;
            SoundType soundType = endingType == EndingType.Lose ? SoundType.LoseSound : SoundType.WinSound;
            _endingUI.InitEndingUI(GetEndingInfo(endingType), rAction, lAction);
            _endingUI.Show();
            EventBus.Publish(new PlaySoundEvent(soundType));
            if (endingType == EndingType.Win)
            {
                _endingUI.CoinsAward();
                int nextLevel = CurrentLevel + 1;
                if (nextLevel > DataSystem.LoadCurrentLevel())
                    DataSystem.SaveCurrentLevel(nextLevel);
                DataSystem.SaveSelectedLevel(nextLevel);
            }
        }

        private EndingInfo GetEndingInfo(EndingType endingType) =>
            _endingData.Find(x => x.EndingType == endingType).EndingInfo;

        private void OnRetry()
        {
            HeartManager.Instance.SpendHeart();
            EventBus.Publish(new LoadSceneEvent(SceneType.GameScene));
        }

        private void OnQuit()
        {
            EventBus.Publish(new LoadSceneEvent(SceneType.MapScene));
        }

        private void OnContinue()
        {
            EventBus.Publish(new LoadSceneEvent(SceneType.GameScene));
        }

        #endregion
    }
}