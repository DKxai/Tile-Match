using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using Unity.VisualScripting;

public class TileCell : MonoBehaviour
{
    public Transform iconSprite;

    public int ID;
    public bool IsClicked = false;
    private void Start()
    {
        transform.DOPunchScale(Vector3.one * 0.3f, 0.5f, 10, 0.8f);
        
    }
    
}