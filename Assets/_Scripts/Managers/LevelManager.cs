using UnityEngine;
using Grid_Map;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridSpawner spawner;
    [SerializeField] private GridView gridView;

    [Header("Settings")]
    [SerializeField] private int defaultWidth = 8;
    [SerializeField] private int defaultHeight = 8;
    [SerializeField] private int defaultLayers = 5;

    public TileGrid CurrentGrid { get; private set; }

    void Start()
    {
        LoadLevel(0);
    }
    public void LoadLevel(int levelIndex)
    {
        CurrentGrid = LevelSaveSystem.LoadLevel(
            levelIndex, defaultWidth + 1, defaultHeight + 1, defaultLayers);

        if (gridView != null)
            gridView.LoadGrid(CurrentGrid);

        RefreshView();
    }

    // =========================
    // Tạo level mới rỗng và lưu ngay
    // =========================
    public void CreateAndSaveNewLevel(int levelIndex)
    {
        // Tạo TileGrid rỗng với kích thước mặc định
        CurrentGrid = new TileGrid(levelIndex, defaultWidth + 1, defaultHeight + 1, defaultLayers);

        LevelSaveSystem.SaveLevel(CurrentGrid);

#if UNITY_EDITOR
        LevelSaveSystem.ExportToResources(levelIndex);
        Debug.Log($"[LevelManager] New level {levelIndex} created & exported.");
#endif

        if (gridView != null)
            gridView.LoadGrid(CurrentGrid);

        
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

        if (gridView != null)
            gridView.LoadGrid(CurrentGrid);

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