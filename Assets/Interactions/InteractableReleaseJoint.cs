using UnityEngine;

public class InteractableReleaseJoint : Interactable
{

    public override string hint()
    {
        return "Release " + this.gameObject.name;
    }

    public override void Interact()
    {
        ConfigurableJoint joint = this.gameObject.GetComponentInParent<Rigidbody>().gameObject.GetComponent<ConfigurableJoint>();
        Destroy(joint);
        this.gameObject.AddComponent<InteractableCart>();
        Destroy(this);
    }
}
