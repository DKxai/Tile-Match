using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using Grid_Map;
using Unity.VisualScripting;

public class TileCell : MonoBehaviour
{
    [Header("Tile info")]
    public Transform iconSprite;
    public int ID;
    
    public bool IsClicked = false;

    [Header("Grid Coordinates")] 
    public int gridX;
    public int gridY;
    public int gridZ;

    private Collider2D col;
    private SpriteRenderer[] sr;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        transform.DOPunchScale(Vector3.one * 0.3f, 0.5f, 10, 0.8f);
        
    }

    public void SetupCell(int x, int y, int layer)
    {
        gridX = x;
        gridY = y;
        gridZ = layer;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -layer;

    }
    public void RefreshState(TileGrid grid)
    {
        if (IsClicked) return;

        bool isBlocked = LevelManager.Instance.RuleValidator.isTileBlocked(grid, gridX, gridY, gridZ);
        
        if(col != null ) col.enabled = !isBlocked;

        float brightness = isBlocked ? 0.5f : 1f;

        foreach (var s in sr)
        {
            if (s != null)
            {
                Color c = s.color;
                
                s.color = new Color(brightness, brightness, brightness,c.a);
            }
        }
    }
}