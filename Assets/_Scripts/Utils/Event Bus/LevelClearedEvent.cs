using Grid_Map;

namespace _Scripts.Utils.Event_Bus
{
    public struct LevelClearedEvent
    {
        public TileGrid Grid;
        public LevelClearedEvent(TileGrid grid)
        {
            Grid = grid;
        }
    }
}