using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private InventoryUi primaryInventoryUi;
    private InventoryUi secondaryInventoryUi;

    bool isOpen = false;
    PlayerManager playerManager;

    private Dictionary<string, Inventory> inventories = new Dictionary<string, Inventory>();

    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        primaryInventoryUi = GameObject.FindWithTag("primaryInventoryUi").GetComponent<InventoryUi>();
        secondaryInventoryUi = GameObject.FindWithTag("secondaryInventoryUi").GetComponent<InventoryUi>();
    }

    internal void OpenInventories(string id)
    {
        primaryInventoryUi.OnOpenInventory(playerManager.GetCurrentInventory());
        secondaryInventoryUi.OnOpenInventory(GetInventory(id));
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    internal void CloseInventories()
    {
        primaryInventoryUi.OnCloseInventory();
        secondaryInventoryUi.OnCloseInventory();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            primaryInventoryUi.OnOpenInventory(playerManager.GetCurrentInventory());
            //freeze Camera
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            primaryInventoryUi.OnCloseInventory();
            //rotate Camera
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    internal Inventory AddInventory(int space)
    {
        string id = Guid.NewGuid().ToString();
        Inventory inventory = new Inventory(id, space);
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
