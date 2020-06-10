using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    public float expire_range;
    private Vector2 launch_position;
    public int attack;

    void Start()
    {
        launch_position = transform.position;
    }

    public void setAttack(int attack)
    {
        this.attack = attack;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(launch_position, transform.position) >= expire_range)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyStats>().takeDamage(attack);
            Destroy(gameObject);
        }
        if (collision.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }
}
