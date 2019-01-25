﻿using TMPro;
using UnityEngine;

public class InventoryUi : MonoBehaviour
{
    public TextMeshProUGUI title;
    public GameObject inventoryUI; 
    public Transform itemsParent;
    public InventorySlot itemSlotsPrefab;
    private Inventory inventory;

    public void OnOpenInventory(Inventory inventory)
    {
        inventoryUI.SetActive(true);
        title.text = inventory.name;
        this.inventory = inventory;
        UpdateUI();
        inventory.onItemChangedCallback -= UpdateUI;
        inventory.onItemChangedCallback +=  UpdateUI;
    }

    public void OnCloseInventory()
    {
        inventoryUI.SetActive(false);
    }

    public void UpdateUI()
    {
        foreach(Transform child in itemsParent){
            Destroy(child.gameObject);
        }
        for( int i = 0; i < inventory.space; i++){
            InventorySlot slot = Instantiate(itemSlotsPrefab, itemsParent);
            slot.init(inventory,i);
        }
    }
}