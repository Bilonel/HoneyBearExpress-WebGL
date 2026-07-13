using UnityEngine;
using HoneyBearExpress.Core;
using HoneyBearExpress.Grid;

namespace HoneyBearExpress.Buildings
{
    public class Extractor : BuildingProcessor, IConveyorConnectable, IItemReceiver,IMachineStatus
    {
        [Header("Production Settings")]
        [SerializeField] private int processingTicks = 20; // İşlem süresi

        public GridPosition InputPosition { get; private set; }
        public GridPosition OutputPosition { get; private set; }
        
        public Conveyor InputConveyor { get; private set; }
        public Conveyor OutputConveyor { get; private set; }

        private HoneyItem _currentItem;
        private int _currentProcessTimer;

        public bool HasItem => _currentItem != null;
        public HoneyItem CurrentItem => _currentItem;
        public Transform ItemTransform => transform;
        
        public string Status ="Idle";
        public string CurrentItemType ="";
        void Update()
        {
            Status = _currentItem != null ? "Processing" : "Idle";
            CurrentItemType = _currentItem != null ? _currentItem.Type.ToString() : "";
        }
        private HoneyItemViewRegistry _viewRegistry;
        private GridSystem _gridSystem;

        public override void Initialize(WorldServices services)
        {
            base.Initialize(services);
            if (!isInitialized) return;

            _viewRegistry = services.HoneyItemViewRegistry;
            _gridSystem = services.GridSystem;
        }
        public override void InitializePlacement(GridPosition position, int rotationIndex)
        {
            base.InitializePlacement(position, rotationIndex);
            CalculateInputOutputPositions();
        }
        
        private void CalculateInputOutputPositions()
        {
            GridPosition direction = GetDirectionFromRotation(RotationIndex);
            OutputPosition = new GridPosition(
                GridPosition.X + direction.X,
                GridPosition.Y + direction.Y
            );
            InputPosition = new GridPosition(
                GridPosition.X - direction.X,
                GridPosition.Y - direction.Y
            );
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
        
        public void RefreshConnections(ConveyorRegistry registry,GridOccupancy gridOccupancy)
        {
            registry.TryGetConveyor(InputPosition, out Conveyor input);
            registry.TryGetConveyor(OutputPosition, out Conveyor output);
            
            InputConveyor = input;
            OutputConveyor = output;
        }
        private void TryPushOutput()
        {
            if (OutputConveyor == null) return;
            
            if (OutputConveyor.TryInsert(_currentItem))
            {
                // Mantıksal aktarım başarılı, şimdi görseli (View) güncelle:
                if (_viewRegistry != null && _viewRegistry.TryGetView(_currentItem, out HoneyItemView view))
                {
                    float moveSpeed = OutputConveyor.MoveSpeed / _currentItem.Weight;
                    view.SetMoveSpeed(moveSpeed);
                    view.SetTargetPosition(_gridSystem.GridToWorld(OutputConveyor.GridPosition));
                }

                _currentItem = null; // Makineyi boşalt
            }
        }

        protected override void ProcessTick(long tick)
        {
            if (!HasItem)
            {
                return;
            }

            if (_currentItem.Type == HoneyItemType.Honeycomb)
            {
                _currentProcessTimer++;
                
                if (_currentProcessTimer >= processingTicks)
                {
                    _currentItem.ChangeType(HoneyItemType.FilteredHoney);
                    // Mantıksal veri değişti, şimdi görseli güncelle:
                    if (_viewRegistry != null && _viewRegistry.TryGetView(_currentItem, out HoneyItemView view))
                    {
                        view.UpdateVisuals(HoneyItemType.FilteredHoney);
                    }
                }
            }
            else if (_currentItem.Type == HoneyItemType.FilteredHoney)
            {
                TryPushOutput();
            }
        }
        public bool CanReceive(HoneyItem item)
        {
            return !HasItem && item.Type==HoneyItemType.Honeycomb;
        }

        public bool TryReceive(HoneyItem item)
        {
            if (!CanReceive(item))
            {
                return false;
            }

            _currentItem = item;
            _currentProcessTimer = 0; // Doğrudan aktarımda da sayacı sıfırla
            return true;
        }
        public float GetProgress()
{
    if (!HasItem || _currentItem.Type != HoneyItemType.Honeycomb) return 0f;
    return (float)_currentProcessTimer / processingTicks;
}

public bool IsUnconnected()
{
    // Giriş VEYA çıkış bandı bağlı değilse bağlantı eksiktir
    return InputConveyor == null || OutputConveyor == null;
}

public bool IsClogged()
{
    // Ürün hazır (FilteredHoney) ama çıkış bandına itilemiyor (çıkış bandı doluysa)
    return HasItem && 
           _currentItem.Type == HoneyItemType.FilteredHoney && 
           OutputConveyor != null && 
           OutputConveyor.HasItem;
}

public bool IsActiveProcessing()
{
    // Bağlantılar tamsa, tıkanıklık yoksa ve üretim sürüyorsa aktiftir
    return HasItem && !IsClogged() && !IsUnconnected();
}
    }
}
