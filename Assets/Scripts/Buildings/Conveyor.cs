using UnityEngine;
using HoneyBearExpress.Grid;

namespace HoneyBearExpress.Buildings
{
    public class Conveyor : Building, IConveyorConnectable, IItemReceiver
    {
        [SerializeField] private float moveSpeed = 5f;
        
        public GridPosition InputPosition { get; private set; }
        public GridPosition OutputPosition { get; private set; }
        
        public float MoveSpeed => moveSpeed;
        
        public Conveyor InputConveyor { get; private set; }
        public IItemReceiver OutputReceiver { get; private set; }
        
        private HoneyItem _currentItem;
        
        public bool HasItem => _currentItem != null;
        public HoneyItem CurrentItem => _currentItem;
        public Transform ItemTransform => transform;

        [SerializeField] public string CurrentItemType ="";
        void Update()
        {
            CurrentItemType = _currentItem != null ? _currentItem.Type.ToString() : "";
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
        
        public void RefreshConnections(ConveyorRegistry registry, GridOccupancy gridOccupancy)
        {
            registry.TryGetConveyor(InputPosition, out Conveyor input);
            InputConveyor=input;

            GameObject building = gridOccupancy.GetBuilding(OutputPosition);

            if (building != null &&
                building.TryGetComponent<IItemReceiver>(out var receiver))
            {
                OutputReceiver = receiver;
            }
            else
            {
                OutputReceiver = null;
            }
        }
        
        public bool TryInsert(HoneyItem item)
        {
            if (_currentItem != null)
            {
                return false;
            }
            
            _currentItem = item;
            item.SetCurrentConveyor(this);
            return true;
        }
        
        public HoneyItem RemoveItem()
        {
            HoneyItem item = _currentItem;
            _currentItem = null;
            return item;
        }
        public bool CanReceive(HoneyItem item)
        {
            return !HasItem;
        }

        public bool TryReceive(HoneyItem item)
        {
            return TryInsert(item);
        }
    }
}
