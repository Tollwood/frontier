using TMPro;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public Camera cam;
    public float distance = 10f;
    private TextMeshProUGUI hint;
    private Interactable interactable;

    private bool interacting = true;

    private void Start()
    {
        InventoryManager.Instance.OnOpenInventoryCallback += OnOpenInventory;
        InventoryManager.Instance.OnCloseInventoryCallback += OnCloseInventory;
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
        if (!interacting)
        {
            return null;
        }
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

    public void OnOpenInventory()
    {
        interacting = false;
    }

    public void OnCloseInventory()
    {
        interacting = true;
    }
}
