using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{

    public float detection_radius;
    public float drop_radius;

    private Transform spawn_position;
    public float return_threshold;
    public float patrol_threshold;

    public bool chasing = false;
    public bool idle = true;
    public bool patrolling = true;

    public Rigidbody2D rb;

    private Transform player;

    public Transform[] patrol_locations;

    public float patrol_buffer;
    private float time_since_stopped;

    private int patrol_point_index;

    public Enemy enemy;

    public void Start()
    {
        System.Random randomNumber = new System.Random(GetInstanceID());
        player = PlayerManager.instance.player.transform;
        spawn_position = transform.parent.transform;
        rb = GetComponent<Rigidbody2D>();
        time_since_stopped = patrol_buffer;
    }

    public AIPath path;

    public void Update()
    {
        GetComponent<AIPath>().maxSpeed = enemy.stats.speed.getValue();
        if (chasing && drop_radius < Vector2.Distance(player.position, transform.position))
        {
            SetCourseToSpawn();
        }
        else if (!chasing && detection_radius >= Vector2.Distance(player.position, transform.position))
        {
            makeActive();
            SetCourseToPlayer();
        }

        if (!chasing)
        {
            if (!idle && return_threshold >= Vector2.Distance(transform.position, spawn_position.position))
            {
                idle = true;
            }
        }
        if (idle)
        {
            //makeIdle();
            patrolling = true;
            setCourseToPatrolPoint();
        }
        if (patrolling)
        {
            setPatrolPoint();
        }
    }

    public void setPatrolPoint()
    {
        if (patrolling && patrol_threshold >= Vector2.Distance(transform.position, patrol_locations[patrol_point_index].position))
        {
            time_since_stopped -= Time.deltaTime;
        }
        if(time_since_stopped <= 0)
        {
            int previous_patrol_point = patrol_point_index;
            patrol_point_index = Random.Range(0, patrol_locations.Length);
            if (patrol_point_index == previous_patrol_point)
            {
                patrol_point_index = patrol_point_index + 1 % (patrol_locations.Length-1);
                patrol_point_index = Mathf.Clamp(patrol_point_index, 0, patrol_locations.Length - 1);
            }
            setCourseToPatrolPoint();
            time_since_stopped = patrol_buffer;
        }
    }

    public void makeIdle()
    {
        GetComponent<AIPath>().canSearch = false;
        GetComponent<AIPath>().canMove = false;
    }

    public void makeActive()
    {
        GetComponent<AIPath>().canSearch = true;
        GetComponent<AIPath>().canMove = true;
    }

    public void setCourseToPatrolPoint()
    {
        patrolling = true;
        idle = false;
        GetComponent<AIDestinationSetter>().target = patrol_locations[patrol_point_index];
    }

    public void SetCourseToPlayer()
    {
        //Debug.Log("Chase player");
            GetComponent<AIDestinationSetter>().target = player.transform;
            chasing = true;
            idle = false;
    }

    public void SetCourseToSpawn()
    {
        //Debug.Log("Return to spawn");
        GetComponent<AIDestinationSetter>().target = spawn_position;
            chasing = false;
            idle = false;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detection_radius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, drop_radius);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, patrol_threshold);
    }
}
