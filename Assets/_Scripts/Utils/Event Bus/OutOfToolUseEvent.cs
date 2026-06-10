using _Scripts.Data;
using _Scripts.Data.Tool;

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