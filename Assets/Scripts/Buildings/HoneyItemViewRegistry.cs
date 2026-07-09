using UnityEngine;
using System.Collections.Generic;

namespace HoneyBearExpress.Buildings
{
    public class HoneyItemViewRegistry : MonoBehaviour
    {
        private Dictionary<HoneyItem, HoneyItemView> _registry;
        
        private void Awake()
        {
            _registry = new Dictionary<HoneyItem, HoneyItemView>();
        }
        
        public void Register(HoneyItem item, HoneyItemView view)
        {
            if (item == null)
            {
                Debug.LogError("HoneyItemViewRegistry: Cannot register null item.", this);
                return;
            }
            
            if (view == null)
            {
                Debug.LogError("HoneyItemViewRegistry: Cannot register null view.", this);
                return;
            }
            
            if (_registry.ContainsKey(item))
            {
                Debug.LogWarning($"HoneyItemViewRegistry: Item {item.Id} is already registered.", this);
                return;
            }
            
            _registry[item] = view;
        }
        
        public void Unregister(HoneyItem item)
        {
            if (item == null)
            {
                return;
            }
            
            if (!_registry.ContainsKey(item))
            {
                Debug.LogWarning($"HoneyItemViewRegistry: Item {item.Id} is not registered.", this);
                return;
            }
            
            _registry.Remove(item);
        }
        
        public bool TryGetView(HoneyItem item, out HoneyItemView view)
        {
            return _registry.TryGetValue(item, out view);
        }
    }
}
