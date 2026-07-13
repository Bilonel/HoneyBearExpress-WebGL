// Assets/Scripts/UI/QuestSlotUI.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HoneyBearExpress.Core;

namespace HoneyBearExpress.UI
{
    public class QuestSlotUI : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private TMP_Text customerNameText;
        [SerializeField] private TMP_Text rewardCoinsText;
        [SerializeField] private TMP_Text rewardXpText;

        // Görev başına maksimum 2 farklı eşya tipi talep edilebilir (Zero-GC Dizisi)
        [System.Serializable]
        public struct RequirementUI
        {
            public GameObject root;
            public TMP_Text progressText;
            public Image itemIcon;
        }

        [SerializeField] private RequirementUI[] requirements; // Tam olarak 2 adet atanacak

        public void UpdateVisual(QuestInstance quest, QuestPanelUI mainPanel)
        {
            if (quest == null || !quest.IsActive)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);
            if(customerNameText!=null) customerNameText.text = quest.CustomerName;

            // Gereksinimleri Güncelle
            for (int i = 0; i < requirements.Length; i++)
            {
		if (requirements[i].root == null) continue;

                if (i < quest.ReqCount)
                {
                    requirements[i].root.SetActive(true);
                    // Yazı bileşeni boş değilse güncelle (Sıfır-GC)
                    if (requirements[i].progressText != null)
                    {
                        requirements[i].progressText.SetText("{0} / {1}", quest.CurrentAmounts[i], quest.TargetAmounts[i]);
                    }
                    
                    // İkon bileşeni boş değilse güncelle
                    if (requirements[i].itemIcon != null)
                    {
                        Sprite icon = mainPanel.GetItemIcon(quest.ReqTypes[i]);
                        if (icon != null)
                        {
                            requirements[i].itemIcon.sprite = icon;
                        }
                    }
                }
                else
                {
                    requirements[i].root.SetActive(false); // Eğer görevde sadece tek tip eşya isteniyorsa 2. satırı gizle
                }
            }
// Ödülleri Güncelle (Null Kontrollü & Sıfır-GC)
            if (rewardCoinsText != null)
            {
                rewardCoinsText.SetText("+{0}", quest.CoinReward);
            }

            if (rewardXpText != null)
            {
                rewardXpText.SetText("+{0} XP", quest.XpReward);
            }
        }
    }
}
