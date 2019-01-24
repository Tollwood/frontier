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
        int? freeIndex = GetFreeIndex(item);
        if (freeIndex == null)
        {
            Debug.Log("Not enough room.");
            return false;
        }
        return Add(item, freeIndex.Value);
    }
    public bool Add(Item item, int index) {
        Item inventoryItem = items[index];
        if (inventoryItem != null && inventoryItem.name == item.name)
        {
            inventoryItem.amount += item.amount;
        }
        else if (inventoryItem != null)
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

    private int? GetFreeIndex(Item item)
    {
        int? emptyIndex = null;
        for (int i = 0; i < items.Length; i++) {
            Item inventoryItem = items[i];
            if (emptyIndex == null && inventoryItem == null)
            {
                emptyIndex = i;
            }
            if (inventoryItem != null && inventoryItem.name == item.name)
            {
                return i;
            }
        }
        return emptyIndex;
    }

    // Remove an item
    public void Remove(Item item)
    {

        for(int i = 0; i<items.Length; i++)
        {
            if (item.Equals(items[i]))
            {
                items[i] = null;
                break;
            }
        }
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
}