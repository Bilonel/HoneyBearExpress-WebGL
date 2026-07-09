using UnityEngine;
using HoneyBearExpress.Buildings;
using HoneyBearExpress.Grid;
using System.Collections.Generic;

namespace HoneyBearExpress.Core
{
    public class WorldServices : MonoBehaviour
    {
        [SerializeField] private TickManager tickManager;
        [SerializeField] private GridSystem gridSystem;
        [SerializeField] private GridOccupancy gridOccupancy;
        [SerializeField] private ConveyorRegistry conveyorRegistry;
        [SerializeField] private HoneyItemViewPool honeyItemViewPool;
        [SerializeField] private HoneyItemViewRegistry honeyItemViewRegistry;
        
        public TickManager TickManager => tickManager;
        public GridSystem GridSystem => gridSystem;
        public GridOccupancy GridOccupancy => gridOccupancy;
        public ConveyorRegistry ConveyorRegistry => conveyorRegistry;
        public HoneyItemViewPool HoneyItemViewPool => honeyItemViewPool;
        public HoneyItemViewRegistry HoneyItemViewRegistry => honeyItemViewRegistry;
        
        [SerializeField] ConveyorSystem initializable1;
        private void Awake()
        {
            if (tickManager == null)
            {
                Debug.LogError("WorldServices: TickManager is not assigned.", this);
            }
            
            if (gridSystem == null)
            {
                Debug.LogError("WorldServices: GridSystem is not assigned.", this);
            }
            
            if (gridOccupancy == null)
            {
                Debug.LogError("WorldServices: GridOccupancy is not assigned.", this);
            }
            
            if (conveyorRegistry == null)
            {
                Debug.LogError("WorldServices: ConveyorRegistry is not assigned.", this);
            }
            
            if (honeyItemViewPool == null)
            {
                Debug.LogError("WorldServices: HoneyItemViewPool is not assigned.", this);
            }
            
            if (honeyItemViewRegistry == null)
            {
                Debug.LogError("WorldServices: HoneyItemViewRegistry is not assigned.", this);
            }
        }
        void Start()
        {
            initializable1.Initialize(this);
        }
    }
}
