using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class BaseShellManager : MonoBehaviour
{
    public static BaseShellManager Instance;

    public List<Transform> slots = new List<Transform>();
    public List<TileCell> tilesInShell = new List<TileCell>();
    Dictionary<int, List<int>> similarTilesID = new Dictionary<int, List<int>>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddTile(TileCell tile)
    {
        if (tilesInShell.Count >= 7)
        {
            Debug.Log("Shell full!");
            return;
        }

        tilesInShell.Add(tile);

        int index = tilesInShell.Count - 1;
        tile.transform.DOMove(slots[index].position, 0.5f).SetEase(Ease.Linear);

        int id = tile.tiles[tile.random].id;
        if (!similarTilesID.ContainsKey(id))
        {
            similarTilesID[id] = new List<int>();
        }

        similarTilesID[id].Add(index);

        CheckMatch(id);
    }

    void CheckMatch(int id)
    {
        if (!similarTilesID.ContainsKey(id)) return;
        if (similarTilesID[id].Count < 3) return;
        List<int> indexes = similarTilesID[id];
        RemoveMatch(indexes, id);
    }

    void RemoveMatch(List<int> indexes, int id)
    {
        indexes.Sort();
        indexes.Reverse();
        int targetIndex = 4;
        foreach (int i in indexes)
        {
            Vector3 targetPosition = slots[targetIndex].position + new Vector3(0, 1, 0);
            targetIndex--;
            TileCell tile = tilesInShell[i];
            tile.transform.DOMove( /*tile.transform.position + new Vector3(0, 2, 0)*/ targetPosition, 0.5f)
                .OnComplete(() => { Destroy(tile.gameObject); });
            tilesInShell.RemoveAt(i);
        }

        similarTilesID.Remove(id);
        RebuildDictionary();
        SlideTiles();
    }

    void SlideTiles()
    {
        for (int i = 0; i < tilesInShell.Count; i++)
        {
            tilesInShell[i].transform.DOMove(slots[i].position, 0.5f).SetEase(Ease.OutExpo);
        }
    }

    void RebuildDictionary()
    {
        similarTilesID.Clear();
        for (int i = 0; i < tilesInShell.Count; i++)
        {
            int id = tilesInShell[i].tiles[tilesInShell[i].random].id;
            if (!similarTilesID.ContainsKey(id))
            {
                similarTilesID[id] = new List<int>();
            }

            similarTilesID[id].Add(i);
        }
    }
}