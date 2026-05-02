using Grid_Map;

namespace _Scripts.Core.Rules
{
    public class StaggerGridValidator : ITileValidator
    {
        public bool isTileBlocked(TileGrid grid, int x, int y, int z)
        {
            if (z <= 0) return false;

            int layerAbove = z - 1;

            if (z % 2 == 0)
            {
                if (HasTileAt(grid, x, y, layerAbove)) return true;
                if (HasTileAt(grid, x - 1, y, layerAbove)) return true;
                if (HasTileAt(grid, x, y - 1, layerAbove)) return true;
                if (HasTileAt(grid, x - 1, y - 1, layerAbove)) return true;
            }
            else
            {
                if (HasTileAt(grid, x, y, layerAbove)) return true;
                if (HasTileAt(grid, x + 1, y, layerAbove)) return true;
                if (HasTileAt(grid, x, y + 1, layerAbove)) return true;
                if (HasTileAt(grid, x + 1, y + 1, layerAbove)) return true;
            }

            return false;
        }

        private bool HasTileAt(TileGrid grid, int x, int y, int layer)
        {
            if (x < 0 || x >= grid.Width || y < 0 || y >= grid.Height || layer < 0 || layer >= grid.Layers)
                return false;

            return grid.GetValue(x, y, layer) != 0;
        }
    }
}