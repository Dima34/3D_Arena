using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerUltra : MonoBehaviour
{
    public bool IsUltraReady { get => isUltraReady; }

    [SerializeField] Spawner _spawner;

    Player player;
    bool isUltraReady = false;
    
    void OnEnable()
    {
        player = GetComponent<Player>();
        GlobalEventManager.OnStrenghtChange.AddListener(checkUltra);
    }

    void checkUltra(float strenght, float maxStrenght)
    {
        if(strenght == maxStrenght)
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
            for(int i = 0; i < _spawner.EnemiesList.Count; i++)
            {
                if (_spawner.EnemiesList[i] != null)
                {
                    GlobalEventManager.OnRewardedEnemyDeath.Fire(_spawner.EnemiesList[i], _spawner.EnemiesList[i].StrengthReward);
                    DestroyObject(_spawner.EnemiesList[i].gameObject);
                }
            }

            player.ApplyStrenghtChanges(player.MaxStrenght);

            setUltra(false);
        }
    }

    private void OnDisable()
    {
        GlobalEventManager.OnStrenghtChange.RemoveListener(checkUltra);
    }
}
