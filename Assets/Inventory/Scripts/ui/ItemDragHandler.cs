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

    private InventoryManager inventoryManager;

    private void Start()
    {
        canvas = GameObject.FindWithTag("canvas").transform;
        inventorySlot = GetComponent<InventorySlot>();
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragable = !inventorySlot.IsEmpty();
        if (isDragable)
        {
            iconTransform = inventorySlot.icon.transform;
            startPosition = iconTransform.position;
            startParent = iconTransform.parent;
            RenderOnTop();
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
        if (!inventorySlot.IsEmpty())
        {
            Debug.Log("cant drop item, slot is already taken");
            return;
        }
        ItemDragHandler originDragHandler = eventData.pointerDrag.GetComponent<ItemDragHandler>();
        InventorySlot originSlot = originDragHandler.inventorySlot;
        originDragHandler.ResetIcon();
        inventoryManager.UpdateInventory(originSlot.inventory, inventorySlot.inventory, originSlot.index, inventorySlot.index);
    }

    private void RenderOnTop() {
        iconTransform.parent = canvas;
        iconTransform.SetAsLastSibling();
        iconTransform.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

}