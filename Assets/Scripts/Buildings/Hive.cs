using UnityEngine;
using HoneyBearExpress.Core;
using HoneyBearExpress.Grid;

namespace HoneyBearExpress.Buildings
{
    public class Hive : BuildingProcessor, IConveyorConnectable,IMachineStatus
    {
        [SerializeField] private int productionInterval = 10;
        
        private HoneyItemViewPool viewPool;
        private HoneyItemViewRegistry viewRegistry;
        private GridSystem gridSystem;
        private ConveyorRegistry conveyorRegistry;
        private Conveyor _outputConveyor;
        private int _currentTicksElapsed = 0; // İlerlemeyi (Progress) ölçmek için sayaç

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
             _currentTicksElapsed++;

            if (_outputConveyor == null || _outputConveyor.HasItem)
            {
                // Tıkalı veya bağlantı yoksa sayacı ilerletme, bekle
                _currentTicksElapsed = Mathf.Clamp(_currentTicksElapsed - 1, 0, productionInterval);
                return;
            }
            
            if (_currentTicksElapsed >= productionInterval)
            {
                ProduceItem();
                _currentTicksElapsed = 0;
            }
        }
        
        
        private void ProduceItem()
        {
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

        // --- IMachineStatus Arayüz Metotları ---

        public float GetProgress()
        {
            return (float)_currentTicksElapsed / productionInterval;
        }

        public bool IsUnconnected()
        {
            return _outputConveyor == null;
        }

        public bool IsClogged()
        {
            return _outputConveyor != null && _outputConveyor.HasItem;
        }

        public bool IsActiveProcessing()
        {
            return _outputConveyor != null && !_outputConveyor.HasItem;
        }
    }
}
