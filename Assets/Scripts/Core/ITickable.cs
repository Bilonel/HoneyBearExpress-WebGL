namespace HoneyBearExpress.Core
{
    public interface ITickable
    {
        void OnTick(long tickCount);
    }
}