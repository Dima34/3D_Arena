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
    [SerializeField] GameObject _ultraButton;
    [SerializeField] Player _player;
    [SerializeField] GameManager _gameManager;

    PlayerUltra playerUltra;


    private void Awake()
    {
        _gameManager.OnScoreChange += setKills;
        _gameManager.OnEndgame += activateEndgameScreen;
        _gameManager.OnScoreInit += initScore;
        _player.OnHealthChange += setHP;
        _player.OnStrenghtChange += setStrength;
        _player.OnPlayerInit += initPlayerValues;

        playerUltra = _player.GetComponent<PlayerUltra>();
        playerUltra.OnUltraStateChanged += toggleUltraButton;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            OpenPause();
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }
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

    void toggleUltraButton(bool ultraState)
    {
        if (ultraState)
        {
            _ultraButton.active = true;
        }
        else
        {
            _ultraButton.active = false;
        }
    }

    private void OnDestroy()
    {
        _gameManager.OnScoreChange -= setKills;
        _gameManager.OnEndgame -= activateEndgameScreen;
        _gameManager.OnScoreInit -= initScore;
        _player.OnHealthChange -= setHP;
        _player.OnStrenghtChange -= setStrength;
        _player.OnPlayerInit -= initPlayerValues;
        playerUltra.OnUltraStateChanged -= toggleUltraButton;
    }
}
