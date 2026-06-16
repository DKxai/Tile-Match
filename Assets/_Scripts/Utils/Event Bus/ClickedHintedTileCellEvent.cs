using _Scripts.Core.Tile;

namespace _Scripts.Utils.Event_Bus
{
    public struct ClickedHintedTileCellEvent
    {
        public readonly TileCell ClickedTile;

        public ClickedHintedTileCellEvent(TileCell clickedTile)
        {
            ClickedTile = clickedTile;
        }
    }
}