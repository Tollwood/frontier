public class ShowMesh : Interactable
{

    private void Start()
    {
        icon = IconManager.Instance.GetIcon(IconManager.SHOVEL_ICON);
    }
    public override string hint()
    {
        return "Hide / Show plane";
    }

    public override void Interact()
    {
        Pole p = this.transform.parent.GetComponent<Pole>();
        p.property.ToggleShowMesh();
    }
}
