using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform PlayerTransform;
    public float followDistance = 25f; // Distance within which the enemy starts following
    public float stopDistance = 2f; // Distance at which the enemy stops in front of the player
    NavMeshAgent agent;
    Animator animator;
    public bool walking;
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
            if (distanceToPlayer > stopDistance)
            {
                agent.destination = PlayerTransform.position; // Move towards the player
                if (!walking)
                {
                    animator.SetTrigger("walk");
                    walking = true;
                }
            }
            else
            {
                agent.ResetPath(); // Stop moving when close enough
                walking = false;
            }

            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
        else
        {
            agent.ResetPath(); // This will stop the agent from moving if outside follow distance
            walking = false;
        }
    }
}