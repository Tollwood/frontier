using UnityEngine;

public class InteractablePullCart : Interactable
{

    private void Start()
    {
        name = "pull cart";
        icon = IconManager.Instance.GetIcon(IconManager.CART_ICON);
    }
    public override void Interact()
    {
        ConfigurableJoint joint = this.gameObject.GetComponentInParent<Rigidbody>().gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedBody = PlayerManager.Instance.CurrentPlayer().GetComponent<Rigidbody>();
        joint.anchor = new Vector3(0, 0.57f, 2.15f);
        joint.axis = new Vector3(1, 0, 0);
        joint.autoConfigureConnectedAnchor = false;
        joint.secondaryAxis = new Vector3(0,1,0);
        joint.connectedAnchor = new Vector3(0,.76f,0.06f);
        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;
        this.gameObject.AddComponent<InteractableReleaseCart>();
        Destroy(this);
    }
}
