using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileCellManager : MonoBehaviour
{
    public static TileCellManager Instance;
    public List<Tile> tiles;
    public List<TileCell> cells = new List<TileCell>();

    private bool IsValidNumberOfTileCell = false;
    private int cellCounter;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        foreach (Transform child in transform)
        {
            cells.Add(child.GetComponent<TileCell>());
           // Debug.Log($"{child.name} has been added to List.");
        }

        IsValidNumberOfTileCell = CheckNumberOfTileCell();
        if (!IsValidNumberOfTileCell) return;

        RandomSystem();
    }

    private bool CheckNumberOfTileCell()
    {
        cellCounter = transform.childCount;
        if (cellCounter < 3 || cellCounter % 3 != 0)
        {
           // Debug.Log("Invalid tile cell numbers.");
            return false;
        }

        return true;
    }

    public void RandomSystem()
    {
        int randomTimes = cellCounter / 3;
        // random iconSprite (tile)
        for (int i = 0; i < randomTimes; i++)
        {
            int randomTile = Random.Range(0, tiles.Count);
            for (int x = 0; x < 3; x++)
            {
                int randomCell = Random.Range(0, cells.Count);
                cells[randomCell].iconSprite.GetComponent<SpriteRenderer>().sprite = tiles[randomTile].iconSprite;
                cells[randomCell].ID = tiles[randomTile].id;
               // Debug.Log($"{cells[randomCell].gameObject.name} Added icon sprite with ID {cells[randomCell].ID}.");
                
                // Debug.Log($"remove {cells[randomCell].gameObject.name}th from List.");
                cells.Remove(cells[randomCell]);
                
                // Debug.Log($"{cells.Count} remaining cells in List.");
            }
        }
    }
}