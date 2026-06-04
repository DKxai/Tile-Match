using System;
using UnityEngine;

namespace _Scripts.Data
{
    public class DataSystem : PersistentSingleton<DataSystem>
    {
        private const string CoinsKey = "Coins";
        private const int DefaultCoins = 100;


        public int LoadCoins() => PlayerPrefs.GetInt(CoinsKey, DefaultCoins);

        public void SaveCoins(int amount)
        {
            PlayerPrefs.SetInt(CoinsKey, amount);
            PlayerPrefs.Save();
        }

        private const string HeartsKey = "Hearts";
        private const string LastHeartTimeKey = "LastHeartTime";
        private const int DefaultHearts = 5;

        public int LoadHearts() => PlayerPrefs.GetInt(HeartsKey, DefaultHearts);

        public void SaveHearts(int amount)
        {
            PlayerPrefs.SetInt(HeartsKey, amount);
            PlayerPrefs.Save();
        }

        public void SaveLastHeartTime(DateTime time)
        {
            PlayerPrefs.SetString(LastHeartTimeKey, time.ToBinary().ToString());
            PlayerPrefs.Save();
        }

        public DateTime LoadLastHeartTime()
        {
            if (!PlayerPrefs.HasKey(LastHeartTimeKey))
                return DateTime.Now;
            long binary = Convert.ToInt64(PlayerPrefs.GetString(LastHeartTimeKey));
            return DateTime.FromBinary(binary);
        }
    }
}