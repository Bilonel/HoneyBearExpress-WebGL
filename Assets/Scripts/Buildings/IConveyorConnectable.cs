using HoneyBearExpress.Grid;

namespace HoneyBearExpress.Buildings
{
    public interface IConveyorConnectable
    {
        GridPosition GridPosition { get; }
        int RotationIndex { get; }
        void RefreshConnections(ConveyorRegistry registry,GridOccupancy gridOccupancy);
    }
}
