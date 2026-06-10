using UnityEngine;

namespace _Scripts.Utils
{
    public static class UtilsClass
    {

        public static Vector3 GetWorldPosition(Vector3 origin,int x, int y, int z, float cellSize)
        {
            return origin + new Vector3(x, y, z) * cellSize;
        }
    }
}