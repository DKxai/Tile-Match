using _Scripts.Data;

namespace _Scripts.Utils.Event_Bus
{
    public struct OutOfToolUseEvent
    {
        public readonly ToolType ToolType;

        public OutOfToolUseEvent(ToolType toolType)
        {
            ToolType = toolType;
        }
    }
}