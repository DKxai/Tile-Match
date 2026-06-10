using System.Collections.Generic;
using _Scripts.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Core.Tile
{
    public class TileSpawner : MonoBehaviour
    {
        [SerializeField] private List<TileModel> tiles;
        private List<TileCell> _cells = new List<TileCell>();
        private bool _isValidNumberOfTileCell = false;
        private int _cellCounter;

        public void Init()
        {
            _cells = new List<TileCell>(GetComponentsInChildren<TileCell>());

            _isValidNumberOfTileCell = CheckNumberOfTileCell();
            if (!_isValidNumberOfTileCell) return;

            RandomSystem(_cells);
        }

        private bool CheckNumberOfTileCell()
        {
            _cellCounter = _cells.Count;
            if (_cellCounter < 3 || _cellCounter % 3 != 0) return false;

            return true;
        }

        public void RandomSystem(List<TileCell> tileCells)
        {
            int cellGroupNums = _cellCounter / 3;

            for (int i = 0; i < cellGroupNums; i++)
            {
                int randomTile = Random.Range(0, tiles.Count);
                for (int x = 0; x < 3; x++)
                {
                    int randomCell = Random.Range(0, _cells.Count);
                    _cells[randomCell].iconSprite.GetComponent<SpriteRenderer>().sprite = tiles[randomTile].iconSprite;
                    _cells[randomCell].ID = tiles[randomTile].id;
                    _cells.Remove(_cells[randomCell]);
                }
            }

            ShellManager.Instance.InitGroupCount(cellGroupNums);
        }
    }
}