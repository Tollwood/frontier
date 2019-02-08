using UnityEngine;

public class PlanningManager : MonoBehaviour
{
    public GameObject playerMode;
    public GameObject playerModeUi;

    private void Start()
    {
        EventManager.StartListening(Events.StartPlanningMode, () => TogglePlanning(true));
        EventManager.StartListening(Events.StopPlanningMode, () => TogglePlanning(false));
    }

    private void TogglePlanning(bool enable)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(enable);

        }
        playerMode.SetActive(!enable);
        playerModeUi.SetActive(!enable);
    }

}
