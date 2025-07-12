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

    

    




    private PlayerHealth playerHealth;

    private Vector3 originalCameraPosition;


    [Header("Blend Tree")]
    private Animator animator;
    private float currentSpeedBT; // speed variable for the blend tree
    private int speedHash;

    public float acceleration = 2.0f;
    public float deceleration = 4.0f;

    public float maxWalkVelocity = 0f;
    public float maxRunVelocity = 1.0f;

    private bool isAttacking = false;
    private float attackTime = 2f;
    private float attackStart = 0f;
    private float originalSpeed;


    void Start()
    {
        // originalSpeed = agent.speed;
        originalCameraPosition = Camera.main.transform.position;


        if (target != null)
        {
            playerHealth = target.GetComponent<PlayerHealth>();
        }

        // fill the waypaoints array. add waypoints
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        //currentTarget = waypoints[1];

        speedHash = Animator.StringToHash("Speed");

    }

    // Update is called once per frame
    void Update()
    {

        float distance = Vector3.Distance(target.transform.position, transform.position);
        bool isRunning = false;

        HandleAttack(); // instead of handling them in update manually
        HandleMovementAnimation();



        if (CanSeePlayer())  // if canSee
        {
            FacePlayer();

            agent.SetDestination(target.transform.position);


            if (distance > 5f)
            {
                //animator.SetFloat("Speed", 1.5f);
                agent.speed = 6f;
                currentSpeedBT = Mathf.Lerp(currentSpeedBT, maxRunVelocity, acceleration * Time.deltaTime);

            }
            if (distance <= 5f && attackStart == 0f) {
                StartAttacking();
            }


        }
        else
        {
            // agent.isStopped = false;
            FaceWp();
            
            if (agent.remainingDistance < 10)
            {
                waypointIndex = waypointIndex == 0 ? 1 : 0;
                Move();
            }

            currentSpeedBT = Mathf.Lerp(currentSpeedBT, maxWalkVelocity, deceleration * Time.deltaTime);
        }
    }

    private void HandleAttack() {
        if (isAttacking)
        {
            attackStart += Time.deltaTime;
            if (attackStart >= attackTime)
            {
                isAttacking = false;
                attackStart = 0;
                agent.speed = originalSpeed;
                agent.isStopped = false;
            }
        }

    }


    //Blend Tree related animation controlling

    private void HandleMovementAnimation() {

        //float normalizedSpeed = agent.velocity.magnitude / agent.speed;

        float targetSpeed = CanSeePlayer() ? 1f : 0f;

        //animator.SetFloat(speedHash, targetSpeed, 0.1f, Time.deltaTime);
        animator.SetFloat(speedHash,currentSpeedBT);

    }

    private void StartAttacking()
    {
        isAttacking = true;
        attackStart = 0f;
        agent.isStopped = true;
        currentSpeedBT = 0f;
        //agent.speed = 0f;
        animator.SetTrigger("Attack");
        playerHealth.TakeDamage(10);
        Debug.Log("Attacked The player");

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
            //animator.SetFloat("Speed", 0.8f);

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
        if (target == null)
        {
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