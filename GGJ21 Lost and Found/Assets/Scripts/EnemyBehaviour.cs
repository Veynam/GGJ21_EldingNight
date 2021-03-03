using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    public Transform[] waypoints;
    public float fieldOfView = 110f;
    public bool playerInSight;
    //public Vector3 lastSighting;

    public GameObject player;

    public int waypointIndex;
    private float wayPointDistance;
    private bool dead = false;

    Animator animator;
    NavMeshAgent agent;

    private float attackDelay;
    private float defaultAttackDelay = 0.35f;

    private float sightingDelay;
    private float defaultSightingDelay = 2f;

    private float distance;
    private Vector3 direction;

    private NavMeshAgent nav;

    MouseLook mouseLook;
    Movement movement;
    Subtitles subtitles;

    void Start()
    {
        attackDelay = defaultAttackDelay;
        sightingDelay = defaultSightingDelay;

        // Get components
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        mouseLook = player.GetComponentInChildren<MouseLook>();
        movement = player.GetComponent<Movement>();
        subtitles = player.GetComponentInChildren<Subtitles>();

        waypointIndex = 0;
    }

    void Update()
    {
        // Get currently playing animation
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        // Get distance from player
        distance = Vector3.Distance(player.transform.position, transform.position);
        direction = player.transform.position - transform.position;

        Sight();
        Chase();
        Patrol();
        Bite();
    }

    void Sight()
	{
        // Start sighting delay count (Used to have the enemy chase the player for a bit even if there is no sight)
        sightingDelay =- Time.deltaTime;
        if (sightingDelay == 0) playerInSight = false;

        // Get player direction and angle
        Vector3 direction = player.transform.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);

        // If player angle is within enemy field of view
        if (angle<fieldOfView * 0.5f)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position + transform.up, direction, out hit, 30f))
            {
                if (hit.collider.gameObject == player)
                {
                    // Enemy notices the player and begins chasing
                    playerInSight = true;
                    sightingDelay = defaultSightingDelay;

                    // Add the player position to be one of the patrol points
                    waypoints[waypointIndex].position = player.transform.position;
                }
                else playerInSight = false;
            }
        }
    }

    void Chase()
	{
        // Upon seeing the player, start moving towards him

        if(playerInSight && distance > 2f)
		{
            animator.SetBool("Walk", true);
            nav.SetDestination(player.transform.position);
		}
	}

    void Patrol()
	{
        if(!playerInSight)
		{
            // Calculate distance to waypoint
            wayPointDistance = Vector3.Distance(transform.position, waypoints[waypointIndex].position);
            if (wayPointDistance < 3f)
            {
                // Go to next waypoint if close to the current one
                waypointIndex++;

                // After iterating over all waypoints, return to the first one
                if (waypointIndex >= waypoints.Length)
                {
                    waypointIndex = 0;
                }
            }
            // Start walking to waypoint
            animator.SetBool("Walk", true);
            nav.SetDestination(waypoints[waypointIndex].position);
        }
	}

    void Bite()
	{
        if (playerInSight && distance <= 2f && dead == false)
        {
            // Start attack delay counting (Used to get the end of attack animation)
            attackDelay -= Time.deltaTime;

            // Stop all enemy movement
            gameObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
            nav.SetDestination(transform.position);

            // Rotate towards player
            var lookPos = player.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1f);

            // Set Bite Animation
            animator.SetBool("Walk", false);
            animator.SetBool("Bite", true);

            RaycastHit hit;

            if (Physics.Raycast(transform.position + transform.up, direction, out hit, 3f))
            {
                if (hit.collider.gameObject == player && attackDelay <= 0)
                {
                    // Kill Player if it is the end of attack animation and player is in range
                    dead = true;
                    mouseLook.dead = true;
                    movement.dead = true;
                    player.GetComponentInChildren<Animator>().SetBool("Death", true);
                }
            }
        }
        else
        {
            animator.SetBool("Bite", false);
            attackDelay = defaultAttackDelay;
        }
	}
}
