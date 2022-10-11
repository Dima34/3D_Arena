using UnityEngine;
using System;


public class Player : MonoBehaviour
{
    public Action<float, float> OnHealthChange;
    public Action<float, float> OnStrenghtChange;
    public Action<float, float, float, float> OnPlayerInit;
    public static Player Current;

    [Header("Health")]
    [SerializeField] float _maxHealth = 100f;
    [SerializeField] float _maxStrength = 100f;

    [Space(5)]
    [Header("Strenght")]
    [SerializeField] float _initialHealth = 100f;
    [SerializeField] float _initialStrength = 50f;

    public float Health
    {
        get { return health; }
    }

    public float MaxHealth
    {
        get { return _maxHealth; }
    }

    public float Strenght
    {
        get { return strength; }
    }

    public float MaxStrenght
    {
        get { return _maxStrength; }
    }

    float health = 0;
    float strength = 0;

    private void Awake() {
        Current = this;
    }

    void Start()
    {
        // Check if initial > max
        health = Mathf.Clamp(_initialHealth, 0, _maxHealth);
        strength = Mathf.Clamp(_initialStrength, 0, _maxStrength);

        OnPlayerInit?.Invoke(health, _maxHealth, strength, _maxStrength);
    }

    public void ApplyHealthChanges(float changePoints)
    {
        health -= changePoints;
        health = Mathf.Clamp(health, 0, _maxHealth);
        OnHealthChange?.Invoke(health, _maxHealth);
        checkHealth();
    }

    void checkHealth()
    {
        if (health == 0)
            GameManager.Current.OnEndgame?.Invoke();
    }

    public void ApplyStrenghtChanges(float changePoints)
    {
        strength -= changePoints;
        strength = Mathf.Clamp(strength, 0, _maxStrength);
        OnStrenghtChange?.Invoke(strength, _maxStrength);
    }
}
