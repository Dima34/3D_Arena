using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _maxHealth = 100f;
    [SerializeField] float _strengthReward = 50f;

    public float MaxHealth {
        get{
            return _maxHealth;
        }
    }

    float health;
    
    // Start is called before the first frame update
    void Start()
    {
        health = _maxHealth;
    }

    public void ApplyHealthChanges(float points, out float remainHealth){
        health += points;
        remainHealth = health;
        checkHealth();
    }

    void checkHealth(){
        if(health <= 0){
            GlobalEventManager.OnEnemyDeath.Fire(_strengthReward);
            DestroyObject(gameObject);
        }
    }
}
