using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class BaseShellManager : MonoBehaviour
{
    public static BaseShellManager Instance;

    public List<Transform> slots = new List<Transform>();
    public List<TileCell> tilesInShell = new List<TileCell>();
    Dictionary<int, List<int>> similarTilesID = new Dictionary<int, List<int>>(); // List(nums of tiles, first Position)

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

        int index;
        int id = tile.ID;
        if (!similarTilesID.ContainsKey(id))
        {
            if (tilesInShell.Count == 0) index = 0;
            else index = tilesInShell.Count;
            similarTilesID[id] = new List<int>()
            {
                1,
                index
            };

            tilesInShell.Add(tile);
        }
        else
        {
            index = similarTilesID[id][0] + similarTilesID[id][1];
            similarTilesID[id][0]++;
            tilesInShell.Insert(index, tile);
        }

        for (int i = 0; i < tilesInShell.Count; i++)
        {
            if (index > i - 1) continue;

            tilesInShell[i].transform.DOMove(slots[i].position, 0.5f).SetEase(Ease.OutExpo);
            similarTilesID[tilesInShell[i].ID][1]++;
        }

        tile.transform.DOMove(slots[index].position, 0.5f).SetEase(Ease.Linear);

        // similarTilesID[id].Add(index);

        CheckMatch(id);
    }

    void CheckMatch(int id)
    {
        if (!similarTilesID.TryGetValue(id, out var similarID)) return;
        if (similarID[0] < 3) return;
        int indexOfFirstOne = similarID[1];
        RemoveMatch(indexOfFirstOne, id);
    }

    void ChangeIndex(TileCell cell, int num)
    {
    }

    void RemoveMatch(int index, int id)
    {
        for (int i = 0; i < 3; i++)
        {
            TileCell tile = tilesInShell[index];
            tile.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f), 0.5f, 3, 0.8f);
            tilesInShell.RemoveAt(index);
            Destroy(tile.gameObject, 1f);
        }

        // foreach (int i in indexes)
        // {
        //     Vector3 targetPosition = slots[targetIndex].position + new Vector3(0, 1, 0);
        //     targetIndex--;
        //     TileCell tile = tilesInShell[i];
        //     tile.transform.DOMove( /*tile.transform.position + new Vector3(0, 2, 0)*/ targetPosition, 0.5f)
        //         .OnComplete(() => { Destroy(tile.gameObject); });
        //     tilesInShell.RemoveAt(i);
        // }

        similarTilesID.Remove(id);
        RebuildDictionary();
        SlideTiles();
    }

    void SlideTiles()
    {
        for (int i = 0; i < tilesInShell.Count; i++)
        {
            tilesInShell[i].transform.DOMove(slots[i].position, 0.5f).SetEase(Ease.OutExpo).SetDelay(1.2f);
        }
    }

    void RebuildDictionary()
    {
        similarTilesID.Clear();
        for (int i = 0; i < tilesInShell.Count; i++)
        {
            int id = tilesInShell[i].ID;
            int index;
            if (!similarTilesID.ContainsKey(id))
            {
                if (tilesInShell.Count == 0) index = 0;
                else index = tilesInShell.Count;
                similarTilesID[id] = new List<int>()
                {
                    1,
                    i
                };
            }
            else
            {
                index = similarTilesID[id][0] + similarTilesID[id][1] - 3;
            }
        }
    }
}