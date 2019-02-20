using System;
using UnityEngine;

public abstract class AbstractPrimaryActionManager : MonoBehaviour
{
    protected Equipment equipedItem;

    private int currentIndex = 0;

    public virtual void Awake()
    {
        EventManager.StartListening(Events.OnItemEquip, ActivatePrimaryAction);
        EventManager.StartListening(Events.OnItemUnEquip, DeactivatePrimaryAction);
    }

    protected void OnPlayerChanged(object currentIndex)
    {
        this.currentIndex = (int)currentIndex; 
    }

    protected abstract void ExecutePrimaryAction();
    protected abstract Capability GetCapability();


    protected virtual void ActivatePrimaryAction(object obj)
    {
        if (((Equipment)obj).capabiltiy == GetCapability())
        {
            EventManager.StartListening(Events.OnExecutePrimaryAction, ExecutePrimaryAction);
            equipedItem = (Equipment)obj;
        }
    }

    protected virtual void DeactivatePrimaryAction(object obj)
    {
        if (((Equipment)obj).capabiltiy == GetCapability())
        {
            EventManager.StopListening(Events.OnExecutePrimaryAction, ExecutePrimaryAction);
            equipedItem = null;
        }
    }

    protected Transform currentPlayer()
    {
        return PlayerManager.Instance.CurrentPlayer().transform;
    }
}
