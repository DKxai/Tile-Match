using System.IO;
using Grid_Map;
using UnityEngine;

public static class LevelSaveSystem
{
    private const string FileNameFormat = "level{0}.json";


    private static string GetPersistentPath(int levelIndex)
    {
        return Path.Combine(Application.persistentDataPath, string.Format(FileNameFormat, levelIndex));
    }


    private static string GetResourcePath(int levelIndex)
    {
        return $"Levels/level{levelIndex}";
    }
    
    public static TileGrid LoadLevel(int levelIndex, int defaultWidth = 8, int defaultHeight = 7, int defaultLayers = 5)
    {
        string persistentPath = GetPersistentPath(levelIndex);

        if (File.Exists(persistentPath))
        {
            string json = File.ReadAllText(persistentPath);
            Debug.Log($"[GridSaveSystem] Load from persistent → {persistentPath}");
            return ParseJson(json, levelIndex, defaultWidth, defaultHeight, defaultLayers);
        }

        TextAsset textAsset = Resources.Load<TextAsset>(GetResourcePath(levelIndex));

        if (textAsset != null)
        {
            Debug.Log($"[GridSaveSystem] Load from Resources → level{levelIndex}");
            return ParseJson(textAsset.text, levelIndex, defaultWidth, defaultHeight, defaultLayers);
        }

        Debug.LogWarning($"[GridSaveSystem] Level {levelIndex} not found → create default");
        return new TileGrid(levelIndex, defaultWidth, defaultHeight, defaultLayers);
    }


    public static void SaveLevel(TileGrid grid)
    {
        string path = GetPersistentPath(grid.Level);
        string json = JsonUtility.ToJson(grid.Save(), true);

        File.WriteAllText(path, json);
        Debug.Log($"[GridSaveSystem] Saved → {path}");
    }


    private static TileGrid ParseJson(string json, int levelIndex, int defaultWidth, int defaultHeight, int defaultLayers)
    {
        GridData data = JsonUtility.FromJson<GridData>(json);

        if (data == null)
        {
            Debug.LogError($"[GridSaveSystem] Invalid JSON for level {levelIndex}");
            return new TileGrid(levelIndex, defaultWidth, defaultHeight, defaultLayers);
        }

        return TileGrid.Load(data);
    }

#if UNITY_EDITOR
    public static void ExportToResources(int levelIndex)
    {
        string src = GetPersistentPath(levelIndex);

        if (!File.Exists(src))
        {
            Debug.LogError($"[GridSaveSystem] No runtime file to export: {src}");
            return;
        }

        string dstDir = Path.Combine(Application.dataPath, "Resources/Levels");
        string dst = Path.Combine(dstDir, $"level{levelIndex}.json");

        Directory.CreateDirectory(dstDir);
        File.Copy(src, dst, true);

        UnityEditor.AssetDatabase.Refresh();

        Debug.Log($"[GridSaveSystem] Exported → {dst}");
    }
#endif
}