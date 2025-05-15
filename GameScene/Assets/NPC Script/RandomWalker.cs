using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomWalker : MonoBehaviour
{
    public float walkRadius = 10f;
    public float pickInterval = 5f;  // Time between choosing new destinations

    private float timer = 0f;
    private NavMeshAgent agent;
    private bool isWaiting = true;
    private float waitTime = 5f;  // Make the NPC stand idle for 5 seconds

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (isWaiting)
        {
            if (timer >= waitTime)
            {
                isWaiting = false;
                timer = 0f;
                MoveToRandomPoint();
            }
        }
        else
        {
            // If agent reaches the target, start waiting again
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                isWaiting = true;
                timer = 0f;
            }
        }
    }

    void MoveToRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, walkRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}
