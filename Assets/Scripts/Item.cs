﻿using UnityEngine;

/* The base item class. All items should derive from this. */

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {

    public new string name = "New Item";    // Name of the item
    public Sprite icon = null;              // Item icon

}