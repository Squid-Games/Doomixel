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
    
    // Attack
    public float attackCooldown;
    bool attacked;

    public float sightRadius, attackRadius;
    bool playerInSightRange, playerInAttackRange;

    public Vector3[] waypointPositions = null;
    private int waypointIndex = 0;

    public int maxWalkTilesRange = 15;
    public int minPathLength = 3;
    public int maxPathLength = 10;

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

        if(distanceLeft.magnitude < 5f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        if (waypointPositions is null)
            return;

        walkPoint = waypointPositions[waypointIndex++];
        waypointIndex %= waypointPositions.Length;

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
