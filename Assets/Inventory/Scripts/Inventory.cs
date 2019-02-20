using UnityEngine;

[System.Serializable]
public class Inventory
{
    public string name = "Inventory";
    public string Id { get; private set; }
    public int space;  // Amount of item spaces
    public StackItem[] items;

    public Inventory(string id, int space, string name)
    {
        this.Id = id;
        this.space = space;
        this.name = name;
        this.items = new StackItem[space];
    }

    public bool Add(StackItem item)
    {
        int? freeIndex = GetIndex(item);
        if (freeIndex == null)
        {
            Debug.Log("Not enough room.");
            return false;
        }
        return Add(item, freeIndex.Value);
    }

    public bool Add(StackItem item, int index) {
        StackItem inventoryItem = items[index];
        if (item.SameItem(inventoryItem))
        {
            inventoryItem.amount += item.amount;
        }
        else if (!FreeSlot(index))
        {
            Debug.Log("Slot already taken");
            return false;
        }
        else
        {
            items[index] = item;
        }
        return true;
    }

    private int? GetIndex(StackItem item)
    {
        int? emptyIndex = null;
        for (int index = 0; index < items.Length; index++) {
            StackItem inventoryItem = items[index];
            if (emptyIndex == null && inventoryItem == null)
            {
                emptyIndex = index;
            }
            if( item.SameItem(inventoryItem))
                return index;
        }
        return emptyIndex;
    }

    // Remove an item
    public bool Remove(int index)
    {
        if (index >= items.Length)
            return false;

        items[index] = null;
        return true;
    }

    public bool SwapItem(int fromIndex, int toIndex)
    {
        StackItem tmpItem = items[toIndex];
        items[toIndex] = items[fromIndex];
        items[fromIndex] = tmpItem;
        return true;
    }

    private bool FreeSlot(int index)
    {
        return items[index] == null;
    }
}