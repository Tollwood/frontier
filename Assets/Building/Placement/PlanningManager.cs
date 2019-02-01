using UnityEngine;

public class PlanningManager : MonoBehaviour
{
    public GameObject playerMode;
    public GameObject playerModeUi;

    private bool planningActive;

    private void Awake()
    {
        planningActive = false;
    }

    public void TogglePlanningMode()
    {
        planningActive = !planningActive;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(planningActive);

        }
        playerMode.SetActive(!planningActive);
        playerModeUi.SetActive(!planningActive);


        if (planningActive)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

}
