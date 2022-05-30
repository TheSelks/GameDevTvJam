using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public GameObject target;
    private NavMeshAgent agent;
    float enemyAttackDistance = 1.0f;
    float distanceToPlayer;
    bool recharge;
    private Animator _animator;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        _animator = GetComponent<Animator>();
        recharge = false;
    }

    void Update()
    {
        agent.SetDestination(target.transform.position);

        if (isPlayerInAttackDistance())
        {
            if(recharge == false)
            {
                agent.speed = 0f;
                //attack code here
                _animator.SetBool("Attack", true);
                recharge = true;
                StartCoroutine(Timer());
                IEnumerator Timer()
                {
                    yield return new WaitForSeconds(1f);
                    _animator.SetBool("Attack", false);
                    recharge = false;
                    agent.speed = 2.0f;
                }
            }
        }
        _animator.SetFloat("Speed", agent.speed);
    }

    bool isPlayerInAttackDistance()
    {
        distanceToPlayer = (target.transform.position - transform.position).magnitude;
        return distanceToPlayer <= enemyAttackDistance;
    }
}

