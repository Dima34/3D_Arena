using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Spawn speed")]
    [SerializeField] float _maxSpawnSpeed = 5f;
    [SerializeField] float _minSpawnSpeed = 2f;
    [SerializeField] float _speedDecreasingStep = 0.5f;

    [Header("Enemy settings")]
    [SerializeField] GameObject _blueEnemy;
    [SerializeField] GameObject _redEnemy;
    [SerializeField] int _startBlueEmount = 1;
    [SerializeField] int _blueAmountStep = 1;


    Player player;

    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        SubscribeListeners();
    }

    public void SubscribeListeners(){
        GlobalEventManager.OnEnemyDeath.AddListener(OnEmenyDeath);
        GlobalEventManager.OnExtraDeath.AddListener(OnExtraDeath);
    }

    public void OnEmenyDeath(float reward){
        player.ApplyStrenghtChanges(-reward);
    }

    public void OnExtraDeath(){
        if(player.Health < player.MaxHealth){
            player.ApplyHealthChanges(-(player.MaxHealth / 2));
        } else{
            player.ApplyStrenghtChanges(-(player.MaxStrenght / 2));
        }
    }
}
