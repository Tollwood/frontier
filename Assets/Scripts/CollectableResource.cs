using UnityEngine;

public class CollectableResource : Interactable
{
    public Item item;
    public int totalAmount = 6;
    public int minAmount = 2;

    public override string hint()
    {
        return "Collect " + item.name;
    }

    public override void Interact()
    {
        Inventory playerInventory = GameObject.FindWithTag("playerInventory").GetComponent<Inventory>();
        Item collectedItems = item.Clone();
        collectedItems.amount = minAmount;
        bool added = playerInventory.Add(collectedItems);
        if (added)
        {
            totalAmount -= minAmount;
        }
        if (totalAmount <= 0)
        {
            Destroy(transform.gameObject);
        }
    }
}
