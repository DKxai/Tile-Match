namespace _Scripts.Utils.Event_Bus
{
    public struct LevelSelectEvent
    {
        public readonly string LevelSelected;

        public LevelSelectEvent(string levelSelected)
        {
            LevelSelected = levelSelected;
        }
    }
}