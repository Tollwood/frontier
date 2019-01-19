using UnityEngine;
using System.Collections;

public class PickUpItem : Interactable
{
    void Interactable(){
        Debug.Log("Picking up " + transform.name);
    }
}
