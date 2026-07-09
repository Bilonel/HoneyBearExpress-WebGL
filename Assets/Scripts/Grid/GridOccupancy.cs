using UnityEngine;
using System.Collections.Generic;
using HoneyBearExpress.Grid;

namespace HoneyBearExpress.Grid
{
    public class GridOccupancy : MonoBehaviour
    {
        private Dictionary<GridPosition, GameObject> _occupiedCells;
        
        private void Awake()
        {
            _occupiedCells = new Dictionary<GridPosition, GameObject>();
        }
        
        public bool IsOccupied(GridPosition position)
        {
            return _occupiedCells.ContainsKey(position);
        }
        
        public bool TryRegister(GridPosition position, GameObject building)
        {
            if (building == null)
            {
                Debug.LogError("Cannot register a null building.", this);
                return false;
            }
            
            if (_occupiedCells.ContainsKey(position))
                return false;
            
            _occupiedCells.Add(position, building);
            return true;
        }
        
        public GameObject GetBuilding(GridPosition position)
        {
            _occupiedCells.TryGetValue(position, out GameObject building);
            return building;
        }
        
        public void Unregister(GridPosition position)
        {
            _occupiedCells.Remove(position);
        }
    }
}
