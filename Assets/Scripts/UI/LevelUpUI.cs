// Assets/Scripts/UI/LevelUpUI.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HoneyBearExpress.Core;

namespace HoneyBearExpress.UI
{
    public class LevelUpUI : MonoBehaviour, IWorldInitializable
    {
        [System.Serializable]
        public struct CardUI
        {
            public GameObject root;
            public TMP_Text titleText;
            public TMP_Text descText;
            public Button selectButton;
        }

        [Header("References")]
        [SerializeField] private GameObject panelRoot;
        [SerializeField] private CardUI[] cards; // Inspector'dan tam olarak 3 kart tanımlanacak

        private PlayerStatsManager _statsManager;
        private UpgradeType[] _activeOffers; // Aktif teklifleri hafızada tutar

        public void Initialize(WorldServices services)
        {
            _statsManager = services.PlayerStatsManager;
            panelRoot.SetActive(false);

            if (_statsManager != null)
            {
                _statsManager.OnLevelUp += ShowLevelUpPanel;
            }

            SetupButtonListenersOnce();
        }

        private void OnDestroy()
        {
            if (_statsManager != null)
            {
                _statsManager.OnLevelUp -= ShowLevelUpPanel;
            }
        }

        // WebGL Optimizasyonu: Runtime'da GC üretmemek için dinleyicileri sadece 1 kez başta kuruyoruz
        private void SetupButtonListenersOnce()
        {
            if (cards.Length >= 3)
            {
                cards[0].selectButton.onClick.AddListener(() => SelectCardByIndex(0));
                cards[1].selectButton.onClick.AddListener(() => SelectCardByIndex(1));
                cards[2].selectButton.onClick.AddListener(() => SelectCardByIndex(2));
            }
        }

        private void ShowLevelUpPanel(UpgradeType[] offers)
        {
            _activeOffers = offers;

            for (int i = 0; i < cards.Length && i < offers.Length; i++)
            {
                UpgradeType type = offers[i];
                GetUpgradeDetails(type, out string title, out string desc);

                cards[i].titleText.text = title;
                cards[i].descText.text = desc;
                
                cards[i].root.SetActive(true);
            }

            panelRoot.SetActive(true);
        }

        private void SelectCardByIndex(int index)
        {
            if (_activeOffers == null || index >= _activeOffers.Length) return;

            panelRoot.SetActive(false); // Paneli kapat
            
            // Seçilen geliştirmeyi uygulayıp oyunu devam ettirir
            _statsManager.SelectUpgrade(_activeOffers[index]);
        }

        // Dil/Metin eşleştirmesi (Dinamik string birleştirme yapmayarak GC engeller)
        private void GetUpgradeDetails(UpgradeType type, out string title, out string desc)
        {
            switch (type)
            {
                case UpgradeType.ConveyorSpeedBoost:
                    title = "Hızlı Bantlar";
                    desc = "Conveyor bantlarının taşıma hızı %5 artar.";
                    break;
                case UpgradeType.ExtractorSpeedBoost:
                    title = "Süper Süzücü";
                    desc = "Bal süzme makinelerinin işleme hızı %10 artar.";
                    break;
                case UpgradeType.HiveProductionBoost:
                    title = "Çalışkan Arılar";
                    desc = "Kovanların üretim yapma süresi %8 kısalır.";
                    break;
                default:
                    title = "Geliştirme";
                    desc = "Pasif bir etki kazandırır.";
                    break;
            }
        }
    }
}
