using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TerrainModificationUi : MonoBehaviour
{
    public GameObject container;
    public Image icon;
    public TextMeshProUGUI currentHeight;
    public TextMeshProUGUI desiredHeight;

    private bool isActive = false;
    TerrainModificationManager terrainModificationManager;

    private void Start()
    {
        EventManager.StartListening(Events.OnItemEquip, OnEquip);
        EventManager.StartListening(Events.OnItemUnEquip, OnUnEquip);
        terrainModificationManager = FindObjectOfType<TerrainModificationManager>();
    }

    private void Update()
    {
        if (!isActive)
            return;
        currentHeight.text = terrainModificationManager.CurrentHeightInMeter() + "m";
        desiredHeight.text = terrainModificationManager.DesiredHeightInMeter() + "m";
    }

    public void OnEquip(System.Object obj)
    {
        Equipment equipment = (Equipment)obj;

        if(equipment.capabiltiy == Capability.Digging)
        {
            isActive = true;
            container.SetActive(true);
            icon.sprite = equipment.icon;
        }
    }

    public void OnUnEquip(object obj)
    {
        Equipment equipment = (Equipment)obj;
        if (equipment.capabiltiy == Capability.Digging)
        {
            isActive = false;
            container.SetActive(false);
            icon.sprite = null;
        }
    }
}
