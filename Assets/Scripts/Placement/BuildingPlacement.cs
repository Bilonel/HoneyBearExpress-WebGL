// Assets/Scripts/Placement/BuildingPlacement.cs
using UnityEngine;
using HoneyBearExpress.Buildings;
using HoneyBearExpress.Grid;
using HoneyBearExpress.Core;

namespace HoneyBearExpress.Placement
{
    public class BuildingPlacement : MonoBehaviour
    {
        [SerializeField] private PlacementPreview placementPreview;
        [SerializeField] private GridOccupancy gridOccupancy;
        [SerializeField] private ConveyorRegistry conveyorRegistry;
        [SerializeField] private WorldServices worldServices;
        
        private PlayerStatsManager _statsManager;
        private BuildingDefinition _currentBuildingDefinition;
        private bool _isPlacementActive = false;

        private static readonly GridPosition[] NeighborOffsets = new[]
        {
            GridPosition.North,
            GridPosition.South,
            GridPosition.East,
            GridPosition.West
        };
        
        public bool IsPlacementActive => _isPlacementActive;
        public BuildingDefinition CurrentBuildingDefinition => _currentBuildingDefinition;

        private void Start()
        {
            if (worldServices != null)
            {
                _statsManager = worldServices.PlayerStatsManager;
            }
            
            ExitPlacementMode(); // Başlangıçta inşaat modu kapalı
        }

        private void Update()
        {
            if (!_isPlacementActive) return;

            // Sol Tık: Yerleştir
            if (Input.GetMouseButtonDown(0))
            {
                PlaceBuilding();
            }

            // Sağ Tık veya ESC: İnşaat modundan çık
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
            {
                ExitPlacementMode();
            }
        }

        // UI Butonları bu fonksiyonu çağıracak
        public void EnterPlacementMode(BuildingDefinition definition)
        {
            if (definition == null) return;

            _currentBuildingDefinition = definition;
            _isPlacementActive = true;

            if (placementPreview != null)
            {
                // Önizlemeyi aktifleştir ve yeni bina tipini ata
                placementPreview.SetVisible(true);
                // NOT: PlacementPreview içindeki buildingDefinition alanını güncellemeliyiz:
                // Bunu yapmak için PlacementPreview'a yeni bir metod ekleyeceğiz (aşağıda görebilirsin).
                placementPreview.SetBuildingDefinition(definition);
            }
        }

        public void ExitPlacementMode()
        {
            _currentBuildingDefinition = null;
            _isPlacementActive = false;

            if (placementPreview != null)
            {
                placementPreview.SetVisible(false);
            }
        }
        
        private void PlaceBuilding()
        {
            if (_currentBuildingDefinition == null || placementPreview == null || 
                gridOccupancy == null || conveyorRegistry == null || _statsManager == null)
            {
                return;
            }

            // 1. Grid Kontrolü
            GridPosition gridPosition = placementPreview.CurrentGridPosition;
            if (gridOccupancy.IsOccupied(gridPosition))
            {
                Debug.LogWarning("Grid hücresi zaten dolu!");
                return;
            }

            // 2. Para Kontrolü ve Satın Alma (TrySpendCoins)
            if (!_statsManager.TrySpendCoins(_currentBuildingDefinition.Cost))
            {
                Debug.LogWarning("Yetersiz bakiye! İnşaat yapılamadı.");
                return; // Satın alamazsa inşaatı iptal et
            }
            
            // 3. İnşa Etme Logic'i (Aynı kalacak)
            Vector3 position = placementPreview.CurrentWorldPosition;
            Quaternion rotation = placementPreview.CurrentRotation;
            GameObject building = Instantiate(_currentBuildingDefinition.Prefab, position, rotation);
            gridOccupancy.TryRegister(gridPosition, building);
            
            IWorldInitializable initializable = building.GetComponent<IWorldInitializable>();
            if (initializable != null)
            {
                initializable.Initialize(worldServices);
            }
            
            Building buildingComponent = building.GetComponent<Building>();
            if (buildingComponent != null)
            {
                buildingComponent.InitializePlacement(gridPosition, placementPreview.CurrentRotationIndex);
            }
            
            Conveyor conveyor = building.GetComponent<Conveyor>();
            if (conveyor != null)
            {
                conveyorRegistry.Register(conveyor);
            }
            
            IConveyorConnectable connectable = building.GetComponent<IConveyorConnectable>();
            if (connectable != null)
            {
                connectable.RefreshConnections(conveyorRegistry,gridOccupancy);
                RefreshNeighborConnections(gridPosition);
            }
        }
        
        private void RefreshNeighborConnections(GridPosition position)
        {
            foreach (GridPosition offset in NeighborOffsets)
            {
                GridPosition neighborPos = position + offset;
                GameObject building = gridOccupancy.GetBuilding(neighborPos);

                if (building != null &&
                    building.TryGetComponent<IConveyorConnectable>(out var connectable))
                {
                    connectable.RefreshConnections(conveyorRegistry,gridOccupancy);
                }    
            }
        }
    }
}
