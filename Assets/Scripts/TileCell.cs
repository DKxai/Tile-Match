using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using Unity.VisualScripting;

public class TileCell : MonoBehaviour
{
    //public List<Tile> tiles;
    public Transform iconSprite;

    public int ID;

    //public Transform target;
    //public SelectedBaseShell SelectedBaseShell;
    private bool clicked = false;
    public int random { get; private set; }

    private void Start()
    {
        // int counter = tiles.Count;
        // random = Random.Range(0, counter);

        // SpriteRenderer sr = iconSprite.GetComponent<SpriteRenderer>();
        // sr.sprite = tiles[random].iconSprite;
        // VibrateCell();
        // transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f), 1f, 3, 0.5f);
        transform.DOPunchScale(Vector3.one * 0.3f, 0.5f, 10, 0.8f);
        
    }

    private void OnMouseDown()
    {
        if (clicked) return;
        clicked = true;
        BaseShellManager.Instance.AddTile(this);
    }
    // private void OnMouseDown()
    // {
    //     // them vao 
    //     SelectedBaseShell.AddedTilesToShell.Add(this.tiles[random]);
    //     SelectedBaseShell.AddedTileCellsToShell.Add(this);
    //     int counter = SelectedBaseShell.AddedTilesToShell.Count;
    //     Debug.Log($"{counter} in the shell");
    //     if (!SelectedBaseShell.SimilarCellsIDPosition.ContainsKey(this.tiles[random].id))
    //     {
    //         SelectedBaseShell.SimilarCellsIDPosition[tiles[random].id] = new List<int>();
    //     }
    //
    //     SelectedBaseShell.SimilarCellsIDPosition[tiles[random].id].Add(counter - 1);
    //     Debug.Log(
    //         $"{this.iconSprite.GetComponent<SpriteRenderer>().sprite.name} {String.Join(",", SelectedBaseShell.SimilarCellsIDPosition[tiles[random].id])}.");
    //     if (counter < 7)
    //     {
    //         transform.DOMove(SelectedBaseShell.BaseShellObjects[counter - 1].transform.position, 0.5f, false)
    //             .SetEase(Ease.OutExpo);
    //         Debug.Log($"{tiles[random].id} is clicked");
    //         if (SelectedBaseShell.SimilarCellsIDPosition[tiles[random].id].Count == 3)
    //         {
    //             foreach (var i in SelectedBaseShell.SimilarCellsIDPosition[tiles[random].id])
    //             {
    //                 SelectedBaseShell.AddedTileCellsToShell[i].transform
    //                     .DOMove(SelectedBaseShell.AddedTileCellsToShell[i].transform.position + new Vector3(1, 1, 0),
    //                         0.5f, false)
    //                     .SetEase(Ease.OutExpo);
    //             }
    //
    //             RemoveFromBaseShell(SelectedBaseShell.SimilarCellsIDPosition[tiles[random].id]);
    //             SlideCell();
    //         }
    //     }
    // }
    //
    // // xoa 3 cai cu va di chuyen cac thanh con lai sang ben trai
    // void RemoveFromBaseShell(List<int> cells)
    // {
    //     SelectedBaseShell.SimilarCellsIDPosition.Remove(SelectedBaseShell.AddedTilesToShell[0].id);
    //     cells.Sort();
    //     cells.Reverse();
    //     foreach (var i in cells)
    //     {
    //         SelectedBaseShell.AddedTilesToShell.RemoveAt(i);
    //         SelectedBaseShell.AddedTileCellsToShell.RemoveAt(i);
    //     }
    // }
    //
    // void MoveOtherCells()
    // {
    // }
    //
    // void VibrateCell()
    // {
    //     //transform.DOShakeRotation(5f, new Vector3(10, 10, 0), 5, 90f, false, ShakeRandomnessMode.Full);
    //     transform.DOLocalRotate(new Vector3(0, 0, 15), 0.1f)
    //         .SetLoops(6, LoopType.Yoyo);
    // }
    //
    // void SlideCell()
    // {
    //     // for (int i = 0; i < SelectedBaseShell.AddedTileCellsToShell.Count; i++)
    //     // {
    //     //     float length = SelectedBaseShell.AddedTileCellsToShell[i].transform.position.x +
    //     //                    SelectedBaseShell.BaseShellObjects[i].transform.position.x;
    //     //
    //     //     SelectedBaseShell.AddedTileCellsToShell[i].transform.DOMoveX(length, 0.5f, false);
    //     // }
    //     for (int i = 0; i < SelectedBaseShell.AddedTileCellsToShell.Count; i++)
    //     {
    //         SelectedBaseShell.AddedTileCellsToShell[i].transform
    //             .DOMove(
    //                 SelectedBaseShell.BaseShellObjects[i].transform.position,
    //                 0.5f
    //             )
    //             .SetEase(Ease.OutExpo);
    //     }
    // }

    void PopUpCell()
    {
    }

    void DestroyCell()
    {
    }
    // Random theo ti le 3 sao cho luon co cap 3 
}