using UnityEngine;

public class InventoryUi : MonoBehaviour
{
    public TMPro.TextMeshProUGUI title;
    public GameObject inventoryUI; 
    public Transform itemsParent; 

    public void onOpenInventory(Inventory inventory)
    {
        inventoryUI.gameObject.SetActive(true);
        title.text = inventory.name;
        UpdateUI(inventory);
    }

    public void OnCloseInventory()
    {
        inventoryUI.gameObject.SetActive(false);
    }

    public void UpdateUI(Inventory inventory)
    {
        InventorySlot[] slots = GetComponentsInChildren<InventorySlot>();

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
