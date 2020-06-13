using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{ 
    public static ProjectileManager instance { get; set; }
    public void Awake()
    {
        if (instance == null) instance = this;
    }

    public Projectile equipped_projectile;

    public void Select(Projectile projectile)
    {
        equipped_projectile = projectile;
    }
}
