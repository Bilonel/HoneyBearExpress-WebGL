// Assets/Scripts/UI/PlayerStatsUI.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HoneyBearExpress.Core;

namespace HoneyBearExpress.UI
{
    public class PlayerStatsUI : MonoBehaviour, IWorldInitializable
    {
        [Header("UI Components")]
        [SerializeField] private TMP_Text coinsText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text xpText;
        [SerializeField] private Image xpSliderImage;

        private PlayerStatsManager _statsManager;

        public void Initialize(WorldServices services)
        {
            _statsManager = services.PlayerStatsManager;

            if (_statsManager != null)
            {
                _statsManager.OnStatsChanged += UpdateUI;
                UpdateUI(); // İlk açılışta ekranı doldur
            }
        }

        private void OnDestroy()
        {
            if (_statsManager != null)
            {
                _statsManager.OnStatsChanged -= UpdateUI;
            }
        }

        // Sadece veri değiştiğinde çalışan sıfır-GC fonksiyon
        private void UpdateUI()
        {
            if (_statsManager == null) return;

            // TMPro SetText: WebGL için tasarlanmış sıfır-GC tamsayı yazdırma yöntemi
            coinsText.SetText("{0}", _statsManager.Coins);
            levelText.SetText("{0}", _statsManager.Level);
            if(xpText!=null)xpText.SetText("{0} / {1}", _statsManager.XP, _statsManager.XpNeededForNextLevel);

            if (xpSliderImage != null)
            {
                float progress = (float)_statsManager.XP / _statsManager.XpNeededForNextLevel;
                xpSliderImage.fillAmount = progress;
            }
        }
    }
}
