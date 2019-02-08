using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    private InventoryUi primaryInventoryUi;
    private InventoryUi secondaryInventoryUi;

    public bool isOpen { get; private set; } = false;

    private Dictionary<string, Inventory> inventories = new Dictionary<string, Inventory>();

    private void Start()
    {
        primaryInventoryUi = GameObject.FindWithTag("primaryInventoryUi").GetComponent<InventoryUi>();
        secondaryInventoryUi = GameObject.FindWithTag("secondaryInventoryUi").GetComponent<InventoryUi>();
    }

    internal void OpenInventories(string id)
    {
        isOpen = true;
        primaryInventoryUi.OnOpenInventory(PlayerManager.Instance.GetCurrentInventory());
        secondaryInventoryUi.OnOpenInventory(GetInventory(id));
        EventManager.TriggerEvent(Events.OnOpenInventory);
    }

    internal void CloseInventories()
    {
        EventManager.TriggerEvent(Events.OnCloseInventory);
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            primaryInventoryUi.OnOpenInventory(PlayerManager.Instance.GetCurrentInventory());
            EventManager.TriggerEvent(Events.OnOpenInventory);
        }
        else
        {
            EventManager.TriggerEvent(Events.OnCloseInventory);
        }
    }

    internal Inventory AddInventory(int space, string inventoryName) {
        return AddInventory(space, inventoryName, null);
    }

    internal Inventory AddInventory(int space, string inventoryName, Item[] items)
    {
        string id = Guid.NewGuid().ToString();
        Inventory inventory = new Inventory(id, space, inventoryName);
        if(items != null && items.Length == space)
        {
            for (int i = 0; i < space; i++)
            {
                inventory.items[i] = items[i];
            }
        }
        inventories.Add(id, inventory);
        return inventory;
    }

    internal void UpdateInventory(Inventory fromInventory, Inventory toInventory, int fromIndex, int toIndex)
    {
        if (fromInventory.Equals(toInventory))
        {
            fromInventory.SwapItem(fromIndex, toIndex);
        }
        else
        {
            toInventory.Add(fromInventory.items[fromIndex], toIndex);
            fromInventory.Remove(fromIndex);
        }
        primaryInventoryUi.UpdateUI();
        secondaryInventoryUi.UpdateUI();
    }

    internal Inventory GetInventory(string id)
    {
        inventories.TryGetValue(id, out Inventory inventory);
        return inventory;
    }

}
