using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public GameObject close_inventory;
    public GameObject open_inventory;

    public void OnEnable()
    {
        close_inventory.gameObject.SetActive(true);
        open_inventory.gameObject.SetActive(false);
    }
}
