using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerUltra : MonoBehaviour
{
    public bool IsUltraReady { get => isUltraReady; }

    [SerializeField] Spawner _spawner;


    GameManager gameManager;
    Player player;
    bool isUltraReady = false;

    void OnEnable()
    {
        player = GetComponent<Player>();
        GlobalEventManager.OnStrenghtChange.AddListener(checkUltra);
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    void checkUltra(float strenght, float maxStrenght)
    {
        if (strenght == maxStrenght)
        {
            setUltra(true);
        }
    }

    void setUltra(bool state)
    {
        isUltraReady = state;
        GlobalEventManager.OnUltraStateChanged.Fire(state);
    }

    public void UseUltra()
    {
        if (isUltraReady)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            gameManager.AddScore(enemies.Length);


            for (int i = 0; i < enemies.Length; i++)
            {
                DestroyObject(enemies[i]);
            }

            _spawner.Clear();
            player.ApplyStrenghtChanges(player.MaxStrenght);

            setUltra(false);
        }
    }

    private void OnDisable()
    {
        GlobalEventManager.OnStrenghtChange.RemoveListener(checkUltra);
    }
}
