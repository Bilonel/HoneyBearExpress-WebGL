// Assets/Scripts/UI/ConstructionMenuUI.cs
using UnityEngine;
using HoneyBearExpress.Core;
using HoneyBearExpress.Placement;
using HoneyBearExpress.Buildings;

namespace HoneyBearExpress.UI
{
    public class ConstructionMenuUI : MonoBehaviour, IWorldInitializable
    {
        [Header("Sub Panels (Alt Paneller)")]
        [SerializeField] private GameObject productionPanel;
        [SerializeField] private GameObject logisticsPanel;
        [SerializeField] private GameObject processingPanel;

        [Header("Building Definitions")]
        [SerializeField] private BuildingDefinition hiveDefinition;
        [SerializeField] private BuildingDefinition conveyorDefinition;
        [SerializeField] private BuildingDefinition extractorDefinition;
	
	[SerializeField] private GameObject BlockerPanel;
        private BuildingPlacement _placementSystem;

        public void Initialize(WorldServices services)
        {
            _placementSystem = services.BuildingPlacement;
            CloseAllSubPanels();
            Debug.Log("[ConstructionMenuUI] Initialized.");
        }

        // KATEGORİ BUTONLARI (Unity Inspector'dan OnClick olayına bağlanacak)
        
        public void ToggleProductionPanel() => TogglePanel(productionPanel);
        public void ToggleLogisticsPanel() => TogglePanel(logisticsPanel);
        public void ToggleProcessingPanel() => TogglePanel(processingPanel);

        // İNŞAAT BUTONLARI (Unity Inspector'dan OnClick olayına bağlanacak)
        
        public void SelectHive() => StartPlacement(hiveDefinition);
        public void SelectConveyor() => StartPlacement(conveyorDefinition);
        public void SelectExtractor() => StartPlacement(extractorDefinition);

        public void SelectDemolish()
        {
            CloseAllSubPanels();
            if (_placementSystem != null)
            {
                _placementSystem.ExitPlacementMode();
                // TODO: İleride silme/yıkım modunu buraya entegre edeceğiz
            }
            Debug.Log("[ConstructionMenuUI] Demolish mode clicked.");
        }

        private void TogglePanel(GameObject targetPanel)
        {
            if (targetPanel == null) return;

            bool isCurrentlyActive = targetPanel.activeSelf;
            
            // Önce tüm alt panelleri kapat (ekran temizlensin)
            CloseAllSubPanels();

            // Eğer tıklanan panel zaten açık değilse, onu aç (Toggle davranışı)
            if (!isCurrentlyActive)
            {
                targetPanel.SetActive(true);
            	BlockerPanel.SetActive(true);
            }
        }

        private void StartPlacement(BuildingDefinition definition)
        {
            if (_placementSystem == null || definition == null) return;

            // İnşaat modunu tetikle ve alt paneli kapat
            _placementSystem.EnterPlacementMode(definition);
            CloseAllSubPanels(); 
        }

        public void CloseAllSubPanels()
        {
            if (productionPanel != null) productionPanel.SetActive(false);
            if (logisticsPanel != null) logisticsPanel.SetActive(false);
            if (processingPanel != null) processingPanel.SetActive(false);
            if(BlockerPanel!=null) BlockerPanel.SetActive(false);
        }
    }
}
