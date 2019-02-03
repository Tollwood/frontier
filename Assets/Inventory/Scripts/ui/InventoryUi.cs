using TMPro;
using UnityEngine;

public class InventoryUi : MonoBehaviour
{
    public TextMeshProUGUI title;
    public GameObject inventoryUI; 
    public Transform itemsParent;
    public InventorySlot itemSlotsPrefab;
    private Inventory inventory;

    private void Start()
    {
        InventoryManager.Instance.OnCloseInventoryCallback += OnCloseInventory;
    }

    public void OnOpenInventory(Inventory inventory)
    {
        inventoryUI.SetActive(true);
        title.text = inventory.name;
        this.inventory = inventory;
        UpdateUI();
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
        if(inventory != null)
        {
            for (int i = 0; i < inventory.space; i++)
            {
                InventorySlot slot = Instantiate(itemSlotsPrefab, itemsParent);
                slot.init(inventory, i);
            }
        }
    }
}
