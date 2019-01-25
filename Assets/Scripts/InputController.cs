using UnityEngine;

public class InputController : MonoBehaviour
{

    public KeyCode inventoryKeyCode = KeyCode.I;
    public InventoryUi mainInventoryUi;

    bool isOpen = false;

    Inventory primaryInventory;

    private void Start()
    {
        primaryInventory = GameObject.FindWithTag("playerInventory").GetComponent<Inventory>();
    }

    void Update()
    {
        if (Input.GetKeyDown(inventoryKeyCode))
        {
            isOpen = !isOpen;
            if (isOpen)
            {
                mainInventoryUi.OnOpenInventory(primaryInventory);
                //freeze Camera
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                mainInventoryUi.OnCloseInventory();
                //rotate Camera
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }


        }
    }
}
