// Assets/Scripts/Core/QuestInstance.cs
using UnityEngine;
using HoneyBearExpress.Buildings;

namespace HoneyBearExpress.Core
{
    public class QuestInstance
    {
        public string CustomerName { get; private set; }
        
        // Bellek çöpünü engellemek için sabit boyutlu diziler (Görev başına max 2 farklı item tipi)
        public HoneyItemType[] ReqTypes { get; private set; } = new HoneyItemType[2];
        public int[] TargetAmounts { get; private set; } = new int[2];
        public int[] CurrentAmounts { get; private set; } = new int[2];
        
        public int ReqCount { get; private set; }
        public int CoinReward { get; private set; }
        public int XpReward { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsActive { get; private set; }

        public void Setup(string customerName, QuestTemplate template, float varianceFactor)
        {
            CustomerName = customerName;
            ReqCount = Mathf.Min(template.Requirements.Length, 2);

            for (int i = 0; i < ReqCount; i++)
            {
                ReqTypes[i] = template.Requirements[i].Type;
                
                // %15 varyasyon (sapma) hesabı
                int baseAmt = template.Requirements[i].BaseAmount;
                int variedAmt = Mathf.RoundToInt(baseAmt * varianceFactor);
                TargetAmounts[i] = Mathf.Max(1, variedAmt); // En az 1 tane istensin
                CurrentAmounts[i] = 0;
            }

            // Ödülleri de zorluk sapmasına göre ölçeklendiriyoruz
            CoinReward = Mathf.RoundToInt(template.BaseCoinReward * varianceFactor);
            XpReward = Mathf.RoundToInt(template.BaseXpReward * varianceFactor);

            IsCompleted = false;
            IsActive = true;
        }

        // Gelen item bu göreve uyuyorsa teslim al ve True dön
        public bool DeliverItem(HoneyItemType type)
        {
            if (IsCompleted || !IsActive) return false;

            bool updated = false;
            for (int i = 0; i < ReqCount; i++)
            {
                if (ReqTypes[i] == type && CurrentAmounts[i] < TargetAmounts[i])
                {
                    CurrentAmounts[i]++;
                    updated = true;
                    break;
                }
            }

            if (updated)
            {
                CheckCompletion();
            }

            return updated;
        }

        private void CheckCompletion()
        {
            bool allDone = true;
            for (int i = 0; i < ReqCount; i++)
            {
                if (CurrentAmounts[i] < TargetAmounts[i])
                {
                    allDone = false;
                    break;
                }
            }
            IsCompleted = allDone;
        }
    }
}
