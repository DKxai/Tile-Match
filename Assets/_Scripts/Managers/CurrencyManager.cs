using UnityEngine;

namespace _Scripts.Managers
{
    public class CurrencyManager : PersistentSingleton<CurrencyManager>
    {
        public int Coins { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Coins = PlayerPrefs.GetInt("Coins");
        }

        public bool HasEnoughCoins(int amount) => Coins >= amount;

        public void AddCoins(int amount)
        {
            Coins += amount;
            Save();
        }

        public bool SpendCoins(int amount)
        {
            if (!HasEnoughCoins(amount)) return false;
            Coins -= amount;
            Save();
            return true;
        }
        private void Save() => PlayerPrefs.SetInt("Coins", Coins);
    }
}