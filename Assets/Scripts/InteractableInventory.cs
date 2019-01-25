using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class InteractableInventory : Interactable
{

    Inventory inventory;

    InventoryUi primaryInventoryUi;
    InventoryUi secondaryInventoryUI;
    Inventory playerInventory;

    bool isOpen = false;

    private void Start()
    {
        playerInventory = GameObject.FindWithTag("playerInventory").GetComponent<Inventory>();
        primaryInventoryUi = GameObject.FindWithTag("primaryInventoryUi").GetComponent<InventoryUi>();
        secondaryInventoryUI = GameObject.FindWithTag("secondaryInventoryUi").GetComponent<InventoryUi>();
        inventory = transform.GetComponent<Inventory>();
    }
    public override string hint()
    {
        return " Open " + inventory.name + " inventory";
    }

    public override void Interact()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            primaryInventoryUi.OnOpenInventory(playerInventory);
            secondaryInventoryUI.OnOpenInventory(inventory);
        }
        else
        {
            primaryInventoryUi.OnCloseInventory();
            secondaryInventoryUI.OnCloseInventory();
        }
    }
}
