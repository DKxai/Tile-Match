using System;
using _Scripts.Data;
using _Scripts.Data.Tool;
using UnityEngine;

namespace _Scripts.SaveSystem
{
    public static class DataSystem
    {
        private const string CurrentLevelKey = "CurrentLevel";
        private const string SelectedLevelKey = "SelectedLevel";
        private const string CoinsKey = "Coins";
        private const string HeartsKey = "Hearts";
        private const string LastHeartTimeKey = "LastHeartTime";
        private const string RetryKey = "Retry";
        private const string AddSlotKey = "AddSlot";
        private const string ShuffleKey = "Shuffle";
        private const string AdsKey = "Ads";

        private const int DefaultLevel = 2;
        private const int DefaultCoins = 100;
        private const int DefaultHearts = 5;
        private const int DefaultToolUse = 0;
        private const int AdsEnabled = 1;

        #region CurrentLevel

        public static int LoadCurrentLevel() => PlayerPrefs.GetInt(CurrentLevelKey, DefaultLevel);

        public static void SaveCurrentLevel(int level)
        {
            PlayerPrefs.SetInt(CurrentLevelKey, level);
            PlayerPrefs.Save();
        }

        #endregion

        #region SelectedLevel

        public static int LoadSelectedLevel() => PlayerPrefs.GetInt(SelectedLevelKey, LoadCurrentLevel());

        public static void SaveSelectedLevel(int level)
        {
            PlayerPrefs.SetInt(SelectedLevelKey, level);
            PlayerPrefs.Save();
        }

        #endregion

        #region Coins

        public static int LoadCoins() => PlayerPrefs.GetInt(CoinsKey, DefaultCoins);

        public static void SaveCoins(int amount)
        {
            PlayerPrefs.SetInt(CoinsKey, amount);
            PlayerPrefs.Save();
        }

        #endregion

        #region Hearts

        public static int LoadHearts() => PlayerPrefs.GetInt(HeartsKey, DefaultHearts);

        public static void SaveHearts(int amount)
        {
            PlayerPrefs.SetInt(HeartsKey, amount);
            PlayerPrefs.Save();
        }

        #endregion

        #region ToolUse

        public static int LoadToolUse(ToolType toolType) => PlayerPrefs.GetInt(toolType.ToString(), DefaultToolUse);

        public static void SaveToolUse(ToolType toolType, int use)
        {
            PlayerPrefs.SetInt(toolType.ToString(), use);
            PlayerPrefs.Save();
        }
        

        #endregion

        #region Ads

        public static bool LoadAds()
        {
            return PlayerPrefs.GetInt(AdsKey, AdsEnabled) == 1? true : false;
        }

        public static void SaveAds(bool isEnabled)
        {
            int value = isEnabled ? 1 : 0;
            PlayerPrefs.SetInt(AdsKey, value);
            PlayerPrefs.Save();
        }

        #endregion
        public static void SaveLastHeartTime(DateTime time)
        {
            PlayerPrefs.SetString(LastHeartTimeKey, time.ToBinary().ToString());
            PlayerPrefs.Save();
        }

        public static DateTime LoadLastHeartTime()
        {
            if (!PlayerPrefs.HasKey(LastHeartTimeKey))
                return DateTime.Now;
            long binary = Convert.ToInt64(PlayerPrefs.GetString(LastHeartTimeKey));
            return DateTime.FromBinary(binary);
        }
    }
}