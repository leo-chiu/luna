using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedScaleManager : MonoBehaviour
{
    public static SpeedScaleManager instance { get; set; }

    public float speed_scale;

    public void Awake()
    {
        if (instance == null) instance = this;
    }
}
