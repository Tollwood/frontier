using UnityEngine;
using UnityEngine.UI;

public class MouseManager: MonoBehaviour
{

    public Crosshair crosshair;
    public new vThirdPersonCamera camera;

    private void Start()
    {
        crosshair = FindObjectOfType<Crosshair>();
        EventManager.StartListening(Events.OnOpenInventory, OnShowMouse);
        EventManager.StartListening(Events.OnCloseInventory, OnHideMouse);
        EventManager.StartListening(Events.StartPlanningMode, OnShowMouse);
        EventManager.StartListening(Events.StopPlacementMode, OnHideMouse);
        EventManager.StartListening(Events.OnOpenMenu, OnShowMouse);
        EventManager.StartListening(Events.OnCloseMenu, OnHideMouse);
    }

    void OnShowMouse() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        crosshair.gameObject.SetActive(false);
    }

    void OnHideMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
        crosshair.gameObject.SetActive(true);
    }
}