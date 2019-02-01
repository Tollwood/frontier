using TMPro;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public Camera cam;
    public float distance = 10f;
    public TextMeshProUGUI hint;

    private Interactable interactable;
    private void Start()
    {
        hint = GameObject.FindGameObjectWithTag("hint").GetComponent<TextMeshProUGUI>();
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
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, distance))
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
