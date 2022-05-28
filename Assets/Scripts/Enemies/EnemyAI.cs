using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public GameObject target;
    private NavMeshAgent agent;
    float enemyVisionDistance = 6.5f;
    float distanceToPlayer;
    public List<Transform> patrolPoints;
    int patrolPointIndex;
    Vector3 patrolPointPosition;
    private Animator _animator;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        patrolPointPosition = patrolPoints[patrolPointIndex].position;

        if ((transform.position - patrolPointPosition).magnitude < 1)
        {
            ++patrolPointIndex;

            if (patrolPointIndex >= patrolPoints.Count)
            {
                patrolPointIndex = 0;
            }

            patrolPointPosition = patrolPoints[patrolPointIndex].position;
        }

        agent.SetDestination(patrolPointPosition);

        if (isPlayerInVisionDistance())
        {
            agent.SetDestination(target.transform.position);
        }
        

        _animator.SetFloat("Speed", agent.speed);
    }

    bool isPlayerInVisionDistance()
    {
        distanceToPlayer = (target.transform.position - transform.position).magnitude;
        return distanceToPlayer <= enemyVisionDistance;
    }
}

