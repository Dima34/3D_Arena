using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

[RequireComponent(typeof(NavMeshAgent))]
public class BlueEnemy : MonoBehaviour
{
    [SerializeField] ObjectFollower _enemyBulletObject;
    NavMeshAgent agent;

    Transform player;
    public LayerMask WhatIsGround, WhatIsPlayer;
    

    // patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;


    // attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    // states
    [SerializeField] float _sightRange, _attackRange;
    public bool playerInSightRange, playerInAttackRange;


    private void Awake() {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, WhatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, WhatIsPlayer);

        RaycastHit forwardObstacleHit = obstacleInWay(transform.position - player.position, _attackRange);

        if (forwardObstacleHit.distance <= _sightRange && forwardObstacleHit.collider.tag == "Player")
        {
            if (playerInAttackRange)
            {
                attackPlayer();
            }
            else if (playerInSightRange)
            {
                chasePlayer();
            }
        }
        else
        {
            patroling();
        }
    }

    RaycastHit obstacleInWay(Vector3 vector, float rayLenght)
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, -vector.normalized, out hit);

        return hit;
    }

    void patroling()
    {
        if (!walkPointSet) searchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // walkpoint reched
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    void searchWalkPoint()
    {
        float randomZ  = Random.Range(-walkPointRange, walkPointRange);
        float randomX  = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // is that point on the ground
        if (Physics.Raycast(walkPoint, -transform.up, 2f, WhatIsGround))
            walkPointSet = true;
    }

    void chasePlayer()
    {
        agent.SetDestination(player.position);
    }

    void attackPlayer()
    {
        walkPoint = player.position;

        // stop the enemy
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            spawnBullet();

            alreadyAttacked = true;
            Invoke(nameof(resetAttack), timeBetweenAttacks);
        }
    }

    void resetAttack()
    {
        alreadyAttacked = false;
    }

    void spawnBullet()
    {
        Vector3 spawnPos = transform.position + transform.forward * transform.localScale.z;
        ObjectFollower bullet = Instantiate(_enemyBulletObject, spawnPos, transform.rotation);
        bullet.StartMoving(player);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _sightRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _attackRange);

    }

    
}
