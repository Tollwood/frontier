using UnityEngine;

/* The base item class. All items should derive from this. */

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
[System.Serializable]
public class Item : ScriptableObject {
    
    public Sprite icon = null;              // Item icon
    public Capability capabiltiy = Capability.None;

    public Item Clone()
    {
        Item clonedItem = new Item
        {
            name = this.name,
            icon = this.icon
        };
        return clonedItem;
    } 

    public bool SameIcon(Item item)
    {
        return item != null && item.name == this.name;
    }
}