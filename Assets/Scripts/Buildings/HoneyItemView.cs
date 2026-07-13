using UnityEngine;
using System;

namespace HoneyBearExpress.Buildings
{
    [Serializable]
    public struct HoneyItemVisualData
    {
        public HoneyItemType Type;
        public Mesh Mesh;
        public Material Material;
    }
    public class HoneyItemView : MonoBehaviour
    {
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private HoneyItemVisualData[] visualData; // Prefab'den doldurulacak
        public HoneyItem Item { get; private set; }
        
        private float _moveSpeed = 5f;
        private Vector3 _targetPosition;

        private HoneyItemViewPool _pool;
        private bool _releaseOnArrival;

        public void Initialize(HoneyItem item,HoneyItemViewPool pool)
        {
            Item = item;
            _targetPosition = transform.position;
            UpdateVisuals(item.Type); // İlk spawn olduğunda tipine göre görsel ayarla
            _pool=pool;
        }
          
        // Obje silinmeden, GC üretmeden görseli anında değiştirir
        public void UpdateVisuals(HoneyItemType type)
        {
            for (int i = 0; i < visualData.Length; i++)
            {
                if (visualData[i].Type == type)
                {
                    meshFilter.sharedMesh = visualData[i].Mesh;
                    meshRenderer.sharedMaterial = visualData[i].Material; // allocation engellemek için sharedMaterial!
                    break;
                }
            }
        }
        public void ReleaseDeferred()
        {
            _releaseOnArrival = true;
        }

        public void Release()
        {
            Item = null;
            _releaseOnArrival = false;
        }
        
        public void SetTargetPosition(Vector3 position)
        {
            _targetPosition = position;
        }
        
        public void SetPositionInstant(Vector3 position)
        {
            transform.position = position;
            _targetPosition = position;
        }
        
        public void SetMoveSpeed(float speed)
        {
            _moveSpeed = speed;
        }
        
        private void Update()
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _targetPosition,
                _moveSpeed * Time.deltaTime
            );

            // Hedefe ulaştıysa ve ertelenmiş silme aktifse havuza dön
            if (_releaseOnArrival && Vector3.Distance(transform.position,_targetPosition)<.02f)
            {
                if (_pool != null)
                {
                    _pool.Release(this);
                }
            }
        }
    }
}
