using System;
using System.Collections.Generic;
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

    // Add a new item if enough room
    public bool Add(Item item)
    {
        int? freeIndex = GetFreeIndex();
        if (freeIndex == null)
        {
            Debug.Log("Not enough room.");
            return false;
        }
        return Add(item, freeIndex.Value);
    }
    public bool Add(Item item, int index) {

        if(items[index] != null)
        {
            Debug.Log("Slot already taken");
            return false;
        }
        items[index] = item;
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

        return true;
    }

    private int? GetFreeIndex()
    {
        for(int i = 0; i< items.Length; i++){
            if(items[i] == null)
            {
                return i;
            }
        }
        return null;
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