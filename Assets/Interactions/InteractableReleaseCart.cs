using UnityEngine;

public class InteractableReleaseCart : Interactable
{
    private void Start()
    {
        name = "release cart";
    }

    public override void Interact()
    {
        ConfigurableJoint joint = this.gameObject.GetComponentInParent<Rigidbody>().gameObject.GetComponent<ConfigurableJoint>();
        Destroy(joint);
        InteractablePullCart pullCart = this.gameObject.AddComponent<InteractablePullCart>();
        pullCart.icon = this.icon;
        Destroy(this);
    }
}
