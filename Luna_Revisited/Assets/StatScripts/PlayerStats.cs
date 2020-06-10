using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    public float attack_cooldown;
    public float cooldown_timer;
    public bool canAttack = true;

    public void FixedUpdate()
    {
        if(!canAttack)
        elapseTime();
    }

    public void elapseTime()
    {
        // to be called in the player's update method every frame
        if (0 < cooldown_timer)
        {
            cooldown_timer -= Time.deltaTime;
        }
        else
        {
            canAttack = true;
        }
    }

    public void resetCooldown()
    {
        canAttack = false;
        cooldown_timer = attack_cooldown;
    }

    void Start()
    {
        EquipmentManager.instance.onEquipmentChanged += onEquipmentChange;
    }

    public void onEquipmentChange(Equipment new_item, Equipment old_item)
    {
        if (new_item != null)
        {
            attack.addModifier(new_item.attack_modifier);
            armor.addModifier(new_item.armor_modifier);
        }

        if(old_item != null)
        {
            attack.removeModifier(old_item.attack_modifier);
            armor.removeModifier(old_item.armor_modifier);
        }
    }
}
