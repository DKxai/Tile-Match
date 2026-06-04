using _Scripts.Data;

namespace _Scripts.Utils.Event_Bus
{
    public struct OnAddHeartButtonClickedEvent
    {
        public readonly ToolType toolType;
        public OnAddHeartButtonClickedEvent(ToolType toolType)
        {
            this.toolType = toolType;
        }
        
    }
}