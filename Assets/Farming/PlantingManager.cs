using System;
using System.Collections.Generic;
using UnityEngine;

public class PlantingManager : AbstractPrimaryActionManager
{
    private readonly string FILE_NAME = "plantingManager";
    private List<Plants> plants = new List<Plants>();
    private List<GameObject> plantsGo = new List<GameObject>();

    public override void Awake()
    {
        base.Awake();
        EventManager.StartListening(Events.OnSave, OnSave);
        EventManager.StartListening(Events.OnLoad, OnLoad);
    }

    private void OnLoad()
    { 
        foreach(GameObject plantGo in plantsGo)
        {
            Destroy(plantGo);
        }
        Plants[] array = SaveManager.Load<Plants[]>(FILE_NAME);
        plants = new List<Plants>();
        foreach(Plants plant in array)
        {
            GameObject prefab = Resources.Load<GameObject>(plant.name);
            PlacePlant(plant.position.ToVector3(), prefab);
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
            PlacePlant(currentPos, equipedItem.Prefab);
            Debug.Log("Plant corn");
        }
        else
        {
            Debug.Log("cant plant if ground is not prepared");
        }
    }

    private void PlacePlant(Vector3 currentPos, GameObject prefab)
    {
        GameObject plantGo = Instantiate(prefab, currentPos, Quaternion.identity);
        plantsGo.Add(plantGo);
        Plants plant = new Plants
        {
            name = prefab.name,
            position = new SerializableVector3
            {
                x = currentPos.x,
                y = currentPos.y,
                z = currentPos.z
            }
        };
        plants.Add(plant);
    }

    private bool OnValidGround(Vector3 currentPos)
    {
        // no plant in given radius
        // ground is prepared
        Collider[] colliders = Physics.OverlapSphere(currentPos, 1, LayerMask.GetMask("Plants"));
        bool noOverlap = colliders.Length == 1;
        return noOverlap;
    }

    protected override Capability GetCapability()
    {
        return Capability.Planting;
    }
}
