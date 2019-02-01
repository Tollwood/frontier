using UnityEngine;

public class InteractableInventory : Interactable
{
    public int space;

    private InventoryManager inventoryManager;
    private Inventory inventory;
    private bool isOpen = false;

    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        if(inventory == null)
        inventory = inventoryManager.AddInventory(space);

    }
    public override string hint()
    {
        return " Open " + inventory.name + " inventory";
    }

    public override void Interact()
    {
        isOpen = !isOpen;
        if (isOpen)
            inventoryManager.OpenInventories(inventory.Id);
        else
            inventoryManager.CloseInventories();
    }
}
