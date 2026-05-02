using UnityEngine;

namespace Utils
{
    public static class UtilsClass
    {
        // public static TextMesh CreateWorldText(string text, Transform parent = null,
        //     Vector3 localPosition = default(Vector3), int fontSize = 40, Color color = default(Color),
        //     TextAnchor textAnchor = TextAnchor.MiddleCenter, TextAlignment textAlignment = TextAlignment.Center,
        //     SortingLayer sortingLayer = 0)
        // {
        //     
        // }

        public static Vector3 GetWorldPosition(Vector3 origin,int x, int y, int z, float cellSize)
        {
            return origin + new Vector3(x, y, z) * cellSize;
        }
    }
}