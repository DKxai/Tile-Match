namespace _Scripts.Utils.Event_Bus
{
    public struct CurrencyChangeEvent
    {
        public readonly int Amount;

        public CurrencyChangeEvent(int amount)
        {
            Amount = amount;
        }
    }
}