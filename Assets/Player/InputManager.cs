using System;
using UnityEngine;

public class InputManager: Singleton<InputManager>
{
    public delegate void onExecutePrimaryAction();
    public onExecutePrimaryAction executePrimaryAction;
    public delegate void DoIncrease();
    public DoIncrease OnIncrease;
    public delegate void DoDecrease();
    public DoDecrease OnDecrease;


    public KeyCode inventoryKeyCode = KeyCode.I;
    public KeyCode interactionKeyCode = KeyCode.E;
    public KeyCode planningModeKey = KeyCode.M;
    public KeyCode switchPlayer = KeyCode.Tab;
    public KeyCode increaseHeightKey = KeyCode.Plus;
    public KeyCode decreaseHeightKey = KeyCode.Minus;

    private PlanningManager planningManager;
    private InteractionController interactionController;
    private PropertyMarker propertyMarker;

    private void Start()
    {
        planningManager = FindObjectOfType<PlanningManager>();
        interactionController = FindObjectOfType<InteractionController>();
        propertyMarker = FindObjectOfType<PropertyMarker>();

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(executePrimaryAction != null && !InventoryManager.Instance.isOpen)
                executePrimaryAction.Invoke();
        }

        if (Input.GetKeyDown(inventoryKeyCode))
            InventoryManager.Instance.ToggleInventory();

        if (Input.GetKeyDown(interactionKeyCode))
            interactionController.TriggerInteraction();

        if (Input.GetKeyDown(planningModeKey))
            planningManager.TogglePlanningMode();
        if (Input.GetKeyDown(switchPlayer))
            PlayerManager.Instance.SwitchPlayer();
        if (Input.GetKeyDown(increaseHeightKey) && OnIncrease != null)
            OnIncrease.Invoke();
        if (Input.GetKeyDown(decreaseHeightKey) && OnDecrease != null)
            OnDecrease.Invoke();



    }
}