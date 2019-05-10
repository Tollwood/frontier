using UnityEngine;
using UnityEngine.EventSystems;

public class CloseInventory : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        InventoryManager.Instance.ToggleInventory();
    }
}
