using UnityEngine;

public class InteractableReleaseCart : Interactable
{
    private void Start()
    {
        name = "release cart";
        icon = IconManager.Instance.GetIcon(IconManager.CART_ICON);
    }

    public override void Interact()
    {
        ConfigurableJoint joint = this.gameObject.GetComponentInParent<Rigidbody>().gameObject.GetComponent<ConfigurableJoint>();
        Destroy(joint);
        this.gameObject.AddComponent<InteractablePullCart>();
        Destroy(this);
    }
}
