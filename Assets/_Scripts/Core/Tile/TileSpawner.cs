using System.Collections.Generic;
using _Scripts.Managers;
using _Scripts.SaveSystem;
using _Scripts.Utils;
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

        public void Init(List<TileCell> cells)
        {
            _cells = new List<TileCell>(cells);
            _isValidNumberOfTileCell = CheckNumberOfTileCell();
            if (!_isValidNumberOfTileCell) return;

            int currentLevel = DataSystem.LoadSelectedLevel();
            int idCount = DifficultyCalculator.GetIDsCount(currentLevel, tiles.Count);
            RandomSystem(idCount);
        }

        private bool CheckNumberOfTileCell()
        {
            _cellCounter = _cells.Count;
            if (_cellCounter < 3 || _cellCounter % 3 != 0) return false;

            return true;
        }

        public void RandomSystem(int idsCount)
        {
            int remainingTiles = _cellCounter;
            int cellGroupNums = _cellCounter / 3;
            List<TileModel> tileModels = GetRandomTile(idsCount);

            for (int i = 0; i < cellGroupNums; i++)
            {
                int randomTile = i % idsCount;
                int numTiles = 3;
                if (Random.value > 0.85f && remainingTiles >= 6 && DataSystem.IsTutorialDone )
                {
                    numTiles += 3;
                    cellGroupNums--;
                    Debug.Log("Special");
                }

                for (int x = 0; x < numTiles; x++)
                {
                    int randomCell = Random.Range(0, _cells.Count);
                    _cells[randomCell].iconSprite.GetComponent<SpriteRenderer>().sprite =
                        tileModels[randomTile].iconSprite;
                    _cells[randomCell].ID = tileModels[randomTile].id;
                    _cells.Remove(_cells[randomCell]);
                    remainingTiles--;
                }
            }

            cellGroupNums = _cellCounter / 3;
            ShellManager.Instance.InitGroupCount(cellGroupNums);
        }

        private List<TileModel> GetRandomTile(int idsCount)
        {
            List<TileModel> cells = new List<TileModel>(tiles);
            List<TileModel> result = new List<TileModel>();
            for (int i = 0; i < idsCount; i++)
            {
                int ran = Random.Range(0, cells.Count);
                result.Add(cells[ran]);
                cells.Remove(cells[ran]);
            }

            return result;
        }
    }
}