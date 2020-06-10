using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    public LayerMask layer;
    public float range;

    public GameObject user;
    public GameObject focus;

    public void Awake()
    {
        user = transform.parent.gameObject;
    }

    void Update()
    {
        if (user.GetComponent<PlayerStats>().canAttack)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Cast();
                user.GetComponent<PlayerStats>().resetCooldown();
            }
        }
    }

    public virtual void Cast()
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, range, layer);
        foreach (Collider2D collider in collisions)
        {
            focus = collider.gameObject;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
