using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance { get; set; }

    public Inventory inventory;

    public Transform slot_parent;

    public InventorySlot[] items;

    public TextMeshProUGUI money; 

    public Button inventory_button;

    public Sprite open_button_texture;
    public Sprite close_button_texture;

    public event Action onInventoryUICallback; 

    public bool inventoryUI_open = false;

    public void Awake()
    {
        if(instance == null) instance = this;
    }

    void Start()
    {
        inventory = Inventory.instance;
        Inventory.instance.onItemCallback += UpdateUI;

        slot_parent.gameObject.SetActive(inventoryUI_open);

        items = slot_parent.GetComponentsInChildren<InventorySlot>();
    }

    private void OnDisable()
    {
        Inventory.instance.onItemCallback -= UpdateUI;
    }

    public void OpenInventoryUI()
    {
        inventoryUI_open = true;
        UpdateUI();
        inventory_button.GetComponent<Image>().sprite = close_button_texture;
        slot_parent.gameObject.SetActive(true);
    }

    public void CloseInventoryUI()
    {
        inventoryUI_open = false;
        inventory_button.GetComponent<Image>().sprite = open_button_texture;
        slot_parent.gameObject.SetActive(false);
        TooltipManager.instance.TurnOffTooltip();
    }


    public void UpdateUI()
    {
        for(int i = 0; i < items.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                items[i].AddItem(inventory.items[i]);
            }
            else
            {
                items[i].ClearSlot();
            }
        }

        money.text = String.Format("{0:#,###0}", Inventory.instance.money);
        
        if (onInventoryUICallback != null)
            onInventoryUICallback.Invoke();

    }

    public void ToggleInventory()
    {
        if (inventoryUI_open)
            CloseInventoryUI();
        else
            OpenInventoryUI();
    }
}
