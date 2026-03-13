using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class BaseShellManager : MonoBehaviour
{
    public static BaseShellManager Instance;

    public List<Transform> slots = new List<Transform>();
    public List<TileCell> tilesInShell = new List<TileCell>();
    Dictionary<int, List<int>> similarTiles = new Dictionary<int, List<int>>(); // List(nums of tiles, first Position)
    private List<TileCell> destroyingCellS = new List<TileCell>();
    bool isDestroying = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddTile(TileCell tile)
    {
        Debug.Log($"1. Destroying Process is :{isDestroying}");
        Debug.Log($"Destroying Cell is {destroyingCellS.Count} + tile in shell {tilesInShell.Count}");
        int fullSlots = isDestroying ? slots.Count : slots.Count - 1;
        int filledSlot = tilesInShell.Count /*+ destroyingCellS.Count*/;
        Debug.Log($"filled slots:{filledSlot}  full slots:{fullSlots}");
        if (filledSlot >= fullSlots)
        {
            Debug.Log("Shell full!");
            return;
        }

        int index;
        int id = tile.ID;
        if (!similarTiles.ContainsKey(id))
        {
            if (tilesInShell.Count == 0) index = 0;
            // else index = tilesInShell.Count;
            else index = filledSlot;
            similarTiles[id] = new List<int>()
            {
                1,
                index
            };

            tilesInShell.Add(tile);
        }
        else
        {
            index = similarTiles[id][0] + similarTiles[id][1];

            similarTiles[id][0]++;
            Debug.Log($"index of Added Tiled is {index} +  tile in shell {tilesInShell.Count} + full slot {fullSlots}");
            tilesInShell.Insert(index, tile);
        }

        int counterFor2SimilarTiles = 0;
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < tilesInShell.Count; i++)
        {
            if (i <= index) continue;

            sequence.Join(tilesInShell[i].transform.DOMove(slots[i].position, 0.35f).SetEase(Ease.OutExpo));
            if (similarTiles[tilesInShell[i].ID][0] == 2 && counterFor2SimilarTiles == 1)
            {
                counterFor2SimilarTiles = 0;
                continue;
            }

            similarTiles[tilesInShell[i].ID][1]++;
            counterFor2SimilarTiles++;
        }

        sequence.Join(tile.transform.DOMove(slots[index].position, 0.35f).SetEase(Ease.OutExpo));

        sequence.OnComplete(() => { CheckMatch(id); });
    }

    void CheckMatch(int id)
    {
        if (!similarTiles.TryGetValue(id, out var similarID)) return;
        if (similarID[0] < 3) return;
        isDestroying = true;
        int indexOfFirstOne = similarID[1];
        Debug.Log($"2. Destroying Process is :{isDestroying}");

        RemoveMatch(indexOfFirstOne, id);
        Debug.Log($"2. Destroying Cell {destroyingCellS.Count}");
    }

    void RemoveMatch(int index, int id)
    {
        for (int i = 0; i < 3; i++)
        {
            destroyingCellS.Add(tilesInShell[index + i]);
        }

        Debug.Log($"2. Destroying Cell {destroyingCellS.Count}");
        Debug.Log($"Destroying Process is :{isDestroying}");


        Sequence sq = DOTween.Sequence();
        foreach (var tileCell in destroyingCellS)
        {
            tileCell.transform.DOKill();
            sq.Join(tileCell.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f), 0.5f, 3, 0.8f)
                .OnComplete(() =>
                {
                    Destroy(tileCell.gameObject);
                    Debug.Log($"3. Destroying Process is :{isDestroying}");

                    destroyingCellS.Remove(tileCell);
                    tilesInShell.Remove(tileCell);
                }));
        }

        sq.OnComplete(() =>
        {
            similarTiles.Remove(id);
            Debug.Log($"2. Destroying Cell {destroyingCellS.Count}");

            RebuildDictionary();
            isDestroying = false;
            Debug.Log($"4. Destroying Process is :{isDestroying}");
            SlideTiles();
        });
    }

    void SlideTiles()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(0.1f);
        for (int i = 0; i < tilesInShell.Count; i++)
        {
            sequence.Join(
                tilesInShell[i].transform.DOMove(slots[i].position, 0.35f).SetEase(Ease.OutExpo));
        }
    }

    void RebuildDictionary()
    {
        similarTiles.Clear();
        for (int i = 0; i < tilesInShell.Count; i++)
        {
            int id = tilesInShell[i].ID;
            if (!similarTiles.ContainsKey(id))
            {
                similarTiles[id] = new List<int>()
                {
                    1,
                    i
                };
            }
            else
            {
                similarTiles[id][0]++;
            }
        }
    }
}