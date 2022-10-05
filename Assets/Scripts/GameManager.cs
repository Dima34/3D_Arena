using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    Player player;


    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        SubscribeListeners();
    }

    public void SubscribeListeners() {
        GlobalEventManager.OnEnemyDeath.AddListener(OnEmenyDeath);
        GlobalEventManager.OnExtraDeath.AddListener(OnExtraDeath);
    }
    
    public void OnEmenyDeath(Enemy enemy, float reward){
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
