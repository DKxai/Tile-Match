using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utils;


public class ShellManager : Singleton<ShellManager>
{

    public List<Transform> slots = new List<Transform>();
    public List<TileCell> tilesInShell = new List<TileCell>();
    Dictionary<int, List<int>> similarTiles = new Dictionary<int, List<int>>(); // List(nums of tiles, first Position)
    private List<TileCell> destroyingCellS = new List<TileCell>();
    bool isDestroying = false;
    int fullSlots = 0;
    int filledSlots = 0;

    private void OnEnable() => TileEventBus.OnTileClicked += AddTile;
    private void OnDisable() => TileEventBus.OnTileClicked -= AddTile;

    public void AddTile(TileCell tile)
    {
        // Debug.Log($"1. Destroying Process is :{isDestroying}");
        // Debug.Log($"1.Destroying Cell is {destroyingCellS.Count} + tile in shell {tilesInShell.Count}");
        fullSlots = isDestroying ? slots.Count : slots.Count - 1;
        filledSlots = tilesInShell.Count /*+ destroyingCellS.Count*/;
        Debug.Log($"1. Adding Tile {tile.ID} filled slots:{filledSlots}  full slots:{fullSlots}");
        //Debug.Log($"1. filled slots:{filledSlots}  full slots:{fullSlots}");
        if (filledSlots >= fullSlots)
        {
            Debug.Log("[BaseShellManager] Shell full!");
            return;
        }

        int index;
        InsertIntoShell(tile, out index);

        MoveRightOtherTiles(index, out var sequence);

        sequence.Join(tile.transform.DOMove(slots[index].position, 0.35f).SetEase(Ease.OutExpo));

        sequence.OnComplete(() => { CheckMatching(tile.ID); });
    }

    private void MoveRightOtherTiles(int index, out Sequence sequence)
    {
        bool haveSameTile = false;
        sequence = DOTween.Sequence();
        for (int i = 0; i < tilesInShell.Count; i++)
        {
            if (i <= index) continue;

            sequence.Join(tilesInShell[i].transform.DOMove(slots[i].position, 0.35f).SetEase(Ease.OutExpo));
            if (similarTiles[tilesInShell[i].ID][0] == 2 && haveSameTile)
            {
                haveSameTile = false;
                continue;
            }

            similarTiles[tilesInShell[i].ID][1]++;
            haveSameTile = true;
        }
    }

    private void InsertIntoShell(TileCell tile, out int index)
    {
        int id = tile.ID;
        if (!similarTiles.ContainsKey(id))
        {
            if (tilesInShell.Count == 0)
            {
                index = 0;
            }
            else index = filledSlots;

            similarTiles[id] = new List<int>()
            {
                1,
                index
            };
        }
        else
        {
            index = similarTiles[id][0] + similarTiles[id][1];

            similarTiles[id][0]++;

            Debug.Log($"2. Insert {id} into {index} +  tile in shell {tilesInShell.Count} + full slot {fullSlots}");
        }

        tilesInShell.Insert(index, tile);
    }

    void CheckMatching(int id)
    {
        if (!similarTiles.TryGetValue(id, out var similarID)) return;
        if (similarID[0] < 3) return;
        isDestroying = true;
        int indexOfFirstOne = similarID[1];
        // Debug.Log($"3. Destroying Process is :{isDestroying}");
        //
        // Debug.Log($"3. Destroying Cell {destroyingCellS.Count}");
        Debug.Log($"3. Checking Matching");
        RemoveMatching(indexOfFirstOne, id);
    }

    void RemoveMatching(int index, int id)
    {
        for (int i = 0; i < 3; i++)
        {
            destroyingCellS.Add(tilesInShell[index + i]);
        }

        Debug.Log($"4. Removing {id}");
        // Debug.Log($"4. Destroying Cell {destroyingCellS.Count}");
        // Debug.Log($"4. Destroying Process is :{isDestroying}");


        Sequence sq = DOTween.Sequence();
        foreach (var tileCell in destroyingCellS)
        {
            tileCell.transform.DOKill();
            sq.Join(tileCell.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f), 0.5f, 3, 0.8f)
                .OnComplete(() =>
                {
                    tilesInShell.Remove(tileCell);
                    Destroy(tileCell.gameObject);
                    destroyingCellS.Remove(tileCell);
                    //Debug.Log($"5. Destroying Process is :{isDestroying}");
                    Debug.Log($"4. Destroy cells  ID {id}: Done!");
                }));
        }

        sq.AppendCallback(() => similarTiles.Remove(id));
        sq.OnComplete(() =>
        {
            RebuildDictionary();
            // Debug.Log($"7. Destroying Cell {destroyingCellS.Count}");

            isDestroying = false;
            //Debug.Log($"7. Destroying Process is :{isDestroying}");
            MoveLeftOtherTiles();
        });
    }

    void MoveLeftOtherTiles()
    {
        Debug.Log($"6.. MoveLeftOtherTiles");
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
            if (!similarTiles.TryGetValue(id, out var similarTile))
            {
                similarTiles[id] = new List<int>()
                {
                    1,
                    i
                };
            }
            else
            {
                similarTile[0]++;
            }
        }

        Debug.Log($"5.. Rebuild Dictionary is {similarTiles.Count}");
    }
}