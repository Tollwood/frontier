﻿using UnityEngine;

public class PlacementManager : MonoBehaviour
{

    private float planningOffset = 4000f;


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

    private void Start()
    {
        transform.parent.transform.position = new Vector3(planningOffset,0,0);
    }

    public void StartBuilding()
    {
        if (!isBuilding)
        {

            isBuilding = true;
            cam = GameObject.FindGameObjectWithTag("planningModeCamera").GetComponent<Camera>();
            newBuilding = Instantiate(buildingPrefab, this.transform);
            collidingCheck = newBuilding.AddComponent<CollidingCheck>();
            buildingToPlace = newBuilding.transform;
            newBuilding.transform.localEulerAngles = Vector3.zero;
            buildingMeshRenderer = buildingToPlace.GetComponent<MeshRenderer>();
            Validate();
        }
    }

    public void AbortBuilding()
    {
        isBuilding = false;
        placed = false;
        Destroy(newBuilding);
    }

    public void Confirm()
    {
        if (placed)
        {
            Ray ray = new Ray(new Vector3(newBuilding.transform.position.x - planningOffset, 3000, newBuilding.transform.position.z), Vector3.down);
            RaycastHit hit;

            if(Physics.Raycast(ray,out hit))
            {
                GameObject go = Instantiate(buildingPrefab, hit.point, newBuilding.transform.rotation);
                go.AddComponent<MeshCollider>();
            }

            newBuilding.transform.parent = null;
            Destroy(newBuilding.GetComponent<CollidingCheck>());
            Destroy(newBuilding.GetComponent<Rigidbody>());
            MeshCollider collider = newBuilding.AddComponent<MeshCollider>();
            collider.convex = true;
            collider.isTrigger = true;

            isBuilding = false;
            placed = false;
            newBuilding = null;
        }
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

    private void ApplyMaterial(MeshRenderer renderer, Material mat)
    {
        renderer.material = mat;
        MeshRenderer[] renderes = renderer.gameObject.GetComponentsInChildren<MeshRenderer>();
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
        var v3 = Input.mousePosition;
        v3.z = 0;
        v3 = cam.ScreenToWorldPoint(v3);
        buildingToPlace.position = new Vector3(v3.x, 0f, v3.z);
    }
}