using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject planningMode;
    public GameObject planningModeUi;

    public GameObject playerMode;
    public GameObject playerModeUi;

    public KeyCode planningModeKey = KeyCode.M;

    private bool planningActive;

    private void Awake()
    {
        planningActive = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(planningModeKey))
        {
            planningActive = !planningActive;
            playerMode.SetActive(!planningActive);
            playerModeUi.SetActive(!planningActive);
            planningMode.SetActive(planningActive);
            planningModeUi.SetActive(planningActive);

            if (!planningActive)
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
}
