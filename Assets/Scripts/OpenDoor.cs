using System.Collections;
using UnityEngine;

public class OpenDoor : Interactable
{

    public bool isOpen = false;
    public float closedPosition = 0f;
    public float openPosition = 90f;
    public float speed = .5f;

    private bool interactable = true;

    public override string hint()
    {
        return "Open door";
    }

    public override void Interact()
    {
        base.Interact();
        if (interactable){
            isOpen = !isOpen;
            interactable = false;
            StartCoroutine("MoveDoor", isOpen);    
        }
    }

    IEnumerator MoveDoor(bool changeToOpen)
    {
        float time = 0;
        var originalRotation = transform.rotation;
        float from = changeToOpen ? closedPosition : openPosition; 
        float to = changeToOpen ? openPosition : closedPosition;

        while (time < 1f)
        {
            time += Time.deltaTime;
            float yRotation = Mathf.Lerp(from, to, time);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
            yield return null;
        }
        interactable = true;
    }
}