using System;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    private InventoryUi primaryInventoryUi;
    private InventoryUi secondaryInventoryUi;

    public bool isOpen { get; private set; } = false;

    private readonly string FILE_NAME = "inventoryManager";

    private InventeryManagmentData imd;

    internal bool AddToCurrentInventtory(StackItem item)
    {
        return GetCurrentInventory().Add(item);
    }

    private void Awake()
    {
        imd = new InventeryManagmentData();
    }

    private void Start()
    {
        primaryInventoryUi = GameObject.FindWithTag("primaryInventoryUi").GetComponent<InventoryUi>();
        secondaryInventoryUi = GameObject.FindWithTag("secondaryInventoryUi").GetComponent<InventoryUi>();
        EventManager.StartListening(Events.NewPlayer, OnNewPlayer);
        EventManager.StartListening(Events.OnPlayerChanged, OnPlayerChanged);
        EventManager.StartListening(Events.OnSave, onSave);
        EventManager.StartListening(Events.OnLoad, onLoad);
    }

    public void OnPlayerChanged(object obj)
    {
        imd.currentPlayerId = ((PlayerData)obj).id;
        CloseInventories();
    }

    public void OnNewPlayer(object obj)
    {
        PlayerData playerData = (PlayerData) obj;
        if (!imd.inventories.TryGetValue(playerData.id, out Inventory inventory))
        {
            AddInventory(playerData.id ,10, playerData.name, null);
        }
    }


    internal Inventory GetCurrentInventory()
    {
        return GetInventory(imd.currentPlayerId);
    }


    internal void OpenInventories(string id)
    {
        isOpen = true;
        primaryInventoryUi.OnOpenInventory(GetCurrentInventory());
        secondaryInventoryUi.OnOpenInventory(GetInventory(id));
        EventManager.TriggerEvent(Events.OnOpenInventory);
    }

    internal void CloseInventories()
    {
        EventManager.TriggerEvent(Events.OnCloseInventory);
        isOpen = false;
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            primaryInventoryUi.OnOpenInventory(GetCurrentInventory());
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
    internal Inventory AddInventory(int space, string inventoryName, StackItem[] items)
    {
       return AddInventory(Guid.NewGuid().ToString(), space, inventoryName, items);
    }

    internal Inventory AddInventory( string id, int space, string inventoryName, StackItem[] items)
    {
        Inventory inventory = new Inventory(id, space, inventoryName);
        if(items != null && items.Length == space)
        {
            for (int i = 0; i < space; i++)
            {
                inventory.items[i] = items[i];
            }
        }
        imd.inventories.Add(id, inventory);
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
        imd.inventories.TryGetValue(id, out Inventory inventory);
        return inventory;
    }

    private void onLoad()
    {
        imd = SaveManager.Load<InventeryManagmentData>(FILE_NAME);
    }
    private void onSave()
    {
        SaveManager.Save(FILE_NAME, imd);
    }
}
