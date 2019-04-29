using UnityEngine;
using UnityEngine.UI;

public class MouseManager: MonoBehaviour
{

    public Image crosshair;
    public new vThirdPersonCamera camera;

    private void Start()
    {
        EventManager.StartListening(Events.OnOpenInventory, OnShowMouse);
        EventManager.StartListening(Events.OnCloseInventory, OnHideMouse);
        EventManager.StartListening(Events.StartPlanningMode, OnShowMouse);
        EventManager.StartListening(Events.StopPlacementMode, OnHideMouse);
    }

    void OnShowMouse() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        crosshair.gameObject.SetActive(false);
        camera.lockCamera = true;
    }

    void OnHideMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
        crosshair.gameObject.SetActive(true);
        camera.lockCamera = false;
    }
}