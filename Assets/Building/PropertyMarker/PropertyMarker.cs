using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PropertyMarker : MonoBehaviour
{
    public GameObject polePrefab; 
    public GameObject messageBoardPrefab;

    public int maxPoles = 4;

    private  List<GameObject> poles = new List<GameObject>();
    private MeshFilter meshFilter;
    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    public void AddPole(Vector3 newPosition)
    {
        if(poles.Count == maxPoles)
        {
            poles.ForEach((GameObject obj) => Destroy(obj));
            poles.Clear();
            meshFilter.mesh = null;
        }

        GameObject pole =  Instantiate(polePrefab, newPosition, Quaternion.identity,this.transform);
        poles.Add(pole);
        if(poles.Count == maxPoles)
        {
            BuildMesh();
        }
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
        return convexHull.Select((Vector3 convexHullElement) => {
            convexHullElement.y += 0.5f;
            return convexHullElement; 
            }).ToList();
    }
}

