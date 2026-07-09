using UnityEngine;

namespace HoneyBearExpress.Buildings
{
    public interface IItemReceiver
    {
        bool CanReceive(HoneyItem item);
        bool TryReceive(HoneyItem item);

        Transform ItemTransform { get; }
    }
}