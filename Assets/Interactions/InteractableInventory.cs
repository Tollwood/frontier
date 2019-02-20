using UnityEngine;

public class InteractableInventory : Interactable
{
    public int space;
    public StackItem[] initialItems;

    private Inventory inventory;

    private void Start()
    {
        if(inventory == null)
        inventory = InventoryManager.Instance.AddInventory(space, name, initialItems);

    }
    public override string hint()
    {
        return " Open " + inventory.name + " inventory";
    }

    public override void Interact()
    {
        InventoryManager.Instance.OpenInventories(inventory.Id);
    }
}
