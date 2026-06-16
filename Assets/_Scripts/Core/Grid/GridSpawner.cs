using System.Collections.Generic;
using _Scripts.Core.Tile;
using _Scripts.Utils;
using _Scripts.Utils.Event_Bus;
using Grid_Map;
using UnityEngine;

namespace _Scripts.Core.Grid
{
    public class GridSpawner : MonoBehaviour
    {
        [SerializeField] private float cellSize = 0.52f;
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private Transform origin;
        [SerializeField] private TileSpawner tileSpawner;

        private readonly List<TileCell> _activeCells = new List<TileCell>();
        private TileGrid _currentGrid;
        private readonly Dictionary<Vector3Int, TileCell> _cellMap = new Dictionary<Vector3Int, TileCell>();
        public TileSpawner TileSpawner => tileSpawner;
        public List<TileCell> ActiveCells => _activeCells;
        public TileGrid CurrentGrid => _currentGrid;
        private void OnEnable() => EventBus.Subscribe<TileClickEvent>(OnTileClicked);
        private void OnDisable() => EventBus.Unsubscribe<TileClickEvent>(OnTileClicked);
        public List<TileCell> CellsForHint = new List<TileCell>();
        private int _cellHintedID = -1;
        private int _hintStep = 0;

        private void OnTileClicked(TileClickEvent e)
        {
            TileCell cell = e.TileCell;
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
            _activeCells.Clear();
            for (int z = 0; z < grid.Layers; z++)
            {
                Vector3 newOrigin =
                        z % 2 == 0 ? origin.position : origin.position + new Vector3(0.5f, 0.5f, 0f) * cellSize
                    ;
                for (int y = grid.Height - 1; y >= 0; y--)
                {
                    for (int x = 0; x < grid.Width; x++)
                    {
                        int value = grid.GetValue(x, y, z);
                        if (value != 0)
                        {
                            Vector3 worldPos = UtilsClass.GetWorldPosition(newOrigin, x, y, z, cellSize);
                            worldPos.x -= x * 0.02f;
                            GameObject obj = Instantiate(cellPrefab, worldPos, Quaternion.identity, origin);
                            TileCell cell = obj.GetComponent<TileCell>();
                            if (cell != null)
                            {
                                cell.SetupCell(x, y, z, worldPos);
                                _activeCells.Add(cell);
                                _cellMap[new Vector3Int(x, y, z)] = cell;
                            }
                        }
                    }
                }
            }

            if (tileSpawner != null)
                tileSpawner.Init(_activeCells);

            RefreshAllCells(grid);
        }

        public void RefreshAllCells(TileGrid grid)
        {
            CellsForHint.Clear();
            foreach (var cell in _activeCells)
            {
                if (cell != null)
                {
                    cell.RefreshState(grid);
                    if (!cell.IsBlocked)
                        CellsForHint.Add(cell);
                }
            }
        }

        void RefreshAffectedCells(TileCell cell)
        {
            if (_currentGrid == null) return;

            for (int layerBelow = cell.gridZ + 1; layerBelow < _currentGrid.Layers; layerBelow++)
            {
                foreach (var pos in GetAffectedPositions(cell.gridX, cell.gridY, layerBelow))
                {
                    var key = new Vector3Int(pos.x, pos.y, layerBelow);
                    if (_cellMap.TryGetValue(key, out TileCell tileCell) && tileCell != null)
                    {
                        tileCell.RefreshState(_currentGrid);
                        if (!tileCell.IsBlocked)
                            CellsForHint.Add(tileCell);
                    }
                }
            }
        }

        public void RebuildCellMap()
        {
            _cellMap.Clear();

            foreach (var cell in _activeCells)
            {
                if (cell == null) continue;

                _cellMap[new Vector3Int(cell.gridX, cell.gridY, cell.gridZ)] = cell;
            }
        }

        private void RemoveCell(TileCell cell)
        {
            _activeCells.Remove(cell);
            _cellMap.Remove(new Vector3Int(cell.gridX, cell.gridY, cell.gridZ));
        }

        private List<Vector2Int> GetAffectedPositions(int gridX, int gridY, int targetLayer)
        {
            List<Vector2Int> positions = new List<Vector2Int>();

            if (targetLayer % 2 == 0)
            {
                positions.Add(new Vector2Int(gridX, gridY));
                positions.Add(new Vector2Int(gridX + 1, gridY));
                positions.Add(new Vector2Int(gridX, gridY + 1));
                positions.Add(new Vector2Int(gridX + 1, gridY + 1));
            }
            else
            {
                positions.Add(new Vector2Int(gridX, gridY));
                positions.Add(new Vector2Int(gridX - 1, gridY));
                positions.Add(new Vector2Int(gridX, gridY - 1));
                positions.Add(new Vector2Int(gridX - 1, gridY - 1));
            }

            return positions;
        }

        public void Clear()
        {
            _cellHintedID = -1;
            CellsForHint.Clear();
            _activeCells.Clear();
            _cellMap.Clear();
            _currentGrid = null;

            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        public TileCell GetHintTileCells()
        {
            if (CellsForHint.Count == 0) return null;

            TileCell cell;
            if (_hintStep % 3 == 0)
            {
                int rand = Random.Range(0, CellsForHint.Count);
                cell = CellsForHint[rand];
                _cellHintedID = cell.ID;

                foreach (var c in CellsForHint)
                {
                    if (c != null && c != cell)
                        c.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
            else
            {
                cell = CellsForHint.Find(x => x != null && x.ID == _cellHintedID);
                if (cell == null) return null;
            }

            cell.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            _hintStep++;
            cell.isHintTileCell = true;
            CellsForHint.Remove(cell);
            return cell;
        }
    }
}