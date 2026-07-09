using UnityEngine;

namespace HoneyBearExpress.Buildings
{
    [CreateAssetMenu(fileName = "BuildingDefinition", menuName = "HoneyBearExpress/Building Definition")]
    public class BuildingDefinition : ScriptableObject
    {
        [SerializeField] private string buildingName;
        [SerializeField] private BuildingType buildingType;
        [SerializeField] private GameObject prefab;
        [SerializeField] private Mesh previewMesh;
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private bool canRotate;
        
        public string BuildingName => buildingName;
        public BuildingType BuildingType => buildingType;
        public GameObject Prefab => prefab;
        public Mesh PreviewMesh => previewMesh;
        public Vector2Int GridSize => gridSize;
        public bool CanRotate => canRotate;
    }
}
