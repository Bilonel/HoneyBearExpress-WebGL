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
        [SerializeField] private PlayerStatsManager playerStatsManager;        
        [SerializeField] private UI.PlayerStatsUI playerStatsUI;
	[SerializeField] private Placement.BuildingPlacement buildingPlacement;
	[SerializeField] private QuestManager questManager;
        public TickManager TickManager => tickManager;
        public GridSystem GridSystem => gridSystem;
        public GridOccupancy GridOccupancy => gridOccupancy;
        public ConveyorRegistry ConveyorRegistry => conveyorRegistry;
        public HoneyItemViewPool HoneyItemViewPool => honeyItemViewPool;
        public HoneyItemViewRegistry HoneyItemViewRegistry => honeyItemViewRegistry;
        public PlayerStatsManager PlayerStatsManager => playerStatsManager;
        public UI.PlayerStatsUI PlayerStatsUI => playerStatsUI;
	public Placement.BuildingPlacement BuildingPlacement => buildingPlacement;
	public QuestManager QuestManager => questManager;

        [SerializeField] ConveyorSystem conveyorSystem;
        [SerializeField] ShippingDock shippingDock;
        [SerializeField] private UI.LevelUpUI levelUpUI;
	[SerializeField] private UI.ConstructionMenuUI constructionMenuUI; // Yeni Eklenen
	[SerializeField] private UI.QuestPanelUI questPanelUI;
        
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
        	try
        	{
			shippingDock.Initialize(this);
			conveyorSystem.Initialize(this);
			playerStatsManager.Initialize(this);     
			playerStatsUI.Initialize(this);
			levelUpUI.Initialize(this);
			constructionMenuUI.Initialize(this);
			questManager.Initialize(this);
			questPanelUI.Initialize(this);
        	}
        	catch(System.Exception e)
        	{
        		Debug.Log("World Services : Some systems are null!");
        	}
        }
}}
