using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ObjectFollower))]

public class RedEnemy : MonoBehaviour
{
    public LayerMask WhatIsGround, WhatIsPlayer;
    public Vector3 WalkPoint;
    public float WalkPointRange;
    bool playerInSightRange, playerInAttackRange;


    [SerializeField] float _hitDamage = 15f;
    [SerializeField] float _sightRange, _attackRange;
    [SerializeField] float _flyUpSpeed = 3f;
    [SerializeField] float _flyUpDistance = 2f;

    NavMeshAgent agent;
    Player player;
    ObjectFollower objectFollower;
    Coroutine attackProcess;
    float startBaseOffset;

    bool walkPointSet;
    bool isFliedUp = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        objectFollower = GetComponent<ObjectFollower>();
        player = GameObject.Find("Player").GetComponent<Player>();
        startBaseOffset = agent.baseOffset;

        StartCoroutine(flyUp());
    }

    private void FixedUpdate()
    {
        if (agent.enabled && isFliedUp)
        {
            playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, WhatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, WhatIsPlayer);

            RaycastHit forwardObstacleHit = obstacleInWay(transform.position - player.transform.position, _attackRange);

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
            agent.SetDestination(WalkPoint);

        Vector3 distanceToWalkPoint = transform.position - WalkPoint;
        Debug.Log(distanceToWalkPoint.magnitude);
        
        Debug.DrawLine(transform.position, WalkPoint);
        // WalkPoint reched
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    void searchWalkPoint()
    {
        float randomZ = Random.Range(-WalkPointRange, WalkPointRange);
        float randomX = Random.Range(-WalkPointRange, WalkPointRange);

        WalkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // is that point on the ground
        if (Physics.Raycast(WalkPoint, -transform.up, 2f, WhatIsGround))
            walkPointSet = true;
    }

    void chasePlayer()
    {
        agent.SetDestination(player.transform.position);
    }

    void attackPlayer()
    {
        if(attackProcess == null)
        {
            // stop the enemy
            agent.SetDestination(transform.position);
            agent.enabled = false;
            objectFollower.StartMoving(player.transform);
            //attackProcess = StartCoroutine(Attack());
        }
    }

    IEnumerator flyUp()
    {
        agent.baseOffset = -_flyUpDistance;
        while (agent.baseOffset < startBaseOffset)
        {
            agent.baseOffset += _flyUpSpeed * Time.deltaTime;
            yield return null;
        }

        isFliedUp = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _sightRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _attackRange);

    }

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;
        if (tag == "Player")
        {
            player.ApplyHealthChanges(_hitDamage);
            DestroyObject(gameObject);
        }
    }
}
