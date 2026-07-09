using UnityEngine;
using System.Collections.Generic;

namespace HoneyBearExpress.Buildings
{
    public class HoneyItemViewPool : MonoBehaviour
    {
        [SerializeField] private HoneyItemView prefab;
        [SerializeField] private int initialSize = 64;
        
        private readonly Queue<HoneyItemView> _pool = new Queue<HoneyItemView>();
        
        private void Awake()
        {
            if (prefab == null)
            {
                Debug.LogError("HoneyItemViewPool: Prefab is not assigned.", this);
                enabled = false;
                return;
            }
            
            Prewarm();
        }
        
        private void Prewarm()
        {
            for (int i = 0; i < initialSize; i++)
            {
                HoneyItemView view = Instantiate(prefab, transform);
                view.gameObject.SetActive(false);
                _pool.Enqueue(view);
            }
        }
        
        public HoneyItemView Get(HoneyItem item)
        {
            if (_pool.Count == 0)
            {
                // Havuz boşsa genişlet (Dinamic Expansion)
                Debug.Log("HoneyItemViewPool: Pool is empty, expanding 3 times...");
                for(int expanded=0;expanded<3;expanded++)
                {
                    HoneyItemView newView = Instantiate(prefab, transform);
                    newView.gameObject.SetActive(false);
                    _pool.Enqueue(newView);
                }
            }
            
            HoneyItemView view = _pool.Dequeue();
            view.gameObject.SetActive(true);
            view.Initialize(item);
            return view;
        }
        
        public void Release(HoneyItemView view)
        {
            view.Release();
            view.gameObject.SetActive(false);
            _pool.Enqueue(view);
        }
    }
}
