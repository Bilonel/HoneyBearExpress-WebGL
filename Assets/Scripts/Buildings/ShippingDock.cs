// Assets/Scripts/Buildings/ShippingDock.cs
using UnityEngine;
using HoneyBearExpress.Core;
using HoneyBearExpress.Grid;

namespace HoneyBearExpress.Buildings
{
    public class ShippingDock : Building, IWorldInitializable, IItemReceiver
    {
        private HoneyItemViewRegistry _viewRegistry;
        private HoneyItemViewPool _viewPool;

        public Transform ItemTransform => transform;
    	WorldServices worldServices;

        public void Initialize(WorldServices services)
        {
            _viewRegistry = services.HoneyItemViewRegistry;
            _viewPool = services.HoneyItemViewPool;

            // Kendi lokasyonunu hesapla ve sisteme kaydet
            GridPosition pos = services.GridSystem.WorldToGrid(transform.position);
            InitializePlacement(pos, 0); // 0 = Default Rotation
            services.GridOccupancy.TryRegister(pos, gameObject);
            
            worldServices=services;
        }

        public bool CanReceive(HoneyItem item)
        {
            // Base (Dock) her zaman eşya kabul edebilir
            return true;
        }

	public bool TryReceive(HoneyItem item)
	{
	    if (item == null) return false;

	    HoneyItemType deliveredType = item.Type;

	    if (_viewRegistry != null && _viewRegistry.TryGetView(item, out HoneyItemView view))
	    {
		if (item.CurrentConveyor != null)
		{
		    float speed = item.CurrentConveyor.MoveSpeed / item.Weight;
		    view.SetMoveSpeed(speed);
		}
		
		view.SetTargetPosition(ItemTransform.position);
		_viewRegistry.Unregister(item); 
		view.ReleaseDeferred();        
	    }

	    if (worldServices != null)
	    {
		// 1. Önce Sipariş sistemini kontrol et (Sipariş varsa eşya oraya teslim edilir)
		bool consumedByQuest = false;
		if (worldServices.QuestManager != null)
		{
		    consumedByQuest = worldServices.QuestManager.OnItemDelivered(deliveredType);
		}

		// 2. Eğer eşyayı hiçbir sipariş istemiyorsa, doğrudan normal fiyattan sat!
		if (!consumedByQuest && worldServices.PlayerStatsManager != null)
		{
		    worldServices.PlayerStatsManager.OnItemDelivered(deliveredType);
		}
	    }
	    
	    return true;
	}
    }
}
