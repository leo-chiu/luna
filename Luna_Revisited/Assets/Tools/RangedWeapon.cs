using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    public GameObject projectile;
    public float projectile_speed;

    public override void Cast()
    {
        base.Cast();
        shoot();
    }

    public void shoot()
    {
        GameObject shot = Instantiate(projectile, transform.position, Quaternion.identity);
        Rigidbody2D shot_rb = shot.GetComponent<Rigidbody2D>();
        shot.GetComponent<ProjectileCollision>().setAttack(damage_bonus + user.GetComponent<PlayerStats>().attack.getValue());
        shot_rb.AddForceAtPosition(transform.right*projectile_speed, shot.transform.position, ForceMode2D.Impulse);
    }
}
