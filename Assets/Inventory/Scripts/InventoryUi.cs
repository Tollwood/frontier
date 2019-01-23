using UnityEngine;

public class InventoryUi : MonoBehaviour
{
    public TMPro.TextMeshProUGUI title;
    public GameObject inventoryUI; 
    public Transform itemsParent;
    public InventorySlot itemSlotsPrefab;
    private Inventory inventory;

    public void onOpenInventory(Inventory inventory)
    {
        inventoryUI.gameObject.SetActive(true);
        title.text = inventory.name;
        this.inventory = inventory;
        UpdateUI();
        inventory.onItemChangedCallback -= UpdateUI;
        inventory.onItemChangedCallback +=  UpdateUI;
    }

    public void OnCloseInventory()
    {
        inventoryUI.gameObject.SetActive(false);
    }

    public void UpdateUI()
    {
        foreach(Transform child in itemsParent){
            Destroy(child.gameObject);
        }

        for( int i = 0; i < inventory.space; i++){
            InventorySlot slot = Instantiate(itemSlotsPrefab, itemsParent);
            slot.name = inventory.name + "-slot-" + i;
                slot.inventory = inventory;
                slot.index = i;
                
                slot.AddItem(inventory.items[i]);
        }
    }
}
