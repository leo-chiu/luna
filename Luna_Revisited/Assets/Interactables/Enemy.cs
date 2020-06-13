using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Interactable
{

    public Hostile enemy;
    public GameObject focus;
    public EnemyStats stats;

    public void Start()
    {
        fillStats();
    }

    public void fillStats()
    {
        stats.armor.base_value = enemy.base_armor;
        stats.attack.base_value = enemy.base_attack;
        stats.speed.base_value = enemy.base_speed;

        stats.max_health = enemy.max_health;
        stats.current_health = stats.max_health;
    }

    public void FixedUpdate()
    {
        if(focus != null && focus.gameObject.tag == "Player")
        {
            if(stats.canAttack)
            {
                Attack();
                stats.resetCooldown();
            }
        }
        stats.elapseTime();
    }

    public override void Interact()
    {
        base.Interact();
        stats.takeDamage(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().attack.getValue());
    }

    public virtual void Attack()
    {
        // overridden in derived enemies
        Debug.Log("Deal " + stats.attack.getValue() + " damage to player");
        focus.GetComponent<PlayerStats>().takeDamage(stats.attack.getValue());
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Enemy has collided with player!");
            SetFocus(collision.gameObject);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Enemy no longer with player!");
            RemoveFocus();
        }
    }

    public void SetFocus(GameObject newFocus)
    {
        focus = newFocus;
    }

    public void RemoveFocus()
    {
        if(focus != null)
            focus = null;
    }
}
