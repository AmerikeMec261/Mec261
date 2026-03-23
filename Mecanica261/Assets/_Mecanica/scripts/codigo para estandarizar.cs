
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int Score = 0;
    public static bool IsPaused;
    public static bool IsDead;
    [SerializeField] private GameObject _player;
    [SerializeField] private Canvas _tutorialCanvas;
    [SerializeField] private Canvas _gameOverCanvas;
    [SerializeField] private Canvas _scoreCanvas;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private GameObject _ground;

    public void Awake()
    {
        startScores();
    }
    void start()
    {
        PrepareGame();
        PrepareCanvas();
    }

    private void PrepareCanvas()
    {
        _tutorialCanvas.gameObject.SetActive(true);
        _gameOverCanvas.gameObject.SetActive(false);
        _pauseButton.gameObject.SetActive(false);
    }

    private void PrepareGame()
    {
        Score = 0;
        IsPaused = false;
        IsDead = false;
        Paused();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPause();
    }

    private void CheckPause()
    {
        if (Input.GetKeyDown("p"))
        {
            Paused();
        }
        checkDead();
    }

    public void Paused()
    {
        IsPaused = !IsPaused;
       // _player.GetComponent<player>().Pause();
        _pauseButton.gameObject.SetActive(true);
        if (IsPaused)
        {
            float noSpeed = 0;  
            HandleAnimatorSpeed(noSpeed);
        }
        else
        {
            float fullSpeed = 1;
            HandleAnimatorSpeed(fullSpeed);
        }

    }

    private void HandleAnimatorSpeed(float animatorspeed)
    {
        _ground.GetComponent<Animator>().speed = 0;
        _player.GetComponent<Animator>().speed = 0;
    }

    public void StopTutorial()
    {
        _tutorialCanvas.gameObject.SetActive(false);
        Paused();
    }

    public void GameOver()
    {
        _gameOverCanvas.gameObject.SetActive(true);
        _scoreCanvas.gameObject.SetActive(false);
        float noSpeed = 0;
        HandleAnimatorSpeed(noSpeed);
        float fullSpeed = 1;
        HandleAnimatorSpeed(fullSpeed);
        saveScore();
    }

    private void startScores()
    {
        if (!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }

        PlayerPrefs.Save();
    }

    public void restart()
    {
        saveScore();
        SceneManager.LoadScene(1);
    }

    public void mainMenu()
    {
        saveScore();
        SceneManager.LoadScene(0);
    }

    public void saveScore()
    {
        if (Score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", Score);
        }
        PlayerPrefs.Save();
    }

    public void checkDead()
    {
        if (IsDead)
        {
            GameOver();
        }
    }
}

