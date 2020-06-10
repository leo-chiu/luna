using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    public GameObject player;
}
