using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public Canvas close_menu;
    public Canvas open_menu;

    public void OnEnable()
    {
        close_menu.gameObject.SetActive(true);
        open_menu.gameObject.SetActive(false);
    }
}
