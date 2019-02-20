public class PickUpItem : Interactable
{
    public StackItem item;

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
        bool pickedUp = InventoryManager.Instance.AddToCurrentInventtory(item);
        if (pickedUp)
        {
            Destroy(transform.gameObject);
        }

    }

}
