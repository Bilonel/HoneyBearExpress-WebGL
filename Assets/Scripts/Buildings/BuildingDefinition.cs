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
	[SerializeField] private int cost = 10;
	[SerializeField] private float previewHeight = 0.8f;
        
        public string BuildingName => buildingName;
        public BuildingType BuildingType => buildingType;
        public GameObject Prefab => prefab;
        public Mesh PreviewMesh => previewMesh;
        public Vector2Int GridSize => gridSize;
        public bool CanRotate => canRotate;
	public int Cost => cost;
	public float PreviewHeight => previewHeight;

    }
}
