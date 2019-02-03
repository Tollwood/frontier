using UnityEngine;
using UnityEngine.UI;

public class MouseManager: MonoBehaviour
{

    public Image crosshair;
    public new vThirdPersonCamera camera;

    private void Start()
    {
        InventoryManager.Instance.OnOpenInventoryCallback += OnOpenInventory;
        InventoryManager.Instance.OnCloseInventoryCallback += OnCloseInventory;
    }

    void OnOpenInventory() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        crosshair.gameObject.SetActive(false);
        camera.lockCamera = true;
    }

    void OnCloseInventory()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
        crosshair.gameObject.SetActive(true);
        camera.lockCamera = false;
    }
}