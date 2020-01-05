
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Property : MonoBehaviour
{

    PropertyMesh playerModeMesh;
    private bool showMesh = false;
    PropertyMesh mapMesh;

    public int maxPoles = 4;
    private float increment = .2f;

    public float heightOffset { get; private set; } = 0f;
    private float meshHeight = 0f;

    internal void ToggleShowMesh()
    {
        showMesh = !showMesh;
        DrawMesh();
    }

    private float minhHeight = 0f;
    private float maxHeight = 100f;
    public float poleHeight = 1.4f;
    public float minDistance = 1f;


    private List<GameObject> poles = new List<GameObject>();
    private List<GameObject> polesOnMap = new List<GameObject>();

    private void Awake()
    {
        GameObject go = new GameObject();
        go.transform.parent = this.transform;
        go.name = "propertyMesh playerMode";
        playerModeMesh = go.AddComponent<PropertyMesh>();

        GameObject pmGo = new GameObject();
        pmGo.transform.parent = this.transform;
        pmGo.name = "propertyMesh MapMode";
        mapMesh= pmGo.AddComponent<PropertyMesh>();
    }

    internal Material material;

    internal void Reset()
    {
        poles.ForEach((GameObject obj) => Destroy(obj));
        poles.Clear();
        polesOnMap.ForEach((GameObject obj) => Destroy(obj));
        polesOnMap.Clear();
        playerModeMesh.Remove();
        mapMesh.Remove();
    }

    internal void AddMarker(GameObject polePrefab, Vector3 newPosition)
    {
        GameObject pole = Instantiate(polePrefab, newPosition, Quaternion.identity, this.transform);
        Pole p = pole.GetComponent<Pole>();
        p.property = this;
        poles.Add(pole);
        // with offSet
        GameObject mapPole = Instantiate(polePrefab, newPosition + new Vector3(PlacementManager.planningOffsetX, 0, 0), Quaternion.identity, this.transform);
        polesOnMap.Add(mapPole.gameObject);
        p.mapPole = mapPole;
        DrawMesh();


    }

    private void DrawMesh()
    {
        if (poles.Count >= 3 && showMesh) {
            List<Vector3> polesPositions = poles.Select((arg) => { return arg.transform.position; }).ToList();
            playerModeMesh.BuildMesh(polesPositions, CalcMeshHeight(polesPositions), material);
            mapMesh.BuildMesh(polesOnMap.Select((arg) => { return arg.transform.position; }).ToList(), 10f, material);
        }
        else
        {
            mapMesh.Remove();
            playerModeMesh.Remove();
        }
    }

    internal void removeMarker(Pole pole)
    {
        polesOnMap.Remove(pole.mapPole.gameObject);
        Destroy(pole.mapPole.gameObject);
        poles.Remove(pole.gameObject);
        DrawMesh();
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

    public bool IsMarkerWithinMinDistance(Vector3 newPosition)

    {
        foreach (GameObject pole in poles)
        {
            if (Vector3.Distance(newPosition, pole.transform.position) < minDistance)
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
        playerModeMesh.BuildMesh(polesPositions, meshHeight, material);
    }

    public void OnDecreaseHeight()
    {
        meshHeight = Mathf.Clamp(meshHeight - increment, minhHeight, maxHeight);
        List<Vector3> polesPositions = poles.Select((arg) => { return arg.transform.position; }).ToList();
        playerModeMesh.BuildMesh(polesPositions, meshHeight, material);
    }
}

