using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager instance { get; set;}

    public InventorySlot curr_slot;

    public Transform tooltip;

    public TextMeshProUGUI text;

    public Canvas tool_tip_canvas;

    public float description_font_size;

    public float item_name_font_size;

    public float count_font_size;

    public float stat_font_size;

    public void Awake()
    {
        if (instance == null) instance = this;
    }

    public void Start()
    {
        InventoryUI.instance.onInventoryUICallback += UpdateTooltip;
    }

    public void OnDisable()
    {
        InventoryUI.instance.onInventoryUICallback -= UpdateTooltip;
    }

    public void UpdateTooltip()
    {
        if (curr_slot == null) return;
        if (curr_slot.item == null || curr_slot.item.item == null) return;

        string name = "";
        int curr_count = 0;
        int max_count = 0;
        string description = "";

        curr_slot.getSlotData(ref name, ref curr_count, ref max_count, ref description);
        RewriteItemTooltip(name, curr_count, max_count, description);
    }

    public void RewriteItemTooltip(string name, int curr_count, int max_count, string description)
    {
        string name_rich_text = "<font=\"ARCADECLASSIC SDF\">";
        string body_rich_text = "";
        string name_bold_text = "<b>";
        string name_unbold_text = "</b>";

        string item_size_text = "<size=" + item_name_font_size + ">";
        string unsize_text = "</size>";
        string description_size_text = "<size=" + description_font_size + ">";
        string count_size_text = "<size=" + count_font_size + ">";
        string stat_size_text = "<size=" + stat_font_size + ">";

        text.text = item_size_text + name_bold_text + name_rich_text + name + name_unbold_text + unsize_text + "\n"
            + count_size_text + body_rich_text + "x " + curr_count + "/" + max_count + unsize_text + "\n"
            + description_size_text + description + unsize_text + "\n";

        if(curr_slot.item.item is Equipment)
        {
            text.text += stat_size_text;
            if ((curr_slot.item.item as Equipment).attack_modifier > 0)
            {
                text.text += "Attack: +" + (curr_slot.item.item as Equipment).attack_modifier;
                text.text += "\n";
            }
            if ((curr_slot.item.item as Equipment).armor_modifier > 0)
            {
                text.text += "Armor: +" + (curr_slot.item.item as Equipment).armor_modifier;
                text.text += "\n";
            }
            if ((curr_slot.item.item as Equipment).speed_modifier > 0)
            {
                text.text += "Speed: +" + (curr_slot.item.item as Equipment).speed_modifier;
                text.text += "\n";
            }
            if ((curr_slot.item.item as Equipment).mining_modifier > 0)
            {
                text.text += "Mining: +" + (curr_slot.item.item as Equipment).mining_modifier;
                text.text += "\n";
            }
            if ((curr_slot.item.item as Equipment).forestry_modifier > 0)
            {
                text.text += "Forestry: +" + (curr_slot.item.item as Equipment).forestry_modifier;
                text.text += "\n";
            }

            text.text += unsize_text;
        }

        if(curr_slot.item.item is Projectile)
        {
            text.text += stat_size_text;
            if ((curr_slot.item.item as Projectile).attack_boost > 0)
            {
                text.text += "Damage: +" + (curr_slot.item.item as Projectile).attack_boost;
                text.text += "\n";
            }
            text.text += unsize_text;
        }
    }

    public void TurnOffTooltip()
    {
        tooltip.gameObject.SetActive(false);
    }

    public void TurnOnTooltip()
    {
        tooltip.gameObject.SetActive(true);
    }
}
