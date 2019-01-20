using UnityEngine;

public class InteractionController : MonoBehaviour
{

    public Camera cam;
    public float distance = 10f;
    public KeyCode key = KeyCode.E;
    public TMPro.TextMeshProUGUI hint;

    void Update()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, distance))
        {

            Interactable interactable = hit.collider.transform.GetComponent<Interactable>();
            if(interactable != null){
                hint.text = "Press ("+ key.ToString()+ ") " + interactable.hint();
                if (Input.GetKeyDown(KeyCode.E)){
                    interactable.Interact();
                    Debug.DrawRay(ray.origin, ray.direction, Color.green);
                }
                else
                {
                    Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
                }
            }
            else
            {
                hint.text = "";
            }
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.white);
            hint.text = "";
        }

    }
}
