public class RemovePole : Interactable
{

    private void Start()
    {
        icon = IconManager.Instance.GetIcon(IconManager.DELETE_ICON);
    }
    public override string hint()
    {
        return "Remove Pole";
    }

    public override void Interact()
    {
        Pole p = this.transform.parent.GetComponent<Pole>();
        p.property.removeMarker(this.transform.parent.gameObject);
        Destroy(this.transform.parent.gameObject);
    }
}
