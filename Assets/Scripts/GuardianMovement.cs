using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class GuardianMovement : MonoBehaviour
{
    public Transform[] waypoints; //array of waypoints(enemies' path)
    private int waypointIndex = 0; //this the current waypoiny enemy is moving to
    //public float speed = 3.0f;  //enemy movement speed


    private NavMeshAgent agent;
    public float rotationSpeed = 20.0f; //rotation speed of the enemy robot

    public Transform target; //player

    private float lookRadius = 100f;

    public float viewdotProductValue = 0.65f;

    public LayerMask obstacleMask;

    private Animator animator;


    void Start()
    {
        // fill the waypaoints array. add waypoints
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        //currentTarget = waypoints[1];

    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (CanSeePlayer())  // if canSee
        {
            FacePlayer();
            agent.SetDestination(target.transform.position);
            agent.speed = 6f;

            animator.SetFloat("Speed", 1.5f);

        }
        else
        {
            FaceWp();
            if (!agent.pathPending && agent.remainingDistance < 10)
            {
                waypointIndex = waypointIndex == 0 ? 1 : 0;
                Move();
            }
        }
        /*if (distance > lookRadius)
        {
            


        }*/

        
    }

    void FaceWp()
    {
        Vector3 direction = (waypoints[waypointIndex].transform.position - this.transform.position).normalized;
        direction.y = 0;

        if (direction.magnitude > 0.01f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void FacePlayer()
    {
        Vector3 direction = (target.transform.position - this.transform.position).normalized;
        direction.y = 0;

        if (direction.magnitude > 0.01f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 20);
        }
    }


    void Move()
    {
        if (agent != null && waypoints[waypointIndex] != null)
        {
            agent.SetDestination(waypoints[waypointIndex].transform.position);
            animator.SetFloat("Speed", 0.8f);

        }

    }


    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (target.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(target.transform.position, transform.position);

        if (distanceToPlayer > lookRadius)
        {
            return false;
        }

        float dot = Vector3.Dot(transform.forward, directionToPlayer);
        if (dot < viewdotProductValue)
        {
            return false;
        }

        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, lookRadius))
        {
            if (hit.transform.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        if (target == null) {
            return;
        }

        Vector3 guardianPos = transform.position;
        Vector3 forward = transform.forward;

        Vector3 ToPlayer = (target.position - transform.position).normalized;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(guardianPos, guardianPos + forward * 2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(guardianPos, target.position);
    }
}
