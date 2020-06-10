using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; set; }

    public int max_item_pickup_sounds_at_once;
    public int current_item_pickup_sounds;

    public void Awake()
    {
        if (instance == null) instance = this;
    }
}
