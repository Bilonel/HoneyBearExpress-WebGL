using UnityEngine;
using HoneyBearExpress.Buildings;
using HoneyBearExpress.Grid;
using HoneyBearExpress.Core;

namespace HoneyBearExpress.Placement
{
    public class BuildingPlacement : MonoBehaviour
    {
        [SerializeField] private BuildingDefinition currentBuildingDefinition;
        [SerializeField] private BuildingDefinition hiveDefinition;
        [SerializeField] private BuildingDefinition conveyorDefinition;
        [SerializeField] private BuildingDefinition extractorDefinition;
        [SerializeField] private PlacementPreview placementPreview;
        [SerializeField] private GridOccupancy gridOccupancy;
        [SerializeField] private ConveyorRegistry conveyorRegistry;
        [SerializeField] private WorldServices worldServices;
        
        private static readonly GridPosition[] NeighborOffsets = new[]
        {
            GridPosition.North,
            GridPosition.South,
            GridPosition.East,
            GridPosition.West
        };
        
        public BuildingDefinition CurrentBuildingDefinition => currentBuildingDefinition;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlaceBuilding();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentBuildingDefinition = hiveDefinition;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentBuildingDefinition = conveyorDefinition;
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                currentBuildingDefinition = extractorDefinition;
            }
        }
        
        private void PlaceBuilding()
        {
            if (currentBuildingDefinition == null)
            {
                return;
            }
            
            if (placementPreview == null)
            {
                return;
            }
            
            if (gridOccupancy == null)
            {
                return;
            }
            
            if (conveyorRegistry == null)
            {
                return;
            }
            
            if (worldServices == null)
            {
                return;
            }
            
            if (currentBuildingDefinition.Prefab == null)
            {
                return;
            }
            
            GridPosition gridPosition = placementPreview.CurrentGridPosition;
            
            if (gridOccupancy.IsOccupied(gridPosition))
            {
                return;
            }
            
            Vector3 position = placementPreview.CurrentWorldPosition;
            Quaternion rotation = placementPreview.CurrentRotation;
            GameObject building = Instantiate(currentBuildingDefinition.Prefab, position, rotation);
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
                connectable.RefreshConnections(conveyorRegistry, gridOccupancy);
                RefreshNeighborConnections(gridPosition, gridOccupancy);
            }
        }
        
        private void RefreshNeighborConnections(GridPosition position, GridOccupancy gridOccupancy)
        {
            foreach (GridPosition offset in NeighborOffsets)
            {
                GridPosition neighborPos = position + offset;
                GameObject building = gridOccupancy.GetBuilding(neighborPos);

                if (building != null &&
                    building.TryGetComponent<IConveyorConnectable>(out var connectable))
                {
                    connectable.RefreshConnections(conveyorRegistry, gridOccupancy);
                }    
            }
        }
    }
}
