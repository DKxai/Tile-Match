using System;
using UnityEngine;

[Serializable]
public class Tile
{
    public TileData data;
    public int id => data.ID;
    public Sprite iconSprite => data.IconSprite;
}