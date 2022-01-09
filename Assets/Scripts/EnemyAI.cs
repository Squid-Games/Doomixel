using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform playerTransform;
    
    // Walk around
    Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attack
    public float attackCooldown;
    bool attacked;

    public float sightRadius, attackRadius;
    bool playerInSightRange, playerInAttackRange;

    void Awake()
    {
        playerTransform = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = false; // use some raycasting using the orientation and radius
        playerInAttackRange = false; // use some raycasting or euclidian distance (because the enemy is already facing the player)

        if(!playerInSightRange && !playerInAttackRange)
        { 
            MoveAround();
        }
        if(playerInSightRange && !playerInAttackRange) 
        { 
            ChasePlayer();
        }
        if(playerInAttackRange && playerInAttackRange) 
        {
            AttackPlayer();
        }
    }

    private void MoveAround() 
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        else
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceLeft = transform.position - walkPoint;

        if(distanceLeft.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        // the enemies are walking on a straight line
        float orientation = Random.Range(-1, 1);
        float randomX = (orientation <= 0.0f) ? Random.Range(-walkPointRange, walkPointRange) : 0;
        float randomZ = (orientation > 0.0f) ? Random.Range(-walkPointRange, walkPointRange) : 0;
    
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
       
        walkPointSet = true;
    }

    private void ChasePlayer() 
    {
        agent.SetDestination(playerTransform.position);
    }

    private void AttackPlayer() 
    {
        agent.SetDestination(transform.position);
        if(!attacked)
        {
            attacked = true;
            Debug.Log("attacked");
            Invoke(nameof(AttackAgain), attackCooldown);
        }
    }

    private void AttackAgain()
    {
        attacked = false;
    }
}
