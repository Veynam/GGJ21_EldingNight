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
        sightingDelay =- Time.deltaTime;
        if (sightingDelay == 0) playerInSight = false;

        Vector3 direction = player.transform.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);

        if (angle<fieldOfView * 0.5f)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position + transform.up, direction, out hit, 30f))
            {
                if (hit.collider.gameObject == player)
                {
                    playerInSight = true;
                    sightingDelay = defaultSightingDelay;
                    waypoints[waypointIndex].position = player.transform.position;
                }
                else playerInSight = false;
            }
        }
    }

    void Chase()
	{
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
            wayPointDistance = Vector3.Distance(transform.position, waypoints[waypointIndex].position);
            if (wayPointDistance < 3f)
            {
                waypointIndex++;
                if (waypointIndex >= waypoints.Length)
                {
                    waypointIndex = 0;
                }
            }
            animator.SetBool("Walk", true);
            nav.SetDestination(waypoints[waypointIndex].position);
        }
	}

    void Bite()
	{
        if (playerInSight && distance <= 2f && dead == false)
        {
            attackDelay -= Time.deltaTime;
            gameObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
            nav.SetDestination(transform.position);
            var lookPos = player.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1f);
            animator.SetBool("Walk", false);
            animator.SetBool("Bite", true);

            RaycastHit hit;

            if (Physics.Raycast(transform.position + transform.up, direction, out hit, 3f))
            {
                if (hit.collider.gameObject == player && attackDelay <= 0)
                {
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
