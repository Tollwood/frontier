using TMPro;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public Camera cam;
    public float distance = 10f;
    public TextMeshProUGUI hint;
    private Interactable interactable;

    private bool interacting = true;

    private void Start()
    {
        EventManager.StartListening(Events.OnOpenInventory, () => interacting = false);
        EventManager.StartListening(Events.OnCloseInventory, () => interacting = true);
    }

    void Update()
    {
        interactable = RayCastInteractable();
        UpdateInteractionHint();
    }

    private void UpdateInteractionHint()
    {
        if (interactable != null)
            hint.text = interactable.hint();
        else
            hint.text = "";
    }

    private Interactable RayCastInteractable()
    {
        if (!interacting)
           return null;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, distance))
        {
            return hit.collider.transform.GetComponent<Interactable>();
        }
        return null;
    }

    public void TriggerInteraction() {
        if (interactable != null)
        {
            interactable.Interact();
        }
    }
}
