using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CombatEventManager : MonoBehaviour
{
    public static CombatEventManager instance { get; set; }

    public void Awake()
    {
        if (instance == null) instance = this;
    }

    public event Action<int> onEnemyDeath;

    public void OnEnemyDeath(int enemy_id)
    {
        if(onEnemyDeath != null)
        {
            onEnemyDeath.Invoke(enemy_id);
        }
    }
}
