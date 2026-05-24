using System.Collections.Generic;
using _Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private LevelManager levelManager;
    
    private List<int> levelIndices = new List<int>();

    void Start()
    {
        
        if (dropdown == null || levelManager == null)
        {
            Debug.LogError("[LevelSelector]: dropdown and/or levelManager are missing");
            return;
        }

        PopulateDropdown();
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        if (levelIndices.Count > 0)
            levelManager.LoadLevel(levelIndices[0]);
    }

    private void OnDestroy()
    {
        if (dropdown != null)
            dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
    }


    private void PopulateDropdown()
    {
        levelIndices.Clear();
        dropdown.ClearOptions();

        var options = new List<string>();
        int index = 0;

        while (true)
        {
            TextAsset asset = Resources.Load<TextAsset>($"Levels/level{index}");
            if (asset == null) break;

            levelIndices.Add(index);
            options.Add($"Level {index}");
            index++;
        }

        if (options.Count == 0)
        {
            Debug.LogWarning("[LevelSelector] Không tìm thấy level nào trong Resources/Levels");
            return;
        }
        options.Add("Add new level");
        dropdown.AddOptions(options);
        dropdown.value = 0;
        dropdown.RefreshShownValue();

        Debug.Log($"[LevelSelector] Loaded {options.Count} level(s) into dropdown");
    }


    public void AddNewLevel()
    {
        int newIndex = levelIndices.Count > 0
            ? levelIndices[levelIndices.Count - 1] + 1
            : 0;

        levelManager.CreateAndSaveNewLevel(newIndex);

        levelIndices.Add(newIndex);
        TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData($"Level {newIndex}");
        dropdown.options.Insert(newIndex, optionData);

        dropdown.value = levelIndices.Count - 1;
        dropdown.RefreshShownValue();
    }


    public void RefreshDropdown()
    {
        int prev = dropdown.value;
        PopulateDropdown();

        if (prev < levelIndices.Count)
        {
            dropdown.value = prev;
            dropdown.RefreshShownValue();
        }
    }

    private void OnDropdownValueChanged(int dropdownIndex)
    {
        if (dropdownIndex < 0 || dropdownIndex >= levelIndices.Count+1) return;
        if(dropdownIndex == levelIndices.Count) AddNewLevel();
        levelManager.LoadLevel(levelIndices[dropdownIndex]);
    }

    public void SaveCurrentLevel() => levelManager.SaveCurrentLevel();
    public void ClearCurrentLevel() => levelManager.CreateAndSaveNewLevel(levelIndices[dropdown.value]);
}