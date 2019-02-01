using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBuildingToPlace : MonoBehaviour
{
    public PlacementManager placementManager;

    public GameObject buildingPrefab;

    public void SetBuilding()
    {
        placementManager.SetBuildingToPlace(buildingPrefab);
    }
}
