using System;
using System.Collections;
using System.Collections.Generic;
using Grid_Map;
using UnityEngine;
using Utils;

public class GridSpawner : MonoBehaviour
{
    [SerializeField] private float cellSize = 0.52f;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform origin;
    [SerializeField] private TileSpawner tileSpawner;

    public List<TileCell> activeCells = new List<TileCell>();
    private TileGrid _currentGrid;
    private Dictionary<Vector3Int, TileCell> _cellMap = new Dictionary<Vector3Int, TileCell>();

    private void OnEnable() => TileEventBus.OnTileClicked += HandleTileClicked;
    private void OnDisable() => TileEventBus.OnTileClicked -= HandleTileClicked;

    private void HandleTileClicked(TileCell cell)
    {
        if (_currentGrid != null)
        {
            _currentGrid.SetValue(cell.gridX, cell.gridY, cell.gridZ, 0);
        }
        RefreshAffectedCells(cell);
        RemoveCell(cell);
    }

    public void Spawn(TileGrid grid)
    {
        _currentGrid = grid;
        _cellMap.Clear();
        activeCells.Clear();
        for (int z = 0; z < grid.Layers; z++)
        {
            Vector3 newOrigin = z % 2 == 0 ? origin.position : origin.position + new Vector3(0.5f, 0.5f, 0f) * cellSize
                ;
            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    int value = grid.GetValue(x, y, z);
                    if (value != 0)
                    {
                        Vector3 worldPos = UtilsClass.GetWorldPosition(newOrigin, x, y, z, cellSize);
                        GameObject obj = Instantiate(cellPrefab, worldPos, Quaternion.identity, origin);
                        TileCell cell = obj.GetComponent<TileCell>();
                        if (cell != null)
                        {
                            cell.SetupCell(x, y, z);
                            activeCells.Add(cell);
                            _cellMap[new Vector3Int(x, y, z)] = cell;
                        }
                    }
                }
            }
        }

        if (tileSpawner != null)
            tileSpawner.Init();

        RefreshAllCells(grid);
    }

    void RefreshAllCells(TileGrid grid)
    {
        foreach (var cell in activeCells)
        {
            if (cell != null)
            {
                cell.RefreshState(grid);
            }
        }
    }

    void RefreshAffectedCells(TileCell cell)
    {
        if (_currentGrid == null) return;

        int layerBelow = cell.gridZ + 1;
        if (layerBelow > _currentGrid.Layers) return;

        foreach (var pos in GetAffectedPositions(cell.gridX, cell.gridY, cell.gridZ))
        {
            var key = new Vector3Int(pos.x, pos.y, layerBelow);
            if (_cellMap.TryGetValue(key, out TileCell tileCell) && tileCell != null)
                tileCell.RefreshState(_currentGrid);
        }
    }

    private void RemoveCell(TileCell cell)
    {
        activeCells.Remove(cell);
        _cellMap.Remove(new Vector3Int(cell.gridX, cell.gridY, cell.gridZ));
    }

    private List<Vector2Int> GetAffectedPositions(int gridX, int gridY, int gridZ)
    {
        List<Vector2Int> positions = new List<Vector2Int>();
        if (gridZ % 2 == 0)
        {
            positions.Add(new Vector2Int(gridX, gridY));
            positions.Add(new Vector2Int(gridX - 1, gridY));
            positions.Add(new Vector2Int(gridX - 1, gridY - 1));
            positions.Add(new Vector2Int(gridX, gridY - 1));
        }
        else
        {
            positions.Add(new Vector2Int(gridX, gridY));
            positions.Add(new Vector2Int(gridX + 1, gridY));
            positions.Add(new Vector2Int(gridX + 1, gridY + 1));
            positions.Add(new Vector2Int(gridX, gridY + 1));
        }

        return positions;
    }

    public void Clear()
    {
        activeCells.Clear();
        _cellMap.Clear();
        _currentGrid = null;

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}