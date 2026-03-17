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

    private static int MainMenuIndex = 0;
    private static int GameplayIndex = 1;

    [SerializeField] private GameObject _player;
    [SerializeField] private Canvas _tutorialCanvas;
    [SerializeField] private Canvas _gameOverCanvas;
    [SerializeField] private Canvas _scoreCanvas;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private GameObject _ground;

    public void Awake()
    {
        StartScores();
    }

    void Start()
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


    void Update()
    {
        CheckIfGameIsPaused();
    }

    private void CheckIfGameIsPaused()
    {
        if (Input.GetKeyDown("p"))
        {
            Paused();
        }
        CheckDead();
    }

    public void Paused()
    {
        IsPaused = !IsPaused;
        _player.GetComponent<Rigidbody>();
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

    private void HandleAnimatorSpeed(float animatorSpeed)
    {
        _ground.GetComponent<Animator>().speed = animatorSpeed;
        _player.GetComponent<Animator>().speed = animatorSpeed;
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
        SaveScore();
    }

    private void StartScores()
    {
        if (!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }

        PlayerPrefs.Save();
    }

    public void Restart()
    {
        SaveScore();
        SceneManager.LoadScene(GameplayIndex);
    }

    public void MainMenu()
    {
        SaveScore();
        SceneManager.LoadScene(MainMenuIndex);
    }

    public void SaveScore()
    {
        if (Score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", Score);
        }
        PlayerPrefs.Save();
    }

    public void CheckDead()
    {
        if (IsDead)
        {
            GameOver();
        }
    }
}
