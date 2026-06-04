using _Scripts.Data;

namespace _Scripts.Utils.Event_Bus
{
    public struct AddingToolUseEvent
    {
        public readonly ToolType ToolType;
        public readonly int Amount;

        public AddingToolUseEvent(ToolType toolType, int amount)
        {
            ToolType = toolType;
            Amount = amount;
        }

    }
}