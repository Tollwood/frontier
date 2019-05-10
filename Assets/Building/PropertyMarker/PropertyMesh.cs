using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PropertyMesh : MonoBehaviour
{

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private float meshHeight = 0f;


    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Remove()
    {
        meshFilter.mesh = new Mesh();
    }

    public void BuildMesh(List<Vector3> polesPositions, float meshHeight, Material material)
    {
        meshRenderer.material = material;
        this.meshHeight = meshHeight;
        
        List<Vector3> vertices = CreateVertices(polesPositions);
        // add height
        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;
        mesh.vertices = vertices.ToArray();
        mesh.triangles = CreateTriangles(vertices); ;

    }

    private int[] CreateTriangles(List<Vector3> convexHullpoints)
    {
        int[] triangles = new int[(convexHullpoints.Count - 2) * 3 * 2];

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

    private List<Vector3> CreateVertices(List<Vector3> positions)
    {

        List<Vector3> convexHull = JarvisMarchAlgorithm.GetConvexHull(positions);

        return convexHull.Select((Vector3 convexHullElement) => {
            convexHullElement.y = meshHeight;
            return convexHullElement; 
            }).ToList();
    }
}

