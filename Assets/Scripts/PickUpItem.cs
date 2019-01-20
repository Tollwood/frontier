﻿using UnityEngine;

public class PickUpItem : Interactable
{
    public Item item;

    public override string hint()
    {
        return "Take " + item.name;
    }

    public override void Interact()
    {
        Inventory playerInventory =  GameObject.FindWithTag("playerInventory").GetComponent<Inventory>();
        playerInventory.Add(item);
        Destroy(transform.gameObject);
    }

}
