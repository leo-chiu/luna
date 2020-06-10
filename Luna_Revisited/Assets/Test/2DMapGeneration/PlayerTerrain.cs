using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTerrain : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "Terrain_Block(Clone)")
        {
            other.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Terrain_Block(Clone)")
        {
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
