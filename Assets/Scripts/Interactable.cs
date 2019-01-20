using UnityEngine;

public abstract class Interactable: MonoBehaviour {

    public virtual void  Interact(){
        Debug.Log("Interacting with:" + transform.name);
    }

    public virtual string hint()
    {
        return " Interact with: " + transform.name;
    }
}
