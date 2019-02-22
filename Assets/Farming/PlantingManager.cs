using System;
using System.Collections.Generic;
using UnityEngine;

public class PlantingManager : AbstractPrimaryActionManager
{
    private readonly string FILE_NAME = "plantingManager";
    private List<PlantData> plants = new List<PlantData>();
    private List<GameObject> plantsGo = new List<GameObject>();

    public override void Awake()
    {
        base.Awake();
        EventManager.StartListening(Events.OnSave, OnSave);
        EventManager.StartListening(Events.OnLoad, OnLoad);
    }

    private void Update()
    {
        for ( int i = plants.Count -1 ; i >=0; i--) {

            bool removed = RemoveDestroyed(i);
            if (removed) break;
            PlantData plant = plants[i];
            plant.timeGrowing += Time.deltaTime;
            if (plant.timeToGrow < plant.timeGrowing)
            {
                if (plant.nextPlant != "")
                {
                    Plant nextPlant = Resources.Load<Plant>(plant.nextPlant);
                    ReplacePlant(plant.position.ToVector3(), nextPlant, i);
                }
                else
                {
                    DestroyPlant(plant);
                }
            }
        }
    }

    private bool RemoveDestroyed(int index)
    {
        if (plantsGo[index] == null)
        {
            plantsGo.RemoveAt(index);
            plants.RemoveAt(index);
            return true;
        }
        return false;
    }

    private void ReplacePlant(Vector3 currentPos, Plant nextPlant, int index)
    {
        GameObject currentPlantGo = plantsGo[index];
        Destroy(currentPlantGo);
        GameObject plantGo = Instantiate(nextPlant.prefab, currentPos, Quaternion.identity);
        plantsGo[index] = plantGo;
        plants[index] = CreatePlantData(currentPos, nextPlant);

    }

    private void DestroyPlant(PlantData plant)
    {
        int index = plants.IndexOf(plant);
        GameObject plantGo = plantsGo[index];
        Destroy(plantGo);
        plantsGo.RemoveAt(index);
        plants.RemoveAt(index);
    }

    private void OnLoad()
    { 
        foreach(GameObject plantGo in plantsGo)
        {
            Destroy(plantGo);
        }
        PlantData[] array = SaveManager.Load<PlantData[]>(FILE_NAME);
        plantsGo = new List<GameObject>();
        plants = new List<PlantData>();
        foreach(PlantData plantData in array)
        {
            Plant plant = Resources.Load<Plant>(plantData.name);
            PlacePlant(plantData.position.ToVector3(), plant);
        }
    }

    private void OnSave()
    {
        SaveManager.Save(FILE_NAME, plants.ToArray());
    }

    protected override void ExecutePrimaryAction()
    {
        Vector3 currentPos = PlayerManager.Instance.CurrentPlayer().transform.position;
        if (OnValidGround(currentPos)) {
            Seed seed = (Seed) equipedItem;
            PlacePlant(currentPos, seed.plant);
            Debug.Log("Plant corn");
        }
        else
        {
            Debug.Log("cant plant if ground is not prepared");
        }
    }

    private void PlacePlant(Vector3 currentPos, Plant plant)
    {
        GameObject plantGo = Instantiate(plant.prefab, currentPos, Quaternion.identity);
        PlantData plantData = CreatePlantData(currentPos, plant);
        plantsGo.Add(plantGo);
        plants.Add(plantData);
    }

    private static PlantData CreatePlantData(Vector3 currentPos, Plant plant)
    {
        return new PlantData
        {
            name = plant.name,
            nextPlant = plant.nextPlant != null ? plant.nextPlant.name : "",
            timeToGrow = plant.timeToLife,
            timeGrowing = 0f,
            position = new SerializableVector3
            {
                x = currentPos.x,
                y = currentPos.y,
                z = currentPos.z
            }
        };
    }

    private bool OnValidGround(Vector3 currentPos)
    {
        Collider[] colliders = Physics.OverlapSphere(currentPos, 1, LayerMask.GetMask("Plants"));
        bool noOverlap = colliders.Length == 1;
        float[,,] splatmapData = TerrainHelper.GetSplatMapAt(currentPos, 1);
        bool onPreparedGround = splatmapData[0,0,1] >= 1;
        return noOverlap && onPreparedGround;
    }

    protected override Capability GetCapability()
    {
        return Capability.Planting;
    }
}
