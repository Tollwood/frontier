using UnityEngine;

public class InteractionController : MonoBehaviour
{

    float distance = 0.9f;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        Vector3 direction = transform.TransformDirection(Vector3.forward * distance);
        Ray ray = new Ray(transform.position, direction);
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, distance) && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Did Hit: "+ hit.collider.name);
            Interactable interactable = hit.collider.transform.GetComponent<Interactable>();
            if(interactable != null){
                interactable.Interact();
                Debug.DrawRay(ray.origin,ray.direction, Color.green);
            }
            else {
                Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
            }
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.white);
            Debug.Log("Did not Hit");
        }
    }
}
