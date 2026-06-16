using System;
using _Scripts.Core.Grid;
using _Scripts.Data;
using _Scripts.Data.Tool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Core.Tools
{
    public class ShuffleTool : BaseToolCommand
    {
        private readonly GridSpawner _gridSpawner;
        protected override ToolType ToolType => ToolType.Shuffle;

        public ShuffleTool(GridSpawner gridSpawner, int useLeft, int useLeftInALevel) : base(useLeft, useLeftInALevel)
        {
            _gridSpawner = gridSpawner;
        }

        public override bool CanExecute()
        {
            return _useLeft > 0 && _gridSpawner != null && _useLeftInALevel > 0;
        }


        public override void Execute()
        {
            if (!CanExecute())
                return;
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

            _gridSpawner.RebuildCellMap();

            _gridSpawner.RefreshAllCells(_gridSpawner.CurrentGrid);
            ComsumeUse();
        }
    }
}