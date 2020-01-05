using UnityEngine;

public class Pole : InteractableMenu
{
    public Property property;
    internal GameObject mapPole;

    internal void Remove()
    {
        property.removeMarker(this);
        Destroy(this.gameObject);
    }
}
