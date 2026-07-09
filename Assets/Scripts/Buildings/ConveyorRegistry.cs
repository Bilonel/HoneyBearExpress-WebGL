using UnityEngine;
using System.Collections.Generic;
using HoneyBearExpress.Grid;

namespace HoneyBearExpress.Buildings
{
    public class ConveyorRegistry : MonoBehaviour
    {
        private Dictionary<GridPosition, Conveyor> _conveyorsMap;
        private List<Conveyor> _conveyorsList; // Hızlı iterasyon için liste

        private void Awake()
        {
            _conveyorsMap = new Dictionary<GridPosition, Conveyor>();
            _conveyorsList = new List<Conveyor>(256); // Başlangıç kapasitesi
        }
        
        public void Register(Conveyor conveyor)
        {
            if (conveyor == null) return;
            
            if (_conveyorsMap.TryAdd(conveyor.GridPosition, conveyor))
            {
                _conveyorsList.Add(conveyor);
            }
        }
        
        public void Unregister(Conveyor conveyor)
        {
            if (conveyor == null) return;
            
            if (_conveyorsMap.Remove(conveyor.GridPosition))
            {
                _conveyorsList.Remove(conveyor); // Liste silme işlemi nadirdir (sadece bina yıkıldığında)
            }
        }
        
        public bool TryGetConveyor(GridPosition position, out Conveyor conveyor)
        {
            return _conveyorsMap.TryGetValue(position, out conveyor);
        }
        
        // SADECE iterasyon için List döndür
        public List<Conveyor> ActiveConveyors => _conveyorsList;
    }
}