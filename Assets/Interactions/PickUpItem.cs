public class PickUpItem : Interactable
{
    public Item item;

    PlayerManager playerManager;

    public override string hint()
    {
        return "Take " + item.name;
    }

    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }

    public override void Interact()
    {
        Inventory playerInventory = playerManager.GetCurrentInventory();
        bool pickedUp = playerInventory.Add(item);
        if (pickedUp)
        {
            Destroy(transform.gameObject);
        }

    }

}
