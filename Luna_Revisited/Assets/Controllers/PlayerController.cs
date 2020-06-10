using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerController : MonoBehaviour
{
    public Interactable focus;
    public Camera cam;

    private Vector2 move_velocity;

    private Rigidbody2D rb;

    public Animator animator;

    public PlayerStats stats;

    public Tool handle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        move_velocity = input.normalized * stats.speed.getValue();

        if(Input.GetAxisRaw("Horizontal") > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if(Input.GetAxisRaw("Horizontal") < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        animator.SetFloat("xspeed", Mathf.Abs(move_velocity.x));
        animator.SetFloat("yspeed", move_velocity.y);

        if (Mathf.Abs(move_velocity.y) > 0)
        {
            animator.SetBool("vertical", true);
        }
        else
        {
            animator.SetBool("vertical", false);
        }

        if (Mathf.Abs(move_velocity.x) > 0)
        {
            animator.SetBool("horizontal", true);
        }
        else
        {
            animator.SetBool("horizontal", false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayPos = new Vector2(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
            if (hit)
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    setFocus(interactable);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Inventory.instance.RemoveItem(0, 4))
            {
                Debug.Log("Removed 4 of 0");
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Quest q = QuestManager.instance.all_quests[1];
            GetComponent<QuestLog>().Add(q);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            QuestManager.instance.OnQuestProgress(0);
        }

        if (focus != null && !focus.active)
        {
            removeFocus();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryUI.instance.ToggleInventory();
        }

        adjustToolLocation();

    }

    private void adjustToolLocation()
    {
        Vector3 shoulderToMouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        shoulderToMouseDir.z = 0;
        handle.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + GetComponent<SpriteRenderer>().bounds.size.y/2, transform.position.z) + (handle.range * shoulderToMouseDir.normalized);
        handle.gameObject.transform.right = shoulderToMouseDir.normalized;
    }

    private void check_still_in_range()
    {
        if (focus != null)
        {
            if (focus.radius < Vector2.Distance(transform.position, focus.transform.position))
            {
                focus.onDefocus();
                focus = null;
            }
        }
    }

    public void FixedUpdate()
    {
        rb.MovePosition(rb.position + (move_velocity * Time.fixedDeltaTime));
        check_still_in_range();
    }

    public void setFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        { 
            if (focus != null)
            {
                focus.onDefocus();
            }
            Interactable oldFocus = focus;
            focus = newFocus;
            if (!focus.onFocus(transform))
                focus = oldFocus;
        }
    }

    public void removeFocus()
    {
        if(focus != null)
            focus.onDefocus();
        focus = null;
    }
}
