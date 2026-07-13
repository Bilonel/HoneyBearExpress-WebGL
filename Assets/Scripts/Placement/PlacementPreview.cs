// Assets/Scripts/Placement/PlacementPreview.cs
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

        // YENİ EKLENEN: 3D Yön Oku Renderer referansı
        [SerializeField] private SpriteRenderer directionIndicatorRenderer;
        
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
            // ... (Mevcut null kontrolleri aynı kalacak) ...
            
            if (directionIndicatorRenderer == null)
            {
                Debug.LogError("PlacementPreview: DirectionIndicatorRenderer is not assigned.", this);
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

        public void SetVisible(bool isVisible)
        {
            if (previewMeshRenderer != null)
            {
                previewMeshRenderer.enabled = isVisible;
            }
            
            directionIndicatorRenderer.enabled = isVisible && (buildingDefinition != null && buildingDefinition.CanRotate);

            enabled = isVisible;
        }

        public void SetBuildingDefinition(BuildingDefinition definition)
        {
            buildingDefinition = definition;
            if (previewMeshFilter != null && definition != null)
            {
                previewMeshFilter.sharedMesh = definition.PreviewMesh;
            }
            UpdatePreviewRotation();
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

                // YENİ EKLENEN: Dinamik Yükseklik ve Gösterim Mantığı
                if (buildingDefinition != null && buildingDefinition.CanRotate  && directionIndicatorRenderer != null)
                {
                    // Okun yüksekliğini binaya özel set et
                    directionIndicatorRenderer.transform.localPosition = new Vector3(0f, buildingDefinition.PreviewHeight, 0f);
                    
                    // Bina dönebiliyorsa oku göster, dönmüyorsa gizle
                    directionIndicatorRenderer.enabled = true;
                }
                else directionIndicatorRenderer.enabled=false;
            }
        }
        
        private void UpdatePreviewRotation()
        {
            previewMeshFilter.transform.localRotation = CurrentRotation;
        }
        
        private void UpdatePreviewPosition()
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Mathf.Abs(ray.direction.y) > 0.0001f)
            {
                float t = -ray.origin.y / ray.direction.y;
                if (t >= 0)
                {
                    Vector3 hitPoint = ray.origin + ray.direction * t;
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
                Material targetMat = _isPlacementValid ? validPreviewMaterial : invalidPreviewMaterial;
                
                previewMeshRenderer.sharedMaterial = targetMat;

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
