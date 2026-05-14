using UnityEngine;

namespace Grid_Map
{
    [CreateAssetMenu(fileName = "ToolData", menuName = "Data/ToolData")]
    public class ToolData : ScriptableObject
    {
        [Header("Shuffle")] public int shuffleCoinCost = 0;
        public int shuffleUseInALevel = 3;

        [Header("Add Slot")] public int addSlotCoinCost = 0;
        public int addSlotUseInALevel = 1;
        [Header("Return")]
        public int returnCoinCost = 0;
        
        [Header("Hint")]
        public int hintCoinCost = 0;
    }
}