using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hostile", menuName = "Hostile")] [System.Serializable]
public class Hostile : ScriptableObject 
{
    public int enemy_id;

    public int max_health;
    public int base_armor;
    public int base_attack;
    public int base_speed;
}
