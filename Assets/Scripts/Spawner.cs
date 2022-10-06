using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    public List<Enemy> EnemiesList { get => enemiesList; }
    [Header("Spawn settings")]
    [SerializeField] float _maxSpawnSpeed = 2f;
    [SerializeField] float _minSpawnSpeed = 5f;
    [SerializeField] float _speedDecreasingStep = 0.5f;
    [Tooltip("The radius from player of enemy spawning area")]
    [SerializeField] float _enemySpawnRadius = 5f;

    [Header("Enemy settings")]
    [SerializeField] BlueEnemy _blueEnemy;
    [SerializeField] RedEnemy _redEnemy;
    [SerializeField] float _redEnemyYSpawnPoint = 0.4f;
    [SerializeField] int _startBlueEmount = 1;
    [SerializeField] int _blueAmountStep = 1;

    Player player;
    List<Enemy> enemiesList;
    float currentRedAmount = 0;
    float currentBlueAmount = 0;

    ObjectPool<Enemy> enemyPool;


    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        StartCoroutine(spawnProcess());
        GlobalEventManager.OnEnemyDeath.AddListener(OnEmenyDeath);

        enemyPool = new ObjectPool<>();
    }

    Enemy createPooledObjec()
    {
        Enemy instance = Instantiate()
    }

    IEnumerator spawnProcess()
    {
        float currentSpawnSpeed = _minSpawnSpeed;
        float blueAmount = _startBlueEmount;

        enemiesList = new List<Enemy>();

        while (true)
        {
            yield return new WaitForSeconds(currentSpawnSpeed);

            if (currentSpawnSpeed > _maxSpawnSpeed)
            {
                currentSpawnSpeed = Mathf.Clamp(currentSpawnSpeed - _speedDecreasingStep, _maxSpawnSpeed, _minSpawnSpeed);
            }
            else
            {
                blueAmount += _blueAmountStep;
            };

            Vector3 spawnPoint;
            while (!RandomMeshPoint(player.transform.position, _enemySpawnRadius, out spawnPoint)) ;

            float neededRedAmount = currentBlueAmount * 4;
            float availableRedAmount = neededRedAmount - currentRedAmount;

            if (blueAmount > currentBlueAmount && availableRedAmount <= 0)
            {
                spawnEnemy(_blueEnemy, spawnPoint);
                currentBlueAmount++;
            }
            else if (availableRedAmount > 0)
            {
                spawnPoint.y = _redEnemyYSpawnPoint; // To spawn red on the "second floor"
                spawnEnemy(_redEnemy, spawnPoint);
                currentRedAmount++;
            }
        }
    }

    void spawnEnemy(Enemy enemy, Vector3 spawnPoint)
    {
        Enemy spawnedEnemy = Instantiate(enemy, spawnPoint, new Quaternion());
        enemiesList.Add(spawnedEnemy);
    }

    bool RandomMeshPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = new Vector3();
        return false;
    }

    public void OnEmenyDeath(Enemy enemy)
    {
        if (enemy.GetComponent<RedEnemy>())
        {
            currentRedAmount--;
        }
        else
        {
            currentBlueAmount--;
        }

        enemiesList.Remove(enemy);
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnEnemyDeath.RemoveListener(OnEmenyDeath);
    }
}
