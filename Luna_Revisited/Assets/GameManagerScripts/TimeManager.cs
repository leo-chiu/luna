using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance { get; private set; }

    public int ItemExpire;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

}
