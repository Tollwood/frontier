using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI amountText;

    public Item item { get; private set; }  // Current item in the slot

    internal int index;
    public Inventory inventory;

    private EquipmentManager equipmentManager;

    public void init(Inventory inventory, int index)
    {
        this.name = inventory.name + "-slot-" + index;
        this.inventory = inventory;
        this.index = index;
        item = inventory.items[index];
        if ( item== null)
        {
            return;
        }
        if(item.amount > 1)
        {
            amountText.gameObject.SetActive(true);
            amountText.text = item.amount + "";
        }
        icon.sprite = item.icon;
        icon.gameObject.SetActive(true);
    }

    private void Start()
    {
        equipmentManager = FindObjectOfType<EquipmentManager>();
    }

    public bool IsEmpty()
    {
        return item == null;
    }

    public void Equip()
    {
        if (item is Equipment)
            equipmentManager.Equip((Equipment)item);
    }
}