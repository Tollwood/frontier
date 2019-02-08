using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PropertyMarker : MonoBehaviour
{

    public GameObject polePrefab;
    public GameObject messageBoardPrefab;

    public int maxPoles = 4;

    private List<GameObject> poles = new List<GameObject>();
    private MeshFilter meshFilter;
    private float increment = .2f;

    public float heightOffset { get; private set; } = 0f;
    private float meshHeight = 0f;
    private float minhHeight = 0f;
    private float maxHeight = 100f;
    public float poleHeight = 1.4f;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
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

        if (poles.Count == maxPoles)
        {
            poles.ForEach((GameObject obj) => Destroy(obj));
            poles.Clear();
            meshFilter.mesh = null;
        }

        GameObject pole =  Instantiate(polePrefab, newPosition, Quaternion.identity,this.transform);
        poles.Add(pole);
        if(poles.Count == maxPoles)
        {
            List<Vector3> polesPositions = poles.Select((arg) => { return arg.transform.position; }).ToList();
            SetHighestPoleAsMinHeight(polesPositions);
            SetLowestPoleAsMaxHeight(polesPositions);
            meshHeight = ((minhHeight + maxHeight) / 2) + heightOffset;
            BuildMesh();
        }

    }

    public void OnIncreaseHeight()
    {
        meshHeight = Mathf.Clamp(meshHeight + increment, minhHeight, maxHeight);
        BuildMesh();
    }

    public void OnDecreaseHeight()
    {
        meshHeight = Mathf.Clamp(meshHeight - increment, minhHeight, maxHeight);
        BuildMesh();
    }


    private void BuildMesh()
    {
        List<Vector3> vertices = CreateVertices();
        // add height
        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;
        mesh.vertices = vertices.ToArray();
        mesh.triangles = CreateTriangles(vertices); ;

    }

    private int[] CreateTriangles(List<Vector3> convexHullpoints)
    {
        int[] triangles = new int[(poles.Count - 2) * 3 * 2];

        int index = 0;
        for (int i = 2; i < convexHullpoints.Count; i++)
        {
            triangles[index] = 0;
            triangles[index+1] = i-1;
            triangles[index+2] = i;
            index += 3;
        }

        for (int i = 2; i < convexHullpoints.Count; i++)
        {
            triangles[index] = 0;
            triangles[index + 1] = i;
            triangles[index + 2] = i-1;
            index += 3;
        }
        return triangles;
    }

    private List<Vector3> CreateVertices()
    {
        List<Vector3> polesPositions = poles.Select((arg) => { return arg.transform.position; }).ToList();
        List<Vector3> convexHull = JarvisMarchAlgorithm.GetConvexHull(polesPositions);

        meshHeight = Mathf.Clamp(meshHeight, minhHeight, maxHeight);
        return convexHull.Select((Vector3 convexHullElement) => {
            convexHullElement.y = meshHeight;
            return convexHullElement; 
            }).ToList();
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
    }
}

