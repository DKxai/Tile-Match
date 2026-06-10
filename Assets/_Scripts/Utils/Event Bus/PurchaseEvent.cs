using _Scripts.Data;
using _Scripts.Data.Tool;

namespace _Scripts.Utils.Event_Bus
{
    public struct PurchaseEvent
    {
        public readonly ToolType ToolType;
        public readonly int Amount;
        public readonly int Cost;

        public PurchaseEvent(ToolType toolType, int amount, int cost)
        {
            ToolType = toolType;
            Amount = amount;
            Cost = cost;
        }
    }
}