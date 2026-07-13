// Assets/Scripts/Core/PlayerStatsManager.cs
using System;
using UnityEngine;
using HoneyBearExpress.Buildings;

namespace HoneyBearExpress.Core
{
    [Serializable]
    public struct ItemRewardConfig
    {
        public HoneyItemType Type;
        public int CoinReward;
        public int XpReward;
    }

    public class PlayerStatsManager : MonoBehaviour, IWorldInitializable
    {
        [Header("Item Rewards")]
        [SerializeField] private ItemRewardConfig[] itemRewards;

        [Header("Leveling Settings")]
        [SerializeField] private int baseXpNeeded = 100;
        [SerializeField] private float xpMultiplierPerLevel = 1.2f;
	public event Action<UpgradeType[]> OnLevelUp;

        // Oyuncu İstatistikleri
        public long Coins { get; private set; }
        public int XP { get; private set; }
        public int Level { get; private set; }=1;
        public int XpNeededForNextLevel { get; private set; }

        // Pasif Geliştirme Çarpanları (Modifiers)
        public float ConveyorSpeedMultiplier { get; private set; } = 1.0f;
        public float ExtractorSpeedMultiplier { get; private set; } = 1.0f;
        public float HiveProductionCooldownMultiplier { get; private set; } = 1.0f; // Düştükçe hızlanır

        private int[] _coinRewardLookupTable;
        private int[] _xpRewardLookupTable;
        private TickManager _tickManager;

        // GC'yi sıfırlamak için önceden allocate edilmiş dizi (Teklif kartları için)
        private readonly UpgradeType[] _offeredUpgrades = new UpgradeType[3];

        private void BuildLookupTables()
        {
            int maxEnumValue = 0;
            var enumValues = Enum.GetValues(typeof(HoneyItemType));
            for (int i = 0; i < enumValues.Length; i++)
            {
                int val = (int)enumValues.GetValue(i);
                if (val > maxEnumValue) maxEnumValue = val;
            }

            _coinRewardLookupTable = new int[maxEnumValue + 1];
            _xpRewardLookupTable = new int[maxEnumValue + 1];

            for (int i = 0; i < itemRewards.Length; i++)
            {
                int index = (int)itemRewards[i].Type;
                _coinRewardLookupTable[index] = itemRewards[i].CoinReward;
                _xpRewardLookupTable[index] = itemRewards[i].XpReward;
            }
        }
        private void LevelUp()
        {
            Level++;
            XpNeededForNextLevel = Mathf.RoundToInt(XpNeededForNextLevel * xpMultiplierPerLevel);
            
            Debug.Log($"[PlayerStatsManager] LEVEL UP! New Level: {Level}. XP needed: {XpNeededForNextLevel}");

            // Ticks durduruluyor (Oyun Pause)
            if (_tickManager != null)
            {
                _tickManager.StopTicking();
            }

            GenerateUpgradeOffers();
            
            // UI'ı tetikle ve 3 teklifi gönder
    OnLevelUp?.Invoke(_offeredUpgrades);
        }

        private void GenerateUpgradeOffers()
        {
            // Havuzdan 3 adet rasgele geliştirme seç (Sıfır-GC)
            _offeredUpgrades[0] = UpgradeType.ConveyorSpeedBoost;
            _offeredUpgrades[1] = UpgradeType.ExtractorSpeedBoost;
            _offeredUpgrades[2] = UpgradeType.HiveProductionBoost;
        }

        private int GetCoinReward(HoneyItemType type)
        {
            int index = (int)type;
            if (index >= 0 && index < _coinRewardLookupTable.Length) return _coinRewardLookupTable[index];
            return 0;
        }

        private int GetXpReward(HoneyItemType type)
        {
            int index = (int)type;
            if (index >= 0 && index < _xpRewardLookupTable.Length) return _xpRewardLookupTable[index];
            return 0;
        }

        // UI'ın dinleyeceği olay
        public event Action OnStatsChanged;

        public void Initialize(WorldServices services)
        {
            _tickManager = services.TickManager;
            Coins = 0;
            XP = 0;
            Level = 1;
            XpNeededForNextLevel = baseXpNeeded;

            BuildLookupTables();
            
            // İlk tetikleme
            OnStatsChanged?.Invoke(); 
            Debug.Log("[PlayerStatsManager] Initialized.");
        }

        public void OnItemDelivered(HoneyItemType type)
        {
            int coinReward = GetCoinReward(type);
            int xpReward = GetXpReward(type);

            Coins += coinReward;
            AddXP(xpReward);

            // Veriler değişti, UI'a haber ver
            OnStatsChanged?.Invoke(); 
            
            Debug.Log($"[PlayerStatsManager] Delivered {type}! +{coinReward} Coins, +{xpReward} XP. Total: {Coins} Gold");
        }

        private void AddXP(int amount)
        {
            XP += amount;
            while (XP >= XpNeededForNextLevel)
            {
                XP -= XpNeededForNextLevel;
                LevelUp();
            }
            
            // Seviye atlamasa bile XP barının güncellenmesi için tetikle
            OnStatsChanged?.Invoke();
        }

        // SelectUpgrade fonksiyonunun sonuna da ekliyoruz (Geliştirme seçilip oyun devam ettiğinde UI yenilensin)
        public void SelectUpgrade(UpgradeType upgrade)
        {
        	switch (upgrade)
            {
                case UpgradeType.ConveyorSpeedBoost:
                    ConveyorSpeedMultiplier += 0.05f; // +%5 hız
                    Debug.Log($"[Upgrade] Conveyor Speed +5%. New Multiplier: {ConveyorSpeedMultiplier}");
                    break;
                case UpgradeType.ExtractorSpeedBoost:
                    ExtractorSpeedMultiplier += 0.10f; // +%10 hız
                    Debug.Log($"[Upgrade] Extractor Speed +10%. New Multiplier: {ExtractorSpeedMultiplier}");
                    break;
                case UpgradeType.HiveProductionBoost:
                    HiveProductionCooldownMultiplier -= 0.08f; // %8 daha hızlı üretim (cooldown düşüyor)
                    HiveProductionCooldownMultiplier = Mathf.Max(0.2f, HiveProductionCooldownMultiplier); // Limit
                    Debug.Log($"[Upgrade] Hive Cooldown -8%. New Multiplier: {HiveProductionCooldownMultiplier}");
                    break;
            }

            if (_tickManager != null)
            {
                _tickManager.StartTicking();
            }

            OnStatsChanged?.Invoke(); // UI güncelle
        }
        
	public bool TrySpendCoins(int amount)
	{
	    if (Coins >= amount)
	    {
		Coins -= amount;
		OnStatsChanged?.Invoke(); // UI'ı bilgilendir
		return true;
	    }
	    return false; // Yetersiz bakiye
	}
	public void AwardCoinsAndXP(int coins, int xp)
	{
	    Coins += coins;
	    AddXP(xp);
	    OnStatsChanged?.Invoke(); // UI'ı güncelle
	}
    }
}

   
