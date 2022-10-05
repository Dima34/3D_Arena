using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class BlueEnemy : Enemy
{
    public float timeBetweenAttacks;

    [SerializeField] ObjectFollower _enemyBulletObject;
    
    bool alreadyAttacked;

    override protected void attackPlayer()
    {
        WalkPoint = player.position;

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
    
}
