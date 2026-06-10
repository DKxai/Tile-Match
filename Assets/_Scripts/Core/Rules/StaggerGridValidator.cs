using Grid_Map;

namespace _Scripts.Core.Rules
{
    public class StaggerGridValidator : ITileValidator
    {
        public bool isTileBlocked(TileGrid grid, int x, int y, int z)
        {
            if (z <= 0) return false;

            for (int layerAbove = z - 1; layerAbove >= 0; layerAbove--)
            {
                if (IsBlockedByLayer(grid, x, y, z, layerAbove))
                    return true;
            }

            return false;
        }

        private bool IsBlockedByLayer(TileGrid grid, int x, int y, int srcLayer, int targetLayer)
        {
            bool srcEven    = srcLayer % 2 == 0;
            bool targetEven = targetLayer % 2 == 0;

            if (srcEven == targetEven)
            {
                return HasTileAt(grid, x, y, targetLayer);
            }

            if (targetEven)
            {
                return HasTileAt(grid, x,     y,     targetLayer)
                       || HasTileAt(grid, x + 1, y,     targetLayer)
                       || HasTileAt(grid, x,     y + 1, targetLayer)
                       || HasTileAt(grid, x + 1, y + 1, targetLayer);
            }
            else
            {
                return HasTileAt(grid, x,     y,     targetLayer)
                       || HasTileAt(grid, x - 1, y,     targetLayer)
                       || HasTileAt(grid, x,     y - 1, targetLayer)
                       || HasTileAt(grid, x - 1, y - 1, targetLayer);
            }
        }

        private bool HasTileAt(TileGrid grid, int x, int y, int layer)
        {
            if (x < 0 || x >= grid.Width || y < 0 || y >= grid.Height
                || layer < 0 || layer >= grid.Layers)
                return false;

            return grid.GetValue(x, y, layer) != 0;
        }
    }
}