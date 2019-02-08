using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshCollider), typeof(Rigidbody))]
public class CollidingCheck : MonoBehaviour
{

    public bool isValid = true;
    private new Rigidbody rigidbody;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        isValid = false;
    }

    private void OnTriggerExit(Collider other)
    {
        isValid = true;
    }
}
