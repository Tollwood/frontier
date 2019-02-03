using System;
using UnityEngine;

public class InputManager: MonoBehaviour
{

    public KeyCode inventoryKeyCode = KeyCode.I;
    public KeyCode interactionKeyCode = KeyCode.E;
    public KeyCode planningModeKey = KeyCode.M;
    public KeyCode placePoleKey = KeyCode.Y;
    public KeyCode switchPlayer = KeyCode.Tab;

    private PlanningManager planningManager;
    private InteractionController interactionController;
    private PropertyMarker propertyMarker;
    private InventoryManager inventoryManager;
    private PlayerManager playerManager;

    private void Start()
    {
        planningManager = FindObjectOfType<PlanningManager>();
        interactionController = FindObjectOfType<InteractionController>();
        propertyMarker = FindObjectOfType<PropertyMarker>();
        inventoryManager = FindObjectOfType<InventoryManager>();
        playerManager = FindObjectOfType<PlayerManager>();

    }

    void Update()
    {
        if (Input.GetKeyDown(inventoryKeyCode))
            inventoryManager.ToggleInventory();
        if (Input.GetKeyDown(placePoleKey))
            propertyMarker.AddPole(GameObject.FindWithTag("Player").transform.position);

        if (Input.GetKeyDown(interactionKeyCode))
            interactionController.TriggerInteraction();

        if (Input.GetKeyDown(planningModeKey))
            planningManager.TogglePlanningMode();
        if (Input.GetKeyDown(switchPlayer))
            playerManager.SwitchPlayer();
    }
}