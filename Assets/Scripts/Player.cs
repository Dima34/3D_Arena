using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float _maxHealth = 100f;
    [SerializeField] float _maxStrength = 100f;

    [Space(5)]
    [Header("Strenght")]
    [SerializeField] float _initialHealth = 100f;
    [SerializeField] float _initialStrength = 50f;
    
    public float Health{
        get{ return health;}
    }

    public float MaxHealth{
        get{ return _maxHealth;}
    }

    public float Strenght{
        get{ return health;}
    }

    public float MaxStrenght{
        get{ return _maxStrength;}
    }

    float health = 0;
    float strength = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Check if initial > max
        health = Mathf.Clamp(_initialHealth, 0, _maxHealth);
        strength = Mathf.Clamp(_initialStrength, 0, _maxStrength);
    }

    public void ApplyHealthChanges(float changePoints){
        health -= changePoints;
        health = Mathf.Clamp(health, 0, _maxHealth);
    }

    public void ApplyStrenghtChanges(float changePoints){
        strength -= changePoints;
        strength = Mathf.Clamp(strength, 0, _maxStrength);

        Debug.Log("Strength applied " + strength);
    }

       
}
