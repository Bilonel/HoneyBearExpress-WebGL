using UnityEngine;
using HoneyBearExpress.Core;
using HoneyBearExpress.Grid;
using System.Collections.Generic;

namespace HoneyBearExpress.Buildings
{
    public class ConveyorSystem : MonoBehaviour, IWorldInitializable, ITickable
    {
        private TickManager tickManager;
        private ConveyorRegistry conveyorRegistry;
        private HoneyItemViewRegistry viewRegistry;
        private bool isInitialized = false;
        
        private readonly List<Transfer> _transfers = new(256);
        private readonly HashSet<IItemReceiver> _reservedDestinations = new(256);
        
        private struct Transfer
        {
            public Conveyor Source;
            public IItemReceiver Destination;
        }
        
        private void Awake()
        {
        }
        
        public void Initialize(WorldServices services)
        {
            tickManager = services.TickManager;
            conveyorRegistry = services.ConveyorRegistry;
            viewRegistry = services.HoneyItemViewRegistry;
            
            if (tickManager == null)
            {
                Debug.LogError("ConveyorSystem: TickManager is not provided by WorldServices.", this);
                enabled = false;
                return;
            }
            
            if (conveyorRegistry == null)
            {
                Debug.LogError("ConveyorSystem: ConveyorRegistry is not provided by WorldServices.", this);
                enabled = false;
                return;
            }
            
            if (viewRegistry == null)
            {
                Debug.LogError("ConveyorSystem: ViewRegistry is not provided by WorldServices.", this);
                enabled = false;
                return;
            }
            
            tickManager.RegisterTickable(this);
            isInitialized = true;
        }
        
        private void OnDestroy()
        {
            if (isInitialized && tickManager != null)
            {
                tickManager.UnregisterTickable(this);
            }
        }
        
        public void OnTick(long tickCount)
        {
            var conveyors = conveyorRegistry.ActiveConveyors;
            for (int i = 0; i < conveyors.Count; i++)
            {
                Conveyor conveyor = conveyors[i];
                
                if (!conveyor.HasItem || conveyor.OutputReceiver == null || !conveyor.OutputReceiver.CanReceive(conveyor.CurrentItem))
                    continue;
                    
                if (_reservedDestinations.Contains(conveyor.OutputReceiver))
                    continue;

                _reservedDestinations.Add(conveyor.OutputReceiver);
                _transfers.Add(new Transfer { Source = conveyor, Destination = conveyor.OutputReceiver });
            }
            
            // Phase 2: Execute guaranteed transfers
            for (int i = 0; i < _transfers.Count; i++)
            {
                Transfer transfer = _transfers[i];

                HoneyItem item = transfer.Source.CurrentItem;

                if (!transfer.Destination.TryReceive(item))
                {
                    continue;
                }
                HoneyItem removedItem = transfer.Source.RemoveItem();

                if (!ReferenceEquals(item, removedItem))
                {
                    Debug.LogError("Conveyor item mismatch.");
                }

                // Synchronize visual position
                if (viewRegistry.TryGetView(item, out HoneyItemView view))
                {
                    float movementSpeed = transfer.Source.MoveSpeed / item.Weight;
                    view.SetMoveSpeed(movementSpeed);
                    view.SetTargetPosition(transfer.Destination.ItemTransform.position);
                }
            }
            
            _transfers.Clear();
            _reservedDestinations.Clear();
        }
    }
}
