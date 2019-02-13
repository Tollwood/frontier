using System;
using UnityEngine;

public class InputManager: Singleton<InputManager>
{
    public KeyCode inventoryKeyCode = KeyCode.I;
    public KeyCode interactionKeyCode = KeyCode.E;
    public KeyCode planningModeKey = KeyCode.M;
    public KeyCode switchPlayer = KeyCode.Tab;
    public KeyCode increaseHeightKey = KeyCode.Plus;
    public KeyCode decreaseHeightKey = KeyCode.Minus;

    private PlanningManager planningManager;
    private InteractionController interactionController;
    private PropertyMarker propertyMarker;

    bool primaryActionEnabled = true;

    private void Start()
    {
        planningManager = FindObjectOfType<PlanningManager>();
        interactionController = FindObjectOfType<InteractionController>();
        propertyMarker = FindObjectOfType<PropertyMarker>();
        EventManager.StartListening(Events.OnOpenInventory, () => primaryActionEnabled = false);
        EventManager.StartListening(Events.StartPlanningMode, () => primaryActionEnabled = false);
        EventManager.StartListening(Events.OnCloseInventory, () => primaryActionEnabled = true);
        EventManager.StartListening(Events.StopPlanningMode, () => primaryActionEnabled = true);
    }

    private void DisablePrimaryAction()
    {
        throw new NotImplementedException();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && primaryActionEnabled)
            EventManager.TriggerEvent(Events.OnExecutePrimaryAction); ;

        if (Input.GetKeyDown(inventoryKeyCode))
            InventoryManager.Instance.ToggleInventory();

        if (Input.GetKeyDown(interactionKeyCode))
            interactionController.TriggerInteraction();

        if (Input.GetKeyDown(planningModeKey))
            EventManager.TriggerEvent(Events.StopPlanningMode);
        if (Input.GetKeyDown(switchPlayer))
            PlayerManager.Instance.SwitchPlayer();
        if (Input.GetKeyDown(increaseHeightKey))
            EventManager.TriggerEvent(Events.OnIncrease); 
        if (Input.GetKeyDown(decreaseHeightKey))
            EventManager.TriggerEvent(Events.OnDecrease);



    }
}