// Assets/Scripts/UI/QuestPanelUI.cs
using UnityEngine;
using HoneyBearExpress.Core;
using HoneyBearExpress.Buildings;

namespace HoneyBearExpress.UI
{
    public class QuestPanelUI : MonoBehaviour, IWorldInitializable
    {
        [Header("Quest Slots")]
        [SerializeField] private QuestSlotUI[] slots; // Tam olarak 3 adet slot atanacak

        [Header("Eşya İkonları Kütüphanesi")]
        [SerializeField] private ItemIconConfig[] itemIcons; // Hangi eşyanın hangi ikona sahip olacağı

        private QuestManager _questManager;

        public void Initialize(WorldServices services)
        {
            _questManager = services.QuestManager;

            if (_questManager != null)
            {
                _questManager.OnQuestsUpdated += RefreshPanel;
                RefreshPanel(); // İlk açılışta verileri çek
            }
        }

        private void OnDestroy()
        {
            if (_questManager != null)
            {
                _questManager.OnQuestsUpdated -= RefreshPanel;
            }
        }

        private void RefreshPanel()
        {
            if (_questManager == null) return;

            for (int i = 0; i < slots.Length; i++)
            {
                if (i < _questManager.ActiveQuests.Length)
                {
                    slots[i].UpdateVisual(_questManager.ActiveQuests[i], this);
                }
                else
                {
                    slots[i].gameObject.SetActive(false);
                }
            }
        }

        // Hızlı O(1) Eşya İkonu Arama
        public Sprite GetItemIcon(HoneyItemType type)
        {
            for (int i = 0; i < itemIcons.Length; i++)
            {
                if (itemIcons[i].Type == type)
                {
                    return itemIcons[i].Icon;
                }
            }
            return null;
        }
    }
}
