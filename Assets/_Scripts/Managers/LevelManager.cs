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
        private GridSpawner spawner;

        [SerializeField] private ShellManager shellManager;
        [Header("Settings")] [SerializeField] private int defaultWidth = 8;
        [SerializeField] private int defaultHeight = 8;
        [SerializeField] private int defaultLayers = 5;

        public TileGrid CurrentGrid { get; private set; }
        private GridView _gridView;
        public ITileValidator RuleValidator { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            RuleValidator = new StaggerGridValidator();
            _gridView = FindObjectOfType<GridView>();
        }

        void Start()
        {
            LoadLevel(0);
            ToolManager.Instance.Initialize(spawner, shellManager);
        }

        public void LoadLevel(int levelIndex)
        {
            CurrentGrid = LevelSaveSystem.LoadLevel(
                levelIndex, defaultWidth + 1, defaultHeight + 1, defaultLayers);

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
                savedLevel, defaultWidth + 1, defaultHeight + 1, defaultLayers);

            if (_gridView != null)
                _gridView.LoadGrid(CurrentGrid);

            RefreshView();
            Debug.Log($"[LevelManager] Level {savedLevel} saved & reloaded.");
#endif
        }

        // public void ClearLevel()
        // {
        //     spawner.Clear();
        //     CurrentGrid = null;
        // }

        private void RefreshView()
        {
            spawner.Clear();
            spawner.Spawn(CurrentGrid);
        }
    }
}