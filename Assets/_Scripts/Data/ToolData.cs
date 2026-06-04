using UnityEngine;

namespace _Scripts.Data
{
    [CreateAssetMenu(fileName = "ToolData", menuName = "Data/ToolData")]
    public class ToolData : ScriptableObject
    {
        [Header("Shuffle")] public int shuffleCoinCost = 0;
        public int shuffleUseInALevel = 3;
        public int shuffleUseLeft;
        [Header("Add Slot")] public int addSlotCoinCost = 0;
        public int addSlotUseInALevel = 1;
        public int addSlotUseLeft;
        [Header("Return")] public int returnCoinCost = 0;
        public int returnUseInALevel = 1;
        public int returnUseLeft;
    }
}