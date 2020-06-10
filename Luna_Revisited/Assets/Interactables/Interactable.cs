using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 5f;

    public bool isFocus = false;

    public bool has_interacted = false;

    public Transform player;

    public bool active = true;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius) ;
    }

    public virtual void Interact()
    {
    }

    public void Update()
    {
    }

    public bool onFocus(Transform playerTransform)
    {
        if (radius >= Vector2.Distance(transform.position, playerTransform.position))
       {
            isFocus = true;
            player = playerTransform;
            Interact();
            has_interacted = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void onDefocus()
    {
        isFocus = false;
    }
}
