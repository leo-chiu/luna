using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementPlatformer : MonoBehaviour
{
    public Rigidbody2D rb;

    public float move_speed;
    public float jump_strength;

    public LayerMask whatIsGround;
    public Transform groundCheck;
    private bool isGrounded;
    public float checkRadius;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        float move_direction = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(move_direction * move_speed, rb.velocity.y);

    }

    public void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.velocity = Vector2.up * jump_strength;
        }
    }
}
