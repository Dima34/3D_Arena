using System.Collections.Generic;
using UnityEngine;
using System;


[RequireComponent(typeof(Player))]
public class PlayerUltra : MonoBehaviour
{
    public bool IsUltraReady { get => isUltraReady; }
    public Action<bool> OnUltraStateChanged;


    [SerializeField] Spawner _spawner;
    Player player;

    bool isUltraReady = false;

    void Start()
    {
        player = Player.Current;
        player.OnStrenghtChange += (checkUltra);
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
        OnUltraStateChanged.Invoke(state);
    }

    public void UseUltra()
    {
        if (isUltraReady)
        {
            List<Enemy> activeEnemies = Utils.GetActiveEnemies();

            foreach (var enemy in activeEnemies)
            {
                GameManager.Current.OnRewardedEnemyDeath?.Invoke(enemy, enemy.StrengthReward);
                enemy.gameObject.SetActive(false);
            }

            player.ApplyStrenghtChanges(player.MaxStrenght);

            setUltra(false);
        }
    }

    private void OnDestroy()
    {
        player.OnStrenghtChange -= (checkUltra);
    }
}
