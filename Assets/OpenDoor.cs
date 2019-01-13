using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{

    private bool isOpen = false;
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isOpen)
            {
                transform.Rotate(0, -90, 0);
            }
            else
            {
                transform.Rotate(0, 90, 0);
            }
            isOpen = !isOpen;
        }
    }
}
