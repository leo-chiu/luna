using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipFloatScript : MonoBehaviour
{
    public float originalY;
    public float originalX;
    public float bobbing_speed_horizontal;
    public float bobbing_speed_vertical;

    public void OnEnable()
    {
        originalY = transform.position.y;
        originalX = transform.position.x;
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, originalY + ((float)Mathf.Sin(Time.time) * bobbing_speed_horizontal), 0);
        transform.position = new Vector3(originalX + ((float)Mathf.Cos(Time.time) * bobbing_speed_vertical), transform.position.y, 0);
    }
}
