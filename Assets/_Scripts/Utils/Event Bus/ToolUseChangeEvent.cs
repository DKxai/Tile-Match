using _Scripts.Data;
using _Scripts.Data.Tool;

namespace _Scripts.Utils.Event_Bus
{
    public struct ToolUseChangeEvent
    {
        public readonly ToolType ToolType;
        public readonly int UseLeft;

        public ToolUseChangeEvent(ToolType toolType, int useLeft)
        {
            ToolType = toolType;
            UseLeft = useLeft;
        }
    }
}