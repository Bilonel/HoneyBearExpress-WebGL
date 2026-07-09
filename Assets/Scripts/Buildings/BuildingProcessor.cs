using UnityEngine;
using HoneyBearExpress.Core;

namespace HoneyBearExpress.Buildings
{
    public abstract class BuildingProcessor : Building, IWorldInitializable,ITickable
    {
        protected TickManager tickManager;
        protected bool isInitialized = false;
        
        public virtual void Initialize(WorldServices services)
        {
            tickManager = services.TickManager;
            
            if (tickManager == null)
            {
                Debug.LogError("BuildingProcessor: TickManager is not provided by WorldServices.", this);
                enabled = false;
                return;
            }
            
            tickManager.RegisterTickable(this);
            isInitialized = true;
        }
        
        protected virtual void OnDestroy()
        {
            if (isInitialized && tickManager != null)
            {
                tickManager.UnregisterTickable(this);
            }
        }
        
        public void OnTick(long tickCount)
        {
            ProcessTick(tickCount);
        }
        
        protected abstract void ProcessTick(long tick);
    }
}
