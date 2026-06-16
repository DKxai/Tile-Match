using System;
using _Scripts.Core.Grid;
using _Scripts.Core.Rules;
using _Scripts.Core.Tile;
using _Scripts.Data;
using _Scripts.SaveSystem;
using Grid_Map;
using UnityEngine;

namespace _Scripts.Managers
{
    public class LevelManager : Singleton<LevelManager>
    {
        [Header("References")] [SerializeField]
        protected GridSpawner spawner;

        [SerializeField] private ShellManager shellManager;
        [Header("Settings")] [SerializeField] protected int defaultWidth = 8;
        [SerializeField] protected int defaultHeight = 8;
        [SerializeField] protected int defaultLayers = 5;
        [SerializeField] private bool spawnTiles = true;
        public TileGrid CurrentGrid { get; protected set; }
        protected GridView _gridView;
        public ITileValidator RuleValidator { get; private set; }
        public event Action OnLevelClear;

        protected override void Awake()
        {
            base.Awake();
            RuleValidator = new StaggerGridValidator();
            _gridView = FindObjectOfType<GridView>();
            
        }

        protected virtual void Start()
        {
            int levelToLoad = PlayerPrefs.GetInt("SelectedLevel", PlayerPrefs.GetInt("CurrentLevel", 1));
            LoadLevel(levelToLoad);
            if (spawnTiles)
                ToolManager.Instance.Initialize(spawner, shellManager);
        }

        public void NotifyLevelClear() => OnLevelClear?.Invoke();

        public virtual void LoadLevel(int level)
        {
            CurrentGrid = LevelSaveSystem.LoadLevel(
                level, false, defaultWidth + 1, defaultHeight + 1, defaultLayers);

            if (_gridView != null)
                _gridView.LoadGrid(CurrentGrid);

            RefreshView();
        }


        public void CreateAndSaveNewLevel(int levelIndex)
        {
            CurrentGrid = new TileGrid(levelIndex, defaultWidth + 1, defaultHeight + 1, defaultLayers);

            LevelSaveSystem.SaveLevel(CurrentGrid);

#if UNITY_EDITOR
            LevelSaveSystem.ExportToResources(levelIndex);
            Debug.Log($"[LevelManager] New level {levelIndex} created & exported.");
#endif

            if (_gridView != null)
                _gridView.LoadGrid(CurrentGrid);


            RefreshView();
        }

        public void SaveCurrentLevel()
        {
            if (CurrentGrid == null)
            {
                Debug.LogWarning("[LevelManager] Nothing to save.");
                return;
            }

            LevelSaveSystem.SaveLevel(CurrentGrid);

#if UNITY_EDITOR
            LevelSaveSystem.ExportToResources(CurrentGrid.Level);

            int savedLevel = CurrentGrid.Level;
            CurrentGrid = LevelSaveSystem.LoadLevel(
                savedLevel, false, defaultWidth + 1, defaultHeight + 1, defaultLayers);

            if (_gridView != null)
                _gridView.LoadGrid(CurrentGrid);

            RefreshView();
            Debug.Log($"[LevelManager] Level {savedLevel} saved & reloaded.");
#endif
        }


        protected void RefreshView()
        {
            ShellManager.Instance.ResetForNewLevel();
            if (spawnTiles)
            {
                spawner.Clear();
                spawner.Spawn(CurrentGrid);
            }
            else
            {
                spawner.Clear();
                if (_gridView != null)
                    _gridView.LoadGrid(CurrentGrid);
            }
        }
    }
}