﻿
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
        StackItem collectedItems = new StackItem(item.name, minAmount);
        bool added = InventoryManager.Instance.AddToCurrentInventtory(collectedItems);
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
