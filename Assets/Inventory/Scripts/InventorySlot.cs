using TMPro;
using UnityEngine;
using UnityEngine.UI;

/* Sits on all InventorySlots. */

public class InventorySlot : MonoBehaviour
{

    public Image icon;
    public TextMeshProUGUI amountText;

    public Item item { get; private set; }  // Current item in the slot

    public int index;
    public Inventory inventory;


    // Add item to the slot
    public void AddItem(Item newItem)
    {
        if(newItem == null)
        {
            return;
        }

        if(newItem.amount > 1)
        {
            amountText.gameObject.SetActive(true);
            amountText.text = newItem.amount + "";
        }
        //set in inventory
        item = newItem;
        icon.sprite = item.icon;
        icon.gameObject.SetActive(true);
    }

}