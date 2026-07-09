using UnityEngine;
using HoneyBearExpress.Core;
using HoneyBearExpress.Grid;

namespace HoneyBearExpress.Buildings
{
    public class Hive : BuildingProcessor, IConveyorConnectable
    {
        [SerializeField] private int productionInterval = 10;
        
        private HoneyItemViewPool viewPool;
        private HoneyItemViewRegistry viewRegistry;
        private GridSystem gridSystem;
        private ConveyorRegistry conveyorRegistry;
        private Conveyor _outputConveyor;
        
        private static int _nextHoneyId = 0;
        
        public override void Initialize(WorldServices services)
        {
            base.Initialize(services);
            if(!isInitialized) return;

            viewPool = services.HoneyItemViewPool;
            viewRegistry = services.HoneyItemViewRegistry;
            gridSystem = services.GridSystem;
            conveyorRegistry = services.ConveyorRegistry;
            
            if (viewPool == null)
            {
                Debug.LogError("Hive: ViewPool is not provided by WorldServices.", this);
                enabled = false;
            }
            
            if (viewRegistry == null)
            {
                Debug.LogError("Hive: ViewRegistry is not provided by WorldServices.", this);
                enabled = false;
            }
            
            if (gridSystem == null)
            {
                Debug.LogError("Hive: GridSystem is not provided by WorldServices.", this);
                enabled = false;
            }
            
            if (conveyorRegistry == null)
            {
                Debug.LogError("Hive: ConveyorRegistry is not provided by WorldServices.", this);
                enabled = false;
            }
        }
        
        public void RefreshConnections(ConveyorRegistry registry,GridOccupancy gridOccupancy)
        {
            GridPosition outputPosition = GetOutputPosition();
            registry.TryGetConveyor(outputPosition, out _outputConveyor);
        }
        
        private GridPosition GetOutputPosition()
        {
            GridPosition direction = GetDirectionFromRotation(RotationIndex);
            return new GridPosition(GridPosition.X + direction.X, GridPosition.Y + direction.Y);
        }
        
        private GridPosition GetDirectionFromRotation(int rotationIndex)
        {
            return rotationIndex switch
            {
                0 => GridPosition.North,
                1 => GridPosition.East,
                2 => GridPosition.South,
                3 => GridPosition.West,
                _ => GridPosition.North
            };
        }
        
        protected override void ProcessTick(long tick)
        {
            if (tick % productionInterval != 0)
            {
                return;
            }
            
            if (_outputConveyor == null)
            {
                return;
            }
            
            if (_outputConveyor.HasItem)
            {
                return;
            }
            
            HoneyItem item = new HoneyItem(_nextHoneyId++, HoneyItemType.Honeycomb);
            bool inserted = _outputConveyor.TryInsert(item);
            
            if (inserted)
            {
                HoneyItemView view = viewPool.Get(item);
                if (view != null)
                {
                    view.SetPositionInstant(transform.position);
                    view.SetMoveSpeed(_outputConveyor.MoveSpeed / item.Weight);
                    view.SetTargetPosition(gridSystem.GridToWorld(_outputConveyor.GridPosition));
                    viewRegistry.Register(item, view);
                }
            }
        }
    }
}
