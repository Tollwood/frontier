using UnityEngine;

public class InventoryUi : MonoBehaviour
{
    public TMPro.TextMeshProUGUI title;
    public GameObject inventoryUI; 
    public Transform itemsParent;

    public GameObject inventorySlotPreFab;

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
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < inventory.items.Count; i++)
        {

            InventorySlot slot = Instantiate(inventorySlotPreFab, transform).GetComponent<InventorySlot>();
            slot.AddItem(inventory.items[i]);
        }
    }
}
