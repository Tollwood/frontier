public class PlaceBuilding : Interactable
{

    private void Start()
    {
        icon = IconManager.Instance.GetIcon(IconManager.BUILDING_ICON);
    }
    public override string hint()
    {
        return "Place building";
    }

    public override void Interact()
    {
        EventManager.TriggerEvent(Events.StartPlanningMode);
    }
}
