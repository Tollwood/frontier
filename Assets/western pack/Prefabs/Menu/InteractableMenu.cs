using UnityEngine;
using UnityEngine.UI;

public class InteractableMenu : Interactable
{

    private MenuAction[]  GetOptions()
    {
        Interactable[] interactables = GetComponentsInChildren<Interactable>();

        MenuAction[] options = new MenuAction[interactables.Length];
        for(int i = 0; i < interactables.Length; i++)
        {
            MenuAction menuAction = new MenuAction();
            menuAction.hint = interactables[i].hint();
            if (interactables[i] == this)
            {
                menuAction.hint = "close";
            }
            options[i] = menuAction;
            options[i].sprite = interactables[i].icon;
            options[i].interaction =  interactables[i].Interact;
        }

        return options;
    }

    private void OnMouseDown()
    {
        RadialMenuSpawner.ins.SpawnMenu(GetOptions());
        EventManager.TriggerEvent(Events.OnOpenMenu);
    }
}
