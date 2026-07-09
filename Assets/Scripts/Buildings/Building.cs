using HoneyBearExpress.Grid;
using UnityEngine;

namespace HoneyBearExpress.Buildings
{
    public abstract class Building : MonoBehaviour
    {
        public GridPosition GridPosition { get; private set; }
        public int RotationIndex { get; private set; }
        
        public virtual void InitializePlacement(GridPosition position, int rotationIndex)
        {
            GridPosition = position;
            RotationIndex = rotationIndex;
        }
    }
}
