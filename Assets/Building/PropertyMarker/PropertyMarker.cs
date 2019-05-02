using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PropertyMarker : MonoBehaviour
{
    public GameObject polePrefab;
    public Material meshMaterial;

    private Property property;

    private void Awake()
    {
        GameObject go =  new GameObject();
        go.transform.parent = this.transform;
        property =  go.AddComponent<Property>();
        property.material = meshMaterial;

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
        if (property.IsMarkerWithinMinDistance(newPosition))
        {
            return;
        }

        property.AddMarker(polePrefab, newPosition);

        EventManager.TriggerEvent(Events.OnCreatePole, newPosition);
    }


    public void OnIncreaseHeight()
    {
        property.OnIncreaseHeight();

    }

    public void OnDecreaseHeight()
    {
        property.OnDecreaseHeight();
    }
}

