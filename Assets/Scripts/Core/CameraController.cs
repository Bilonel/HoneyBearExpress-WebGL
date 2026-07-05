using UnityEngine;

namespace HoneyBearExpress.Core
{
    public class CameraController : MonoBehaviour
    {
        [Header("Zoom Settings")]
        [SerializeField] private float minZoomDistance = 8f;
        [SerializeField] private float maxZoomDistance = 25f;
        [SerializeField] private float zoomSpeed = 5f;
        
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 10f;
        
        [Header("Camera Rotation")]
        [SerializeField] private Vector3 rotationEuler = new Vector3(35f, 45f, 0f);
        
        private Camera _camera;
        private bool _isDragging;
        private float _currentZoomDistance;
        
        // TODO: Replace Unity Input with project InputService for touch support
        
        private void Awake()
        {
            _camera = GetComponent<Camera>();
            
            transform.rotation = Quaternion.Euler(rotationEuler);
            _currentZoomDistance = transform.position.y;
        }
        
        private void Update()
        {
            HandleDragInput();
            HandleZoomInput();
        }
        
        private void HandleDragInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isDragging = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
            }
            
            if (_isDragging)
            {
                Vector3 mouseDelta = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0f);
                Vector3 movement = CalculateDragMovement(mouseDelta);
                transform.position += movement;
            }
        }
        
        private Vector3 CalculateDragMovement(Vector3 mouseDelta)
        {
            Vector3 right = transform.right;
            Vector3 forward = transform.forward;
            forward.y = 0f;
            forward.Normalize();
            
            float sensitivity = moveSpeed * Time.deltaTime;
            
            Vector3 movement = (-right * mouseDelta.x + -forward * mouseDelta.y) * sensitivity;
            movement.y = 0f;
            
            return movement;
        }
        
        private void HandleZoomInput()
        {
            float scrollDelta = Input.mouseScrollDelta.y;
            
            if (Mathf.Abs(scrollDelta) > 0.01f)
            {
                float zoomChange = scrollDelta * zoomSpeed * Time.deltaTime;
                _currentZoomDistance = Mathf.Clamp(_currentZoomDistance - zoomChange, minZoomDistance, maxZoomDistance);
                
                Vector3 forward = transform.forward;
                float yRatio = forward.y;
                
                if (Mathf.Abs(yRatio) > 0.001f)
                {
                    float distanceToMove = (transform.position.y - _currentZoomDistance) / yRatio;
                    transform.position += forward * distanceToMove;
                }
            }
        }
    }
}
