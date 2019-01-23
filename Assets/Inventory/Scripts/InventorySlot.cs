using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* Sits on all InventorySlots. */

public class InventorySlot : MonoBehaviour
{

    public Image icon;

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
        //set in inventory
        item = newItem;
        icon.sprite = item.icon;
        icon.gameObject.SetActive(true);
    }

}