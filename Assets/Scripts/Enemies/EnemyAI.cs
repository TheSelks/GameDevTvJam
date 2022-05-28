using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public GameObject target;
    private NavMeshAgent agent;
    float enemyVisionDistance = 6.5f;
    float enemyAttackDistance = 1.0f;
    float distanceToPlayer;
    bool recharge;
    public List<Transform> patrolPoints;
    int patrolPointIndex;
    Vector3 patrolPointPosition;
    private Animator _animator;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        _animator = GetComponent<Animator>();
        recharge = false;
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

        if (isPlayerInAttackDistance())
        {
            if(recharge == false)
            {
                
                //attack code here
                _animator.SetBool("Attack", true);
                recharge = true;
                StartCoroutine(Timer());
                IEnumerator Timer()
                {
                    yield return new WaitForSeconds(1.5f);
                    recharge = false;
                }
            }
        }
        else
        {
            _animator.SetBool("Attack", false);
        }
        

        _animator.SetFloat("Speed", agent.speed);
    }

    bool isPlayerInVisionDistance()
    {
        distanceToPlayer = (target.transform.position - transform.position).magnitude;
        return distanceToPlayer <= enemyVisionDistance;
    }

    bool isPlayerInAttackDistance()
    {
        distanceToPlayer = (target.transform.position - transform.position).magnitude;
        return distanceToPlayer <= enemyAttackDistance;
    }
}

