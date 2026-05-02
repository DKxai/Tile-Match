using Grid_Map;

namespace _Scripts.Core.Rules
{
    public interface ITileValidator
    {
        bool isTileBlocked(TileGrid grid, int x, int y, int z);
    }
}