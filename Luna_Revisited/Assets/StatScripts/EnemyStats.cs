using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    public float attack_cooldown;
    public float cooldown_timer;
    public bool canAttack = true;

    public Animator anim;

    public void elapseTime()
    {
        // to be called in the enemy's update method every frame
        if(0 < cooldown_timer)
        {
            cooldown_timer -= Time.deltaTime;
        }
        else
        {
            canAttack = true;
        }
    }

    public override void takeDamage(int damage)
    {
        base.takeDamage(damage);
        Debug.Log("Enemy has taken " + damage + " damage!");
    }

    public void resetCooldown()
    {
        canAttack = false;
        cooldown_timer = attack_cooldown;
    }

    public override void Die()
    {
        Death();
    }

    public void Death()
    {
        Debug.Log("Enemy has died");
        DropItems();
        // play some death animation
        // PlayDeathAnimtion();
        Destroy(gameObject);
    }

    public void DropItems()
    {
        Debug.Log("Drop Items");
        GameObject drop_template = (GameObject)Resources.Load("Prefabs/Item/Loot", typeof(GameObject));
        // refer to the enemy drop table and roll dice 
        ItemPickup drop = Instantiate(drop_template, transform.position, Quaternion.identity, null).GetComponentInChildren<ItemPickup>();
        
        drop.primeItem(1, 20);

    }

    public IEnumerator PlayDeathAnimtion()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }

}
