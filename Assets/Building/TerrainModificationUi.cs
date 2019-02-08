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

    private void Start()
    {
        EventManager.StartListening(Events.OnItemEquip, OnEquip);
        EventManager.StartListening(Events.OnItemUnEquip, OnUnEquip);

    }

    private void Update()
    {
        if (!isActive)
            return;
        currentHeight.text = TerrainModificationManager.Instance.CurrentHeightInMeter() + "m";
        desiredHeight.text = TerrainModificationManager.Instance.DesiredHeightInMeter() + "m";
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

    public void OnUnEquip(System.Object obj)
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
