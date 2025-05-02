using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform PlayerTransform;
    public float followDistance = 25f; // Distance within which the enemy starts following
    NavMeshAgent agent;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerTransform.position);

        if (distanceToPlayer <= followDistance)
        {
            agent.destination = PlayerTransform.position;
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
        else
        {
            agent.ResetPath(); // This will stop the agent from moving.
        }
    }
}