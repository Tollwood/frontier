public class RemovePole : Interactable
{

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
