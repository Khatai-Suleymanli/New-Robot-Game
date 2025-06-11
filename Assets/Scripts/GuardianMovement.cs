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

    private float lookRadius = 5f;


    void Start()
    {
        // fill the waypaoints array. add waypoints
        agent = GetComponent<NavMeshAgent>();
        //currentTarget = waypoints[1];

    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance < lookRadius) {
            FacePlayer();
            agent.SetDestination(target.transform.position);

        }
        if (distance > lookRadius) {
            FaceWp();
            if (!agent.pathPending && agent.remainingDistance < 5)
            {
                waypointIndex = waypointIndex == 0 ? 1 : 0;
                Move();
            }


        }
        
       /* if (waypointIndex < waypoints.Length)
        {

            transform.position = Vector3.MoveTowards(transform.position,
                waypoints[waypointIndex].position, speed * Time.deltaTime);

            if (waypoints[waypointIndex] == null)
            {
                return;
            }

            Vector3 direction = waypoints[waypointIndex].position - transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            float maxDegreesdlt = rotationSpeed * Time.deltaTime;


            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDegreesdlt);



            if (Vector3.Distance(this.transform.position, waypoints[waypointIndex].position)
                < 0.1f && waypointIndex != 1)
            {
                waypointIndex++;
            }
            else if (Vector3.Distance(this.transform.position, waypoints[waypointIndex].position)
                < 0.1f && waypointIndex == 1) {
                waypointIndex--;
            }
            else
            {
                //enemy will shoot tower
            }

        }*/
    }

    void FaceWp() {
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


    void Move() {
        if(agent != null && waypoints[waypointIndex] != null ){
            agent.SetDestination(waypoints[waypointIndex].transform.position);

        }
    
    }
}
