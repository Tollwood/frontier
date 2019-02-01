using UnityEngine;

public class CollectableResource : Interactable
{
    public Item item;
    public int totalAmount = 6;
    public int minAmount = 2;
    private PlayerManager playerManager;
    public override string hint()
    {
        return "Collect " + item.name;
    }

    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }

    public override void Interact()
    {
        Item collectedItems = item.Clone();
        collectedItems.amount = minAmount;
        bool added = playerManager.GetCurrentInventory().Add(collectedItems);
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
