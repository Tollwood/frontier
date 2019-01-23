using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(InventorySlot))]
public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public InventorySlot inventorySlot { get; private set; }

    private Vector3 startPosition;
    private Transform startParent;

    private Transform canvas;

    private Transform iconTransform;
    bool isDragable;
    private void Start()
    {
        canvas = GameObject.FindWithTag("canvas").transform;
        inventorySlot = GetComponent<InventorySlot>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragable = inventorySlot.item != null;
        if (isDragable)
        {
            iconTransform = inventorySlot.icon.transform;
            startPosition = iconTransform.position;
            startParent = iconTransform.parent;
            iconTransform.parent = canvas;
            iconTransform.SetAsLastSibling();
            iconTransform.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragable)
        {
            iconTransform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ResetIcon();
    }

    public void ResetIcon()
    {
        if (isDragable)
        {
            iconTransform.position = startPosition;
            iconTransform.parent = startParent;
            iconTransform.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {

        Debug.Log("DripDrop");
        if (inventorySlot.item != null)
        {
            Debug.Log("cant drop item, slot is already taken");
            return;
        }
        InventorySlot originSlot = eventData.pointerDrag.GetComponent<ItemDragHandler>().inventorySlot;
        originSlot.GetComponent<ItemDragHandler>().ResetIcon();
        if (originSlot.inventory.Equals(inventorySlot.inventory))
        {
            inventorySlot.inventory.SwapItem(originSlot.index, inventorySlot.index);
        }
        else
        {
            inventorySlot.inventory.Add(originSlot.item, inventorySlot.index);
            originSlot.inventory.Remove(originSlot.item);
        }
    }
}