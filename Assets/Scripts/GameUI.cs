using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] TMP_Text _killsText;
    [SerializeField] TMP_Text _hpText;
    [SerializeField] TMP_Text _strengthText;
    [SerializeField] TMP_Text _endgameKillsText;

    [SerializeField] GameObject _pauseScreen;
    [SerializeField] GameObject _endgameScreen;


    private void OnEnable()
    {
        GlobalEventManager.OnScoreChange.AddListener(setKills);
        GlobalEventManager.OnHealthChange.AddListener(setHP);
        GlobalEventManager.OnStrenghtChange.AddListener(setStrength);
        GlobalEventManager.OnEndgame.AddListener(activateEndgameScreen);
        GlobalEventManager.OnPlayerInit.AddListener(initPlayerValues);
        GlobalEventManager.OnScoreInit.AddListener(initScore);
    }

    void initPlayerValues(float health, float maxHealth, float strength, float maxStrength)
    {
        setHP(health, maxHealth);
        setStrength(strength, maxStrength);
    }

    void initScore(int score)
    {
        setKills(score);
    }

    void setKills(int score)
    {
        _killsText.text = score.ToString();
    }

    void setHP(float currentHp, float maxHp)
    {
        _hpText.text = currentHp + "/" + maxHp;
    }

    void setStrength(float currentStrength, float maxStrngth)
    {
        _strengthText.text = currentStrength + "/" + maxStrngth;
    }

    void activateEndgameScreen()
    {
        setEndgameKillsText(_killsText.text);
        OpenEndgame();
    }

    void setEndgameKillsText(string text)
    {
        _endgameKillsText.text = text;
    }

    public void CloseScreens()
    {
        _pauseScreen.active = false;
        _endgameScreen.active = false;
    }

    public void OpenPause()
    {
        _pauseScreen.active = true;
    }

    public void OpenEndgame()
    {
        _endgameScreen.active = true;
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnScoreChange.RemoveListener(setKills);
        GlobalEventManager.OnHealthChange.RemoveListener(setHP);
        GlobalEventManager.OnStrenghtChange.RemoveListener(setStrength);
        GlobalEventManager.OnEndgame.RemoveListener(activateEndgameScreen);
        GlobalEventManager.OnPlayerInit.RemoveListener(initPlayerValues);
        GlobalEventManager.OnScoreInit.RemoveListener(initScore);
    }
}
