using UnityEngine;

namespace _Scripts.Utils
{
    public static class DifficultyCalculator
    {
        private const int StartIDsCount = 2;
        private const int LevelsPerStep = 3;

        public static int GetIDsCount(int currentLevel, int maxIDs)
        {
            int count = StartIDsCount + (currentLevel - 1) / LevelsPerStep;
            return Mathf.Clamp(count, StartIDsCount, maxIDs);
        }
    }
}