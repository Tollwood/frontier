using UnityEngine;

public class Inventory : MonoBehaviour
{
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    public new string name = "Inventory";

    public int space = 10;  // Amount of item spaces

    // Our current list of items in the inventory
    public Item[] items;

    private void Awake()
    {
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
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

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
    public void Remove(int index)
    {
        items[index] = null;
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    internal void SwapItem(int fromIndex, int toIndex)
    {
        Item tmpItem = items[toIndex];
        items[toIndex] = items[fromIndex];
        items[fromIndex] = tmpItem;
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    private bool FreeSlot(int index)
    {
        return items[index] == null;
    }
}