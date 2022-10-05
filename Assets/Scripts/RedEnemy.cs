using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectFollower))]
public class RedEnemy : Enemy
{
    [SerializeField] float _hitDamage = 15f;
    [SerializeField] float _flyUpSpeed = 3f;
    [SerializeField] float _flyUpDistance = 2f;

    ObjectFollower objectFollower;
    Coroutine attackProcess;
    float startBaseOffset;

    protected override void Start()
    {
        base.Start();

        objectFollower = GetComponent<ObjectFollower>();
        startBaseOffset = agent.baseOffset;

        StartCoroutine(flyUp());
    }

    protected override void FixedUpdate()
    {
        if (agent.enabled && isFliedUp)
        {
            base.FixedUpdate();
        }
    }

    override protected void attackPlayer()
    {
        if (attackProcess == null)
        {
            // stop the enemy
            agent.SetDestination(transform.position);
            agent.enabled = false;
            objectFollower.StartMoving(player.transform);
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

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;
        if (tag == "Player")
        {
            player.GetComponent<Player>().ApplyHealthChanges(_hitDamage);
            DestroyObject(gameObject);
        }
    }
}
