// Assets/Scripts/Buildings/IMachineStatus.cs
namespace HoneyBearExpress.Buildings
{
    public interface IMachineStatus
    {
        float GetProgress();
        bool IsUnconnected();
        bool IsClogged();
        bool IsActiveProcessing();
    }
}
