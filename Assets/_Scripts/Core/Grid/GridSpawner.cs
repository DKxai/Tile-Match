using System.Collections;
using System.Collections.Generic;
using Grid_Map;
using UnityEngine;
using Utils;

public class GridSpawner : MonoBehaviour
{
    [SerializeField] private float cellSize = 0.52f;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform origin;
    [SerializeField] private TileSpawner tileSpawner;
    public void Spawn(TileGrid grid)
    {
        for (int z = 0; z < grid.Layers; z++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    int value = grid.GetValue(x, y, z);
                    if (value != 0)
                    {
                        Vector3 newOrigin;
                        if (z % 2 == 0)
                        {
                            newOrigin = new Vector3(1, 1, 0) * cellSize;
                        }
                        

                        Vector3 worldPos = origin.position + UtilsClass.GetWorldPosition(x, y, z, cellSize);
                        GameObject obj = Instantiate(cellPrefab, worldPos, Quaternion.identity, origin);
                    }
                }
            }
            
        }
        if(tileSpawner != null)
            tileSpawner.Init();
    }

    public void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}