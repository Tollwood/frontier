using UnityEngine;

public class InteractableInventory : Interactable
{
    public int space;
    public StackItem[] initialItems;

    private Inventory inventory;

    private void Start()
    {
        name = "inventory";
        icon = IconManager.Instance.GetIcon(IconManager.INVENTORY_ICON);
        if(inventory == null)
        inventory = InventoryManager.Instance.AddInventory(space, name, initialItems);

    }


    public override void Interact()
    {
        InventoryManager.Instance.OpenInventories(inventory.Id);
    }
}
