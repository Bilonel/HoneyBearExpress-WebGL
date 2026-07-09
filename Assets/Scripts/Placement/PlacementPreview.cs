using UnityEngine;
using HoneyBearExpress.Grid;
using HoneyBearExpress.Buildings;

namespace HoneyBearExpress.Placement
{
    public class PlacementPreview : MonoBehaviour
    {
        [SerializeField] private MeshFilter previewMeshFilter;
        [SerializeField] private MeshRenderer previewMeshRenderer;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private GridSystem gridSystem;
        [SerializeField] private Camera camera;
        [SerializeField] private float smoothTime = 0.08f;
        [SerializeField] private Material validPreviewMaterial;
        [SerializeField] private Material invalidPreviewMaterial;
        [SerializeField] private BuildingDefinition buildingDefinition;
        [SerializeField] private GridOccupancy gridOccupancy;
        
        private GridPosition _lastGridPosition;
        private Vector3 _targetPosition;
        private Vector3 _smoothVelocity;
        private BuildingDefinition _lastBuildingDefinition;
        private bool _isPlacementValid;
        private int _rotationIndex;
        
        public GridPosition CurrentGridPosition => _lastGridPosition;
        public Vector3 CurrentWorldPosition => _targetPosition;
        public bool IsPlacementValid => _isPlacementValid;
        public int CurrentRotationIndex => _rotationIndex;
        public Quaternion CurrentRotation => Quaternion.Euler(0f, _rotationIndex * 90f, 0f);
        
        private void Awake()
        {
            if (previewMeshFilter == null)
            {
                Debug.LogError("PlacementPreview: PreviewMeshFilter is not assigned.", this);
                enabled = false;
                return;
            }
            
            if (previewMeshRenderer == null)
            {
                Debug.LogError("PlacementPreview: PreviewMeshRenderer is not assigned.", this);
                enabled = false;
                return;
            }
            
            if (camera == null)
            {
                Debug.LogError("PlacementPreview: Camera is not assigned.", this);
                enabled = false;
                return;
            }
            
            if (gridSystem == null)
            {
                Debug.LogError("PlacementPreview: GridSystem is not assigned.", this);
                enabled = false;
                return;
            }
            
            if (validPreviewMaterial == null)
            {
                Debug.LogError("PlacementPreview: ValidPreviewMaterial is not assigned.", this);
                enabled = false;
                return;
            }
            
            if (invalidPreviewMaterial == null)
            {
                Debug.LogError("PlacementPreview: InvalidPreviewMaterial is not assigned.", this);
                enabled = false;
                return;
            }
            
            if (gridOccupancy == null)
            {
                Debug.LogError("PlacementPreview: GridOccupancy is not assigned.", this);
                enabled = false;
                return;
            }
            
            _targetPosition = previewMeshFilter.transform.position;
            _isPlacementValid = true;
            previewMeshRenderer.sharedMaterial = validPreviewMaterial;
        }
        
        private void Update()
        {
            HandleRotationInput();
            UpdateBuildingDefinition();
            UpdatePreviewPosition();
            SmoothMoveToTarget();
            UpdatePlacementValidity();
        }
        
        private void HandleRotationInput()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (buildingDefinition != null && buildingDefinition.CanRotate)
                {
                    _rotationIndex = (_rotationIndex + 1) % 4;
                    UpdatePreviewRotation();
                }
            }
        }
        
        private void UpdateBuildingDefinition()
        {
            if (buildingDefinition != _lastBuildingDefinition)
            {
                _lastBuildingDefinition = buildingDefinition;
                if (buildingDefinition != null && buildingDefinition.PreviewMesh != null)
                {
                    previewMeshFilter.sharedMesh = buildingDefinition.PreviewMesh;
                }
                UpdatePreviewRotation();
            }
        }
        
        private void UpdatePreviewRotation()
        {
            previewMeshFilter.transform.localRotation = CurrentRotation;
        }
        
        private void UpdatePreviewPosition()
        {
            // C++ tarafında optimize edilmiş bir şekilde origin ve direction'ı al
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            // Senin yazdığın Saf Matematik: Y = 0 düzlemiyle kesişim
            if (Mathf.Abs(ray.direction.y) > 0.0001f) // Sıfıra bölünme hatasını engelle
            {
                // t = -O.y / D.y
                float t = -ray.origin.y / ray.direction.y;
                
                // Sadece ileri doğru olan kesişimleri al (Kameranın arkasını hesaplama)
                if (t >= 0)
                {
                    // P = O + t * D
                    Vector3 hitPoint = ray.origin + ray.direction * t;
                    
                    // Grid koordinatlarına dönüştür
                    GridPosition gridPosition = gridSystem.WorldToGrid(hitPoint);
                    
                    if (gridPosition != _lastGridPosition)
                    {
                        _lastGridPosition = gridPosition;
                        _targetPosition = gridSystem.GridToWorld(gridPosition);
                    }
                }
            }
        }
        
        private void UpdatePlacementValidity()
        {
            
            bool isValid = !gridOccupancy.IsOccupied(_lastGridPosition);
            
            if (isValid != _isPlacementValid)
            {
                _isPlacementValid = isValid;
                previewMeshRenderer.sharedMaterial = _isPlacementValid ? validPreviewMaterial : invalidPreviewMaterial;
            }
        }
        
        private void SmoothMoveToTarget()
        {
            previewMeshFilter.transform.position = Vector3.SmoothDamp(
                previewMeshFilter.transform.position, 
                _targetPosition, 
                ref _smoothVelocity, 
                smoothTime
            );
        }
    }
}
