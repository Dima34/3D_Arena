using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    public List<BlueEnemy> BlueEnemiesList { get => blueEnemiesList; set => blueEnemiesList = value; }
    public List<RedEnemy> RedEnemiesList { get => redEnemiesList; set => redEnemiesList = value; }

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
    List<BlueEnemy> blueEnemiesList;
    List<RedEnemy> redEnemiesList;

    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        StartCoroutine(spawnProcess());
        GlobalEventManager.OnEnemyDeath.AddListener(OnEmenyDeath);
    }

    IEnumerator spawnProcess()
    {
        float currentSpawnSpeed = _minSpawnSpeed;
        float blueAmount = _startBlueEmount;
        blueEnemiesList = new List<BlueEnemy>();
        redEnemiesList = new List<RedEnemy>();

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

            Debug.DrawLine(player.transform.position, spawnPoint, Color.red, 2f);

            float neededRedAmount = blueEnemiesList.Count * 4;
            float availableRedAmount = neededRedAmount - redEnemiesList.Count;

            if (blueAmount > blueEnemiesList.Count && availableRedAmount <= 0)
            {
                BlueEnemy enemy = Instantiate(_blueEnemy, spawnPoint, new Quaternion(0, 0, 0, 0));
                blueEnemiesList.Add(enemy);
            }
            else if (availableRedAmount > 0)
            {
                spawnPoint.y = _redEnemyYSpawnPoint; // To spawn red on the "second floor"
                RedEnemy redEnemy = Instantiate(_redEnemy, spawnPoint, new Quaternion(0, 0, 0, 0));
                redEnemiesList.Add(redEnemy);
            }
        }
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

    public void OnEmenyDeath(Enemy enemy, float reward)
    {

        if (enemy.GetComponent<RedEnemy>())
        {
            redEnemiesList.Remove(enemy.GetComponent<RedEnemy>());
        }
        else
        {
            blueEnemiesList.Remove(enemy.GetComponent<BlueEnemy>());
        }
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnEnemyDeath.RemoveListener(OnEmenyDeath);

    }
}
