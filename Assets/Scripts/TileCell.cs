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
    private bool clicked = false;
    public int random { get; private set; }

    private void Start()
    {
        transform.DOPunchScale(Vector3.one * 0.3f, 0.5f, 10, 0.8f);
        
    }

    private void OnMouseDown()
    {
        if (clicked) return;
        clicked = true;
        BaseShellManager.Instance.AddTile(this);
    }
    
    void PopUpCell()
    {
    }

    void DestroyCell()
    {
    }
    // Random theo ti le 3 sao cho luon co cap 3 
}