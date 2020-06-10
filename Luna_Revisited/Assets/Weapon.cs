using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Tool
{
    public int damage_bonus;

    public override void Cast()
    {
        base.Cast();
        attack();
    }

    public void attack()
    {
        if (focus != null && focus.GetComponent<Enemy>() != null)
        {
            focus.GetComponent<Enemy>().Interact();
            focus = null;
        }
    }
}
