using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int current_health;
    public int max_health;

    public Stat armor;
    public Stat attack;
    public Stat speed;

    public Transform host;

    public void Awake()
    {
        host = transform.parent;
        current_health = max_health;
    }

    public virtual void takeDamage(int damage)
    {
        current_health -= damage;

        if(current_health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Debug.Log("Character has died");
    }
}
