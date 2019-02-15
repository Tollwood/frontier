using UnityEngine;

public abstract class AbstractPrimaryActionManager : MonoBehaviour
{
    protected Transform player;

    private void Awake()
    {
        EventManager.StartListening(Events.OnPlayerChanged, (object obj) => player = ((GameObject)obj).transform);
        EventManager.StartListening(Events.OnItemEquip, ActivatePrimaryAction);
        EventManager.StartListening(Events.OnItemUnEquip, DeactivatePrimaryAction);
    }

    protected void OnPlayerChanged(System.Object player)
    {
        this.player = ((GameObject)player).transform;
        // update canPlant according to currentPlayer
    }

    protected abstract void ExecutePrimaryAction();
    protected abstract Capability GetCapability();


    protected virtual void ActivatePrimaryAction(object obj)
    {
        if (((Equipment)obj).capabiltiy == GetCapability())
        {
            EventManager.StartListening(Events.OnExecutePrimaryAction, ExecutePrimaryAction);
        }
    }

    protected virtual void DeactivatePrimaryAction(object obj)
    {
        if (((Equipment)obj).capabiltiy == GetCapability())
        {
            EventManager.StopListening(Events.OnExecutePrimaryAction, ExecutePrimaryAction);
        }
    }


}
