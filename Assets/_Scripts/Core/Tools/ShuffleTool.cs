using System;
using _Scripts.Data;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace _Scripts.Core.Tools
{
    /// <summary>
    /// shuffle base on active cells in scene and reload ( or respawn base on grid view) or level
    /// because when click on cell -> value of that cell in Array return to 0
    /// but currently when spawn it check number of TileCell that not include in
    /// base Shell so that can invalid to random.
    /// </summary>
    public class ShuffleTool : IToolCommand
    {
        private GridSpawner _gridSpawner;
        private int _useLeft;

        public ShuffleTool(GridSpawner gridSpawner, int useLeft)
        {
            _gridSpawner = gridSpawner;
            _useLeft = useLeft;
        }

        public bool CanExecute()
        {
            return _useLeft > 0 && _gridSpawner != null;
        }

        public int UseLeft => _useLeft;

        public void Execute()
        {
            if (!CanExecute())
                return;
            // Implement business login here
            var tiles = _gridSpawner.ActiveCells;

            for (int i = tiles.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);

                Vector3Int pos1 = tiles[i].GridPos();
                Vector3Int pos2 = tiles[j].GridPos();
                
                Vector3 worldPos1 = tiles[i].worldPos;
                Vector3 worldPos2 = tiles[j].worldPos;

                tiles[i].SetupCell(pos2.x, pos2.y, pos2.z, worldPos2);
                tiles[j].SetupCell(pos1.x, pos1.y, pos1.z, worldPos1);

                (tiles[i], tiles[j]) = (tiles[j], tiles[i]);
            }

            foreach (var tile in tiles)
            {
                tile.MoveToWorldPosition(tile.worldPos);
            }
            _gridSpawner.RefreshAllCells(_gridSpawner.CurrentGrid);
            if (_useLeft > 0) _useLeft--;
            TileEventBus.OnToolUsed?.Invoke(ToolType.Shuffle, _useLeft);
        }
    }
}