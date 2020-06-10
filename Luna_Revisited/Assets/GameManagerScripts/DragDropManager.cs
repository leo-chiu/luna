using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropManager : MonoBehaviour
{
    public static DragDropManager instance { get; set; }

    public Transform hovered_UI;
    public int hovered_UI_index;
    public Transform dragged_UI;
    public int dragged_UI_index;

    public void Awake()
    {
        if (instance == null) instance = this;
    }
}
