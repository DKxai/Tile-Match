using System.Collections.Generic;
using UnityEngine;

    public class SelectedBaseShell : MonoBehaviour
    {
        public Dictionary<int, List<int>> SimilarCellsIDPosition = new Dictionary<int, List<int>>();
        public List<GameObject> BaseShellObjects = new List<GameObject>();
        public List<Tile> AddedTilesToShell = new List<Tile>(); // co the chuyen sang hashset
        public List<TileCell> AddedTileCellsToShell = new List<TileCell>();
        

        void Awake()
        {
            SimilarCellsIDPosition.Clear();
        }

        // public int[] CheckThreeSameCells()
        // {
        //     if (tiles.Count < 3) return null;
        //     for (int i = 0; i < tiles.Count; i++)
        //     {
        //         array = new int[] { 0, 0, 0 };
        //         int counter = 0;
        //
        //         for (int y = 1; y < tiles.Count; y++)
        //         {
        //             if (cells.ContainsKey(tiles[i]))
        //             {
        //                 counter++;
        //                 switch (counter)
        //                 {
        //                     case 2:
        //                         array[0] = i;
        //                         array[1] = y;
        //                         continue;
        //                     case 3:
        //                         array[2] = y;
        //                         continue;
        //                     default: break;
        //                 }
        //             }
        //         }
        //         counter = 0;
        //
        //         Debug.Log($"{array[0]}, {array[1]}, {array[2]}");
        //         if (counter == 2) return array;
        //     }
        //
        //     return new int[] { 0, 0, 0 };
        // }
        
    }