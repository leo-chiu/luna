using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCameraScript : MonoBehaviour
{
    public Transform focus;

    public float camera_move_speed;

    public void Start()
    {
        focus = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Update()
    {
        Vector3 focus_pos = focus.position;
        focus_pos.z = -5f;
        transform.position = Vector3.Lerp(transform.position, focus_pos, Time.deltaTime * camera_move_speed);
    }
}
