using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public static float planningOffsetX = 1000f;

    [Header("Rotation Settings")]
    public KeyCode rotateLeftKey = KeyCode.Q;
    public KeyCode rotateRightKey = KeyCode.E;
    public float rotationSpeed = 40f;
    private Camera cam;

    [Header("Placement Settings")]
    private bool validToPlace = true;
    private CollidingCheck collidingCheck;
    private GameObject buildingPrefab;
    internal void SetBuildingToPlace(GameObject newPrefab)
    {
        if (!isBuilding)
        {
            this.buildingPrefab = newPrefab;
        }
    }
    private Transform buildingToPlace;
    private MeshRenderer buildingMeshRenderer;

    [Header("Placement Settings")]
    public Material validMaterial;
    public Material invalidMaterial;
    public Material placedMaterial;

    private bool isBuilding = false;
    private bool placed = false;

    GameObject newBuilding;

    TerrainModificationManager terrainModificationManager;
    private void Start()
    {
        transform.parent.transform.position = new Vector3(planningOffsetX,0,0);
        terrainModificationManager = FindObjectOfType<TerrainModificationManager>();
    }

    public void StartBuilding()
    {
        if (!isBuilding)
        {
            isBuilding = true;
            cam = GameObject.FindGameObjectWithTag("planningModeCamera").GetComponent<Camera>();
            newBuilding = Instantiate(buildingPrefab, this.transform, this.transform.parent);
            collidingCheck = newBuilding.AddComponent<CollidingCheck>();
            buildingToPlace = newBuilding.transform;
            newBuilding.transform.localEulerAngles = Vector3.zero;
            buildingMeshRenderer = buildingToPlace.GetComponent<MeshRenderer>();
            Validate();
        }
    }

    public void StopPlacement()
    {
        InputManager.Instance.StopPlacementMode();
    }

    public void Confirm()
    {
        if (placed)
        {
            GameObject go = Instantiate(buildingPrefab, GetPositionForBuilding(), newBuilding.transform.rotation);
            go.AddComponent<MeshCollider>();
            PlaceBuildingOnMap();
            ClosePlanningMode();
        }
    }

    private void ClosePlanningMode()
    {
        isBuilding = false;
        placed = false;
        newBuilding = null;
        EventManager.TriggerEvent(Events.StopPlacementMode);
    }

    private void PlaceBuildingOnMap()
    {
        newBuilding.transform.parent = this.transform.parent;
        Destroy(newBuilding.GetComponent<CollidingCheck>());
        Destroy(newBuilding.GetComponent<Rigidbody>());
        MeshCollider newBuildingCollider = newBuilding.AddComponent<MeshCollider>();
        newBuildingCollider.convex = true;
        newBuildingCollider.isTrigger = true;
    }

    private Vector3 GetPositionForBuilding()
    {
        Vector3 pos = newBuilding.transform.position + new Vector3(-1 * planningOffsetX, 0, 0);

        float height = Terrain.activeTerrain.SampleHeight(pos);
        return new Vector3(pos.x, height, pos.z);
    }

    private void Update()
    {
        if (isBuilding)
        {
            if (!placed)
            {
                FollowMouse();
                RotateBuilding();
                Validate();
            }

            if (Input.GetMouseButtonDown(0) && validToPlace)
            {
                placed = true;
                ApplyMaterial(buildingMeshRenderer, placedMaterial);
            }
            if (Input.GetMouseButtonDown(1))
            {
                placed = false;
                Validate();
            }
        }
    }

    private void Validate()
    {
        validToPlace = collidingCheck.isValid;
        Material mat = validToPlace ? validMaterial : invalidMaterial;
        ApplyMaterial(buildingMeshRenderer, mat);

    }

    private void ApplyMaterial(MeshRenderer meshRenderer, Material mat)
    {
        meshRenderer.material = mat;
        MeshRenderer[] renderes = meshRenderer.gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer rend in renderes)
        {
            rend.material = mat;
        }
    }

    private void RotateBuilding()
    {
        if (Input.GetKey(rotateLeftKey) && !placed)
        {
            buildingToPlace.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.World);

        }
        if (Input.GetKey(rotateRightKey) && !placed)
        {
            buildingToPlace.Rotate(Vector3.down * Time.deltaTime * rotationSpeed, Space.World);
        }
    }

    private void FollowMouse()
    {
        Vector3 v3 = Input.mousePosition;
        v3.z = 0;
        v3 = cam.ScreenToWorldPoint(v3);
        buildingToPlace.position = new Vector3(v3.x, 0f, v3.z);
    }

    public void OnPolePlaced(System.Object obj)
    {
        Transform position = (Transform)obj;
        Debug.Log("OnCreatePole: " + position);
    }
}
