using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject Menu;
    public GameObject Inventory;
    private void OnEnable()
    {
        Menu.gameObject.SetActive(true);
        Inventory.gameObject.SetActive(true);
    }
}
