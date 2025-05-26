using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GuardianMovement : MonoBehaviour
{
    public Transform[] waypoints; //array of waypoints(enemies' path)
    private int waypointIndex = 0; //this the current waypoiny enemy is moving to
    public float speed = 3.0f;  //enemy movement speed

    public float rotationSpeed = 20.0f; //rotation speed of the enemy robot

    void Start()
    {
        // fill the waypaoints array. add waypoints

    }

    // Update is called once per frame
    void Update()
    {
        if (waypointIndex < waypoints.Length)
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



            if (Vector3.Distance(transform.position, waypoints[waypointIndex].position)
                < 0.1f && waypointIndex != 1)
            {
                waypointIndex++;
            }
            else if (Vector3.Distance(transform.position, waypoints[waypointIndex].position)
                < 0.1f && waypointIndex == 1) {
                waypointIndex--;
            }
            else
            {
                //enemy will shoot tower
            }

        }
    }
}
