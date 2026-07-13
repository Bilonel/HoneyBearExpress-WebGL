using UnityEngine;

namespace HoneyBearExpress.Grid
{
    public class GridSystem : MonoBehaviour
    {
        [Header("Grid Settings")]
        [SerializeField, Min(0.01f)] private float cellSize = 1f;
        [SerializeField, Min(1)] private int gridWidth = 20;
        [SerializeField, Min(1)] private int gridHeight = 20;
        
        public float CellSize => cellSize;
        
        public GridPosition WorldToGrid(Vector3 worldPosition)
        {
            Vector3 localPosition = worldPosition - transform.position;
            int x = Mathf.FloorToInt(localPosition.x / cellSize);
            int y = Mathf.FloorToInt(localPosition.z / cellSize);
            return new GridPosition(x, y);
        }
        
        public Vector3 GridToWorld(GridPosition position)
        {
            float x = position.X * cellSize + cellSize * 0.5f;
            float z = position.Y * cellSize + cellSize * 0.5f;
            return transform.position + new Vector3(x, 0f, z);
        }
        
        private void OnDrawGizmos()
        {
            DrawGrid();
        }
        
        private void DrawGrid()
        {
            Gizmos.color = Color.white;
            
            float totalWidth = gridWidth * cellSize;
            float totalHeight = gridHeight * cellSize;
            Vector3 origin = transform.position;
            
            // Draw vertical lines
            for (int x = 0; x <= gridWidth; x++)
            {
                float xPos = x * cellSize;
                Vector3 start = origin + new Vector3(xPos, 0f, 0f);
                Vector3 end = origin + new Vector3(xPos, 0f, totalHeight);
                Gizmos.DrawLine(start, end);
            }
            
            // Draw horizontal lines
            for (int y = 0; y <= gridHeight; y++)
            {
                float zPos = y * cellSize;
                Vector3 start = origin + new Vector3(0f, 0f, zPos);
                Vector3 end = origin + new Vector3(totalWidth, 0f, zPos);
                Gizmos.DrawLine(start, end);
            }
        }
    }
}
