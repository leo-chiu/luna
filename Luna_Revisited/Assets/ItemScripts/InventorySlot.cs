using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;

    public ItemStack item;

    public Sprite slot_selected_sprite;

    public Sprite slot_not_selected_sprite;

    public GameObject slot; 

    private TooltipManager tooltipManager;

    private bool empty = true;

    public void Start()
    {
        tooltipManager = TooltipManager.instance;
    }

    public void AddItem(ItemStack item)
    {
        if (item == null || item.item == null) return;

        this.item = item;
        icon.sprite = item.item.sprite;
        icon.enabled = true;
        empty = false;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        empty = true;
    }

    public void Use()
    {
        if(item != null && item.item != null)
            item.item.Use();
    }

    public void RemoveAndClear()
    {
        if (item == null || item.item == null) return;
        int slot_index;
        if (gameObject.name.Split('(', ')').Length <= 1)
        {
            slot_index = 0;
        }
        else {
            int.TryParse(gameObject.name.Split('(', ')')[1], out slot_index);
        }
        Inventory.instance.RemoveAtSlot(slot_index);
        ClearSlot();
        InventoryUI.instance.UpdateUI();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!empty)
        {
            tooltipManager.curr_slot = this;
            slot.GetComponent<Image>().sprite = slot_selected_sprite;
            showToolTip();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipManager.curr_slot = null;
        slot.GetComponent<Image>().sprite = slot_not_selected_sprite;
        tooltipManager.tooltip.gameObject.SetActive(false);
    }

    public void getSlotData(ref string name, ref int curr_count, ref int max_count, ref string description)
    {
        if (item == null || item.item == null) return;
        name = item.item.name;
        curr_count = item.count;
        max_count = item.item.stack_capacity;
        description = item.item.description;
    }

    public void showToolTip()
    {
        Vector2 set_pivots = new Vector2();
        float yRatio = Mathf.InverseLerp(0, Screen.height, transform.position.y - transform.gameObject.GetComponent<RectTransform>().rect.height/2);
        if (yRatio >= .5)
        {
           set_pivots.y = 1;
        }
        else
        {
           set_pivots.y = 0;
        }   
        float xRatio = Mathf.InverseLerp(0, Screen.width, transform.position.x - transform.gameObject.GetComponent<RectTransform>().rect.width/2);
        if (xRatio >= .5)
        {
            set_pivots.x = 1;
        }
        else
        {
            set_pivots.x = 0;
        }

        tooltipManager.tooltip.gameObject.GetComponent<RectTransform>().pivot = set_pivots;

        tooltipManager.tooltip.GetComponent<RectTransform>().position = transform.position;

        tooltipManager.RewriteItemTooltip(item.item.name, item.count, item.item.stack_capacity, item.item.description);

        tooltipManager.TurnOnTooltip();
    }
}
