using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOscillator : MonoBehaviour
{
    private Vector2 original_position;
    public float oscillation_magnitude;

    public void Start()
    {
        original_position = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition = new Vector2(original_position.x + Mathf.Sin(Time.time) * oscillation_magnitude, 
            original_position.y + Mathf.Cos(Time.time) * oscillation_magnitude);
    }
}
