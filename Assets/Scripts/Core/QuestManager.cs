// Assets/Scripts/Core/QuestManager.cs
using UnityEngine;
using System.Collections.Generic;
using HoneyBearExpress.Buildings;

namespace HoneyBearExpress.Core
{
    public class QuestManager : MonoBehaviour, IWorldInitializable
    {
        [SerializeField] private QuestDatabase questDatabase;
        
        private PlayerStatsManager _statsManager;
        
        // Ekranda her an tam olarak 3 aktif görev bulunur (Sıfır-GC için önceden allocate edildi)
        public readonly QuestInstance[] ActiveQuests = new QuestInstance[3];
        
        public event System.Action OnQuestsUpdated;

        public void Initialize(WorldServices services)
        {
            _statsManager = services.PlayerStatsManager;

            // Örnekleri bellekte oluştur (Runtime'da bir daha new çağrılmayacak)
            for (int i = 0; i < ActiveQuests.Length; i++)
            {
                ActiveQuests[i] = new QuestInstance();
            }

            GenerateAllQuests();
            Debug.Log("[QuestManager] Initialized.");
        }

        // ShippingDock'tan gelen eşyayı işler
        // Eğer görevler tüketirse True, görevler istemezse doğrudan satılabilmesi için False döner.
        public bool OnItemDelivered(HoneyItemType type)
        {
            bool consumedByQuest = false;
            
            for (int i = 0; i < ActiveQuests.Length; i++)
            {
                if (ActiveQuests[i].IsActive && ActiveQuests[i].DeliverItem(type))
                {
                    consumedByQuest = true;

                    if (ActiveQuests[i].IsCompleted)
                    {
                        CompleteQuest(i);
                    }
                    break; // Eşya sadece ilk eşleşen aktif göreve verilir
                }
            }

            if (consumedByQuest)
            {
                OnQuestsUpdated?.Invoke();
            }

            return consumedByQuest;
        }

        private void CompleteQuest(int index)
        {
            QuestInstance quest = ActiveQuests[index];
            
            if (_statsManager != null)
            {
                // Oyuncuyu ödüllendir
                _statsManager.AwardCoinsAndXP(quest.CoinReward, quest.XpReward);
            }

            // Yeni bir görev üret
            GenerateQuest(index);
        }

        private void GenerateAllQuests()
        {
            for (int i = 0; i < ActiveQuests.Length; i++)
            {
                GenerateQuest(i);
            }
            OnQuestsUpdated?.Invoke();
        }

        private void GenerateQuest(int index)
        {
            if (questDatabase == null || questDatabase.templates.Length == 0) return;

            int playerLevel = _statsManager != null ? _statsManager.Level : 1;

            // Seviyeye uygun şablon bul
            QuestTemplate selectedTemplate = GetRandomTemplateForLevel(playerLevel);

            // Müşteri ismi seç
            string customerName = GetRandomCustomerName();

            // %15 varyasyon çarpanı (0.85f ile 1.15f arası)
            float variance = Random.Range(0.85f, 1.15f);

            // Hazır olan slota yeni görev bilgilerini yazdır
            ActiveQuests[index].Setup(customerName, selectedTemplate, variance);
        }

        private QuestTemplate GetRandomTemplateForLevel(int level)
        {
		if (questDatabase == null || questDatabase.templates == null || questDatabase.templates.Length == 0)
	    {
		Debug.LogError("[QuestManager] QuestDatabase veya görev şablonları boş! Editörden atama yaptığınızdan emin olun.");
		return default;
	    }
	    
            int matchCount = 0;
            for (int i = 0; i < questDatabase.templates.Length; i++)
            {
                if (questDatabase.templates[i].TargetLevel == level) matchCount++;
            }

            if (matchCount > 0)
            {
                int targetIdx = Random.Range(0, matchCount);
                int currentMatch = 0;
                for (int i = 0; i < questDatabase.templates.Length; i++)
                {
                    if (questDatabase.templates[i].TargetLevel == level)
                    {
                        if (currentMatch == targetIdx) return questDatabase.templates[i];
                        currentMatch++;
                    }
                }
            }

            // 2. Aşama (Smart Fallback): Tam eşleşme yoksa, oyuncunun seviyesinden küçük en yakın şablonu bul
	    Debug.LogWarning($"[QuestManager] Seviye {level} için özel şablon bulunamadı. En yakın alt seviye şablonu aranıyor...");
	    
	    int closestLevel = -1;
	    int closestIdx = -1;
	    for (int i = 0; i < questDatabase.templates.Length; i++)
	    {
		int tLevel = questDatabase.templates[i].TargetLevel;
		if (tLevel <= level && tLevel > closestLevel)
		{
		    closestLevel = tLevel;
		    closestIdx = i;
		}
	    }

	    if (closestIdx != -1)
	    {
		Debug.Log($"[QuestManager] Seviye {level} için en yakın alt seviye şablonu bulundu: Seviye {closestLevel}");
		return questDatabase.templates[closestIdx];
	    }

	    // 3. Aşama (Absolute Fallback): Hiçbir şey bulunamazsa ilk şablonu dön
	    Debug.LogError("[QuestManager] Oyuncu seviyesine uygun hiçbir şablon bulunamadı! Dizi indeksindeki ilk şablon (Index 0) zorla çekiliyor.");
	    return questDatabase.templates[0];
        }

        private string GetRandomCustomerName()
        {
            if (questDatabase.customerNames == null || questDatabase.customerNames.Length == 0)
            {
                return "Gizemli Ayı";
            }
            int idx = Random.Range(0, questDatabase.customerNames.Length);
            return questDatabase.customerNames[idx];
        }
    }
}
