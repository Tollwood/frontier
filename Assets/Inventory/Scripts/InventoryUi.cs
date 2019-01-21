using UnityEngine;

public class InventoryUi : MonoBehaviour
{
    public TMPro.TextMeshProUGUI title;
    public GameObject inventoryUI; 
    public Transform itemsParent;
    public InventorySlot itemSlotsPrefab;

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

    foreach(Transform child in itemsParent){
            GameObject.Destroy(child.gameObject);
        }

    InventorySlot[] slots = new InventorySlot[inventory.space];
    for( int i = 0; i < inventory.space; i++){
       slots[i] = Instantiate(itemSlotsPrefab, itemsParent);
   }

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
        }
    }
}
