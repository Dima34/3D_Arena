using UnityEngine;
using System.Collections.Generic;
using System;


public class GameManager : MonoBehaviour
{
    public static GameManager Current;
    public Action<Enemy, float> OnRewardedEnemyDeath;
    public Action<Enemy> OnEnemyDeath;
    public Action OnExtraDeath;
    public Action<int> OnScoreChange;
    public Action<int> OnScoreInit;
    public Action OnEndgame;


    Player player;
    int score;

    private void Awake()
    {
        Current = this;
    }

    void Start()
    {
        resumeGame();
        player = GameObject.FindObjectOfType<Player>();
        SubscribeListeners();
        initScore();
    }

    void initScore()
    {
        score = 0;
        OnScoreInit?.Invoke(score);
    }

    public void SubscribeListeners()
    {
        OnRewardedEnemyDeath += makeRewardedEnemyDeath;
        OnExtraDeath += makeExtraDeath;
        OnEndgame += pauseGame;
    }

    void makeRewardedEnemyDeath(Enemy enemy, float reward)
    {
        player.ApplyStrenghtChanges(-reward);
        score++;
        OnScoreChange?.Invoke(score);
    }

    void makeExtraDeath()
    {
        if (player.Health < player.MaxHealth)
        {
            player.ApplyHealthChanges(-(player.MaxHealth / 2));
        }
        else
        {
            player.ApplyStrenghtChanges(-(player.MaxStrenght / 2));
        }
    }

    void pauseGame()
    {
        Time.timeScale = 0;
    }

    void resumeGame()
    {
        Time.timeScale = 1;
    }

    private void OnDisable()
    {
        OnRewardedEnemyDeath -= makeRewardedEnemyDeath;
        OnExtraDeath -= makeExtraDeath;
        OnEndgame -= pauseGame;
    }
}
