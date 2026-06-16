using _Scripts.Core.Tile;
using _Scripts.Data;
using _Scripts.Managers;
using _Scripts.SaveSystem;
using _Scripts.Utils.Event_Bus;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Core.Tutorial_Session
{
    public class TutorialManager : LevelManager
    {
        [SerializeField] private RectTransform handPointer;
        [SerializeField] private Button skipButton;
        private int _index = 0;
        private TileCell _cell;

        private void OnEnable()
        {
            EventBus.Subscribe<ClickedHintedTileCellEvent>(OnHintCellClicked);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<ClickedHintedTileCellEvent>(OnHintCellClicked);
        }

        protected override void Awake()
        {
            base.Awake();
            if (DataSystem.IsTutorialDone)
            {
                Finish();
                return;
            }

            skipButton.onClick.AddListener(Finish);
            EventBus.Subscribe<LevelClearedEvent>(OnCleared);
        }

        protected override void Start()
        {
            if (DataSystem.IsTutorialDone)
                return;
            LoadNextTutorial();
            HintStep();
        }

        private void OnCleared(LevelClearedEvent levelClearedEvent)
        {
            handPointer.gameObject.SetActive(false);
            LoadNextTutorial();
        }


        private void LoadNextTutorial()
        {
            _index++;
            if (_index > 2)
            {
                Finish();
                return;
            }

            LoadLevel(_index);
        }

        private void OnDestroy()
        {
            skipButton.onClick.RemoveListener(Finish);
            EventBus.Unsubscribe<LevelClearedEvent>(OnCleared);
        }

        public override void LoadLevel(int level)
        {
            CurrentGrid = LevelSaveSystem.LoadLevel(
                level, true, defaultWidth + 1, defaultHeight + 1, defaultLayers);

            if (_gridView != null)
                _gridView.LoadGrid(CurrentGrid);

            RefreshView();
        }

        private void Finish()
        {
            DataSystem.MarkDone();
            EventBus.Publish(new LoadSceneEvent(SceneType.MapScene));
        }

        private void HintStep()
        {
            _cell = spawner.GetHintTileCells();
            handPointer.DOKill();

            if (_cell == null)
            {
                handPointer.gameObject.SetActive(false);
                return;
            }

            handPointer.gameObject.SetActive(true);
            SetHandPointerPosition(_cell.transform.position);
            AnimationHandPointer();
        }

        private void SetHandPointerPosition(Vector3 worldPos)
        {
            Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                handPointer.parent as RectTransform,
                screenPos,
                null,
                out Vector2 localPoint
            );

            handPointer.localPosition = localPoint;
        }

        private void OnHintCellClicked(ClickedHintedTileCellEvent hintCellEvent)
        {
            HintStep();
        }

        private void AnimationHandPointer()
        {
            handPointer.localRotation = Quaternion.identity;
            handPointer.DORotate(new Vector3(0, 0, -10), 0.4f)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}