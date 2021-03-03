using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    /// <summary>
    /// Old script - Not used anywhere
    /// Patrol script before level and navmesh were made
    /// </summary>
    
    public Transform[] waypoints;
    public int speed;

    public int waypointIndex;
    private float distance;

    Animator animator;

    bool wait = false;
    bool caroutineInitiated = false;

    void Start()
    {
        waypointIndex = 0;
        transform.LookAt(waypoints[waypointIndex].position);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Calculate distance 
        distance = Vector3.Distance(transform.position, waypoints[waypointIndex].position);

        // Decide wheter to wait first or start walking toward next point
        if (distance < 1f && caroutineInitiated == false)
		{
            StartCoroutine(Wait());
        }
        PatrolInitiate();
    }

    void PatrolInitiate()
	{
        // Start walking forward (because the enemy is either rotating towards next point or is already looking at it)
        if(wait == false)
		{
            animator.SetBool("Walk", true);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
	}

    void IncreaseIndex()
	{
        // Set next point destination
        caroutineInitiated = false;
        waypointIndex++;

        // After iterating over all waypoints, return to the first one
        if(waypointIndex >= waypoints.Length)
		{
            waypointIndex = 0;
		}

        // Start rotation Coroutine
        StartCoroutine(LookAtSmoothly(transform, waypoints[waypointIndex].position, 0.5f));
	}

    IEnumerator Wait()
	{
        // Wait 7 seconds at a patrol point
        wait = true;
        caroutineInitiated = true;
        animator.SetBool("Walk", false);
        yield return new WaitForSeconds(7);
        wait = false;

        // Set next point destination
        IncreaseIndex();
    }

    IEnumerator LookAtSmoothly(Transform objectToMove, Vector3 worldPosition, float duration)
    {
        // To make enemy rotate smoothly instead of just turning instantly

        Quaternion currentRot = objectToMove.rotation;
        Quaternion newRot = Quaternion.LookRotation(worldPosition - objectToMove.position, objectToMove.TransformDirection(Vector3.up));

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            objectToMove.rotation = Quaternion.Slerp(currentRot, newRot, counter / duration);
            yield return null;
        }
    }
}
