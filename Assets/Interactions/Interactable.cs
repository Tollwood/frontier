﻿using UnityEngine;

public abstract class Interactable: MonoBehaviour {

    public Sprite icon { protected set; get; }
    public bool showHint = false;
    public new string name;

    public virtual void  Interact(){
        Debug.Log("Interacting with:" + name);
    }

    public virtual string hint()
    {
        return name;
    }
}
