using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
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
        distance = Vector3.Distance(transform.position, waypoints[waypointIndex].position);
        if(distance < 1f && caroutineInitiated == false)
		{
            StartCoroutine(Wait());
        }
        PatrolInitiate();
    }

    void PatrolInitiate()
	{
        if(wait == false)
		{
            animator.SetBool("Walk", true);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
	}

    void IncreaseIndex()
	{
        caroutineInitiated = false;
        waypointIndex++;
        if(waypointIndex >= waypoints.Length)
		{
            waypointIndex = 0;
		}
        StartCoroutine(LookAtSmoothly(transform, waypoints[waypointIndex].position, 0.5f));
	}

    IEnumerator Wait()
	{
        wait = true;
        caroutineInitiated = true;
        animator.SetBool("Walk", false);
        yield return new WaitForSeconds(7);
        wait = false;
        IncreaseIndex();
    }

    IEnumerator LookAtSmoothly(Transform objectToMove, Vector3 worldPosition, float duration)
    {
        Quaternion currentRot = objectToMove.rotation;
        Quaternion newRot = Quaternion.LookRotation(worldPosition -
            objectToMove.position, objectToMove.TransformDirection(Vector3.up));

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            objectToMove.rotation =
                Quaternion.Lerp(currentRot, newRot, counter / duration);
            yield return null;
        }
    }
}
