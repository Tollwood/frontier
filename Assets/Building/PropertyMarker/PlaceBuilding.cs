public class PlaceBuilding : Interactable
{

    public override string hint()
    {
        return "Place building";
    }

    public override void Interact()
    {
        EventManager.TriggerEvent(Events.StartPlanningMode);
    }
}
