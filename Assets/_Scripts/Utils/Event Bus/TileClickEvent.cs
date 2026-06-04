namespace _Scripts.Utils.Event_Bus
{
    public struct TileClickEvent
    {
        public readonly TileCell TileCell;

        public TileClickEvent(TileCell tileCell)
        {
            TileCell = tileCell;
        }
    }
}