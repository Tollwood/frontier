using UnityEngine;

public class InventoryUi : MonoBehaviour
{
    public KeyCode inventoryKeyCode = KeyCode.I;
    public GameObject inventoryUI; 
    public Transform itemsParent; 

    void Update()
    {
        if (Input.GetKeyDown(inventoryKeyCode))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            if (inventoryUI.activeSelf)
            {
                //freeze Camera
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                //rotate Camera
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            onOpenInventory(GameObject.FindWithTag("playerInventory").GetComponent<Inventory>());
        }
    }

    public void onOpenInventory(Inventory inventory)
    {
        UpdateUI(inventory);
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
