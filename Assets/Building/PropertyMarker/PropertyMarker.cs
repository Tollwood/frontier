using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PropertyMarker : MonoBehaviour
{
    public GameObject polePrefab;
    public GameObject messageBoardPrefab;
    public Material meshMaterial;

    public int maxPoles = 4;

    private List<GameObject> poles = new List<GameObject>();
    private List<GameObject> polesOnMap = new List<GameObject>();
    private List<PropertyMesh> properties = new List<PropertyMesh>();

    private float increment = .2f;

    public float heightOffset { get; private set; } = 0f;
    private float meshHeight = 0f;
    private float minhHeight = 0f;
    private float maxHeight = 100f;
    public float poleHeight = 1.4f;
    public float minDistance = 1f;

    private void Awake()
    {
        GameObject go =  new GameObject();
        go.transform.parent = this.transform;
        PropertyMesh pm =  go.AddComponent<PropertyMesh>();
        properties.Add(pm);
    }
    private void Start()
    {
        EventManager.StartListening(Events.OnItemEquip, ActivatePropertyMarking);
        EventManager.StartListening(Events.OnItemUnEquip, DeactivatePropertyMarking);

    }

    private void ActivatePropertyMarking(System.Object obj)
    {
        Equipment equipment = (Equipment)obj;
        if (equipment.capabiltiy == Capability.PropertyMarking)
        {
            EventManager.StartListening(Events.OnExecutePrimaryAction,AddPole);
            EventManager.StartListening(Events.OnIncrease, OnIncreaseHeight);
            EventManager.StartListening(Events.OnDecrease, OnDecreaseHeight);
        }
    }
    private void DeactivatePropertyMarking(System.Object obj)
    {
        Equipment equipment = (Equipment)obj;
        if (equipment.capabiltiy == Capability.PropertyMarking)
        {
            EventManager.StopListening(Events.OnExecutePrimaryAction,AddPole);
            EventManager.StopListening(Events.OnIncrease, OnIncreaseHeight);
            EventManager.StopListening(Events.OnDecrease, OnDecreaseHeight);
        }
    }


    public void AddPole()
    {

        Transform playerTransform = PlayerManager.Instance.CurrentPlayer().transform;
        Vector3 newPosition = playerTransform.position + (playerTransform.forward * 0.5f);
        if (WithinMinDistance(newPosition))
        {
            return;
        }

        if (poles.Count == maxPoles)
        {
            poles.ForEach((GameObject obj) => Destroy(obj));
            poles.Clear();
            polesOnMap.ForEach((GameObject obj) => Destroy(obj));
            polesOnMap.Clear();
            properties.ForEach((PropertyMesh obj) => obj.Reset());

        }

        poles.Add(Instantiate(polePrefab, newPosition, Quaternion.identity, this.transform));
        // with offSet
        polesOnMap.Add(Instantiate(polePrefab, newPosition + new Vector3(PlacementManager.planningOffsetX,0,0), Quaternion.identity, FindObjectOfType<PlanningManager>().transform.parent));

        if (poles.Count == maxPoles)
        {
            List<Vector3> polesPositions = poles.Select((arg) => { return arg.transform.position; }).ToList();

            properties.ForEach((PropertyMesh pm) => pm.BuildMesh(polesPositions, CalcMeshHeight(polesPositions),meshMaterial));
            // BuildMesh(polesOnMap);
        }
        EventManager.TriggerEvent(Events.OnCreatePole, newPosition);
    }

    private bool WithinMinDistance(Vector3 newPosition)
    {
        foreach(GameObject pole in poles)
        {
            if(Vector3.Distance(newPosition,pole.transform.position)< minDistance)
            {
                return true;
            }
        }
        return false;
    }

    public void OnIncreaseHeight()
    {
        meshHeight = Mathf.Clamp(meshHeight + increment, minhHeight, maxHeight);
        List<Vector3> polesPositions = poles.Select((arg) => { return arg.transform.position; }).ToList();
        properties.ForEach((PropertyMesh pm) => pm.BuildMesh(polesPositions, meshHeight, meshMaterial));
    }

    public void OnDecreaseHeight()
    {
        meshHeight = Mathf.Clamp(meshHeight - increment, minhHeight, maxHeight);
        List<Vector3> polesPositions = poles.Select((arg) => { return arg.transform.position; }).ToList();
        properties.ForEach((PropertyMesh pm) => pm.BuildMesh(polesPositions, meshHeight, meshMaterial));
    }


    private float CalcMeshHeight(List<Vector3> polesPositions)
    {
        SetHighestPoleAsMinHeight(polesPositions);
        SetLowestPoleAsMaxHeight(polesPositions);
        meshHeight = ((minhHeight + maxHeight) / 2) + heightOffset;
        Mathf.Clamp(meshHeight, minhHeight, maxHeight);
        return meshHeight;
    }


    private void SetLowestPoleAsMaxHeight(List<Vector3> convexHull)
    {
        maxHeight = float.MaxValue;
        foreach (Vector3 v in convexHull)
        {
            if (v.y < minhHeight)
            {
                maxHeight = v.y;
            }
        }
        maxHeight += poleHeight;
    }

    private void SetHighestPoleAsMinHeight(List<Vector3> convexHull)
    {
        minhHeight = 0f;
        foreach (Vector3 v in convexHull)
        {
            if (v.y > minhHeight)
            {
                minhHeight = v.y;
            }
        }
        minhHeight += .1f;
    }
}

