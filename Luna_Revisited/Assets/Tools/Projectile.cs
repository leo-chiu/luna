using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile", menuName = "Inventory/Projectile")]
public class Projectile : Item
{
    public int attack_boost;

    public Sprite projectile_sprite;

    public override void Use()
    {
        ProjectileManager.instance.Select(this);
        Debug.Log("Selected Projectile");
    }
}
