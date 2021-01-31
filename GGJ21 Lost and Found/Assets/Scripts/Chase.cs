using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Chase : MonoBehaviour
{
    public float fieldOfView = 110f;
    public bool playerInSight;
    public Vector3 lastSighting;

    public GameObject player;

    Animator animator;
    NavMeshAgent agent;

    private float attackDelay;
    private float defaultAttackDelay = 0.96f;

    private Vector3 previousLastSighting;
    private SphereCollider col;

    void Start()
    {
        // Get components
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        col = GetComponent<SphereCollider>();
    }

    void Update()
    {
        attackDelay = defaultAttackDelay;

        // Get currently playing animation
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        // Get player direction from monster
        Vector3 direction = player.transform.position - transform.position;
    }
    /*
	private void OnTriggerStay(Collider other)
	{
		if(other.gameObject == player)
		{
            playerInSight = false;
		}
	}
    */
}
