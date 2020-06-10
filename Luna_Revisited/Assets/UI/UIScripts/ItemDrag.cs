using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private float offsetX;
    private float offsetY;

    private Vector3 originalPosition;

    private RectTransform inventory_panel;

    private Transform originalParent;

    public CanvasGroup canvas_group;

    public void OnBeginDrag(PointerEventData eventData)
    {
        DragDropManager.instance.dragged_UI = transform.parent;
        DragDropManager.instance.dragged_UI_index = transform.parent.GetSiblingIndex();
        originalPosition = transform.position;
        inventory_panel = GameObject.Find("Scroll View").transform as RectTransform;
        offsetX = transform.position.x - Input.mousePosition.x;
        offsetY = transform.position.y - Input.mousePosition.y;
        originalParent = transform.parent;
        gameObject.transform.SetParent(GameObject.Find("OutsideViewport").transform);
        canvas_group.blocksRaycasts = false;
        DragDropManager.instance.hovered_UI = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = new Vector3(offsetX + Input.mousePosition.x, offsetY + Input.mousePosition.y);
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bool to_drop_item = false;
        if (!RectTransformUtility.RectangleContainsScreenPoint(inventory_panel, Input.mousePosition))
        {
            Debug.Log("Dropped outside of inventory");
            to_drop_item = true;
        }
        else
        {
            Debug.Log("Dropped within inventory");
        }
        if (DragDropManager.instance.hovered_UI != null)
        {
            Debug.Log("Dropped on " + DragDropManager.instance.hovered_UI.gameObject.transform.parent.GetSiblingIndex());
            if (Inventory.instance.items.Count - 1 >= DragDropManager.instance.hovered_UI.gameObject.transform.parent.GetSiblingIndex())
            {
                Debug.Log("Swap slots");
                int a_index = DragDropManager.instance.hovered_UI_index;
                Debug.Log("a: " + a_index);
                int b_index = DragDropManager.instance.dragged_UI_index;
                Debug.Log("b: " + b_index);
                Inventory.instance.SwapSlots(a_index, b_index);
            }
        }
        gameObject.transform.SetParent(originalParent);
        gameObject.transform.SetAsFirstSibling();
        transform.position = originalPosition;
        if (to_drop_item)
        {
            InventorySlot slot = transform.parent.GetComponent<InventorySlot>();
            slot.RemoveAndClear();
        }

        DragDropManager.instance.dragged_UI = null;
        canvas_group.blocksRaycasts = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DragDropManager.instance.hovered_UI = transform.parent;
        DragDropManager.instance.hovered_UI_index = transform.parent.GetSiblingIndex();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DragDropManager.instance.hovered_UI = null;
    }

}
