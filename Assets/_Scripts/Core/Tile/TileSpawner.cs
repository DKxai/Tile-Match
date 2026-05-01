using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] List<TileModel> tiles;
    [SerializeField] List<TileCell> cells = new List<TileCell>();
    private TileCell[] _tileCells;
    private bool _isValidNumberOfTileCell = false;
    private int _cellCounter;

    public void Init()
    {
        _tileCells = GetComponentsInChildren<TileCell>();

        cells = new List<TileCell>(_tileCells);

        _isValidNumberOfTileCell = CheckNumberOfTileCell();
        if (!_isValidNumberOfTileCell) return;

        RandomSystem();
    }

    private bool CheckNumberOfTileCell()
    {
        _cellCounter = cells.Count;
        if (_cellCounter < 3 || _cellCounter % 3 != 0)
        {
            Debug.Log("[TileSpawner] Invalid tile cell numbers.");
            return false;
        }

        return true;
    }

    private void RandomSystem()
    {
        int randomTimes = _cellCounter / 3;

        for (int i = 0; i < randomTimes; i++)
        {
            int randomTile = Random.Range(0, tiles.Count);
            for (int x = 0; x < 3; x++)
            {
                int randomCell = Random.Range(0, cells.Count);
                cells[randomCell].iconSprite.GetComponent<SpriteRenderer>().sprite = tiles[randomTile].iconSprite;
                cells[randomCell].ID = tiles[randomTile].id;
                Debug.Log(
                    $"[TileSpawner] {cells[randomCell].gameObject.name} Added icon sprite with ID {cells[randomCell].ID}.");

                Debug.Log($"[TileSpawner] remove {cells[randomCell].gameObject.name}th from List.");
                cells.Remove(cells[randomCell]);
                Debug.Log($"[TileSpawner] {cells.Count} remaining cells in List.");
            }
        }
    }
}