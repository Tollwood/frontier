using UnityEngine;

public class Inventory
{
    public string name = "Inventory";
    public string Id { get; private set; }
    public int space;  // Amount of item spaces
    public Item[] items;

    public Inventory(string id, int space)
    {
        this.Id = id;
        this.space = space;
        items = new Item[space];
    }

    public bool Add(Item item)
    {
        int? freeIndex = GetIndex(item);
        if (freeIndex == null)
        {
            Debug.Log("Not enough room.");
            return false;
        }
        return Add(item, freeIndex.Value);
    }

    public bool Add(Item item, int index) {
        Item inventoryItem = items[index];
        if (item.SameIcon(inventoryItem))
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

    private int? GetIndex(Item item)
    {
        int? emptyIndex = null;
        for (int index = 0; index < items.Length; index++) {
            Item inventoryItem = items[index];
            if (emptyIndex == null && inventoryItem == null)
            {
                emptyIndex = index;
            }
            if( item.SameIcon(inventoryItem))
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
        Item tmpItem = items[toIndex];
        items[toIndex] = items[fromIndex];
        items[fromIndex] = tmpItem;
        return true;
    }

    private bool FreeSlot(int index)
    {
        return items[index] == null;
    }
}