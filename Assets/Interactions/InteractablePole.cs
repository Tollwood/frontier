public class InteractablePole : Interactable
{

    public override string hint()
    {
        return "Place house";
    }

    public override void Interact()
    {
        EventManager.TriggerEvent(Events.StartPlanningMode);
    }
}
