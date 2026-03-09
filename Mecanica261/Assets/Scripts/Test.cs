using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int score = 0;
    public static bool isPaused;
    public static bool isDead;

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
        ProperGame();
        ProperCanvas();
    }

    private void ProperCanvas()
    {
        _tutorialCanvas.gameObject.SetActive(true);
        _gameOverCanvas.gameObject.SetActive(false);
        _pauseButton.gameObject.SetActive(false);
    }

    private void ProperGame()
    {
        score = 0;
        isPaused = false;
        isDead = false;
        Paused();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInPause();
    }

    private void CheckInPause()
    {
        if (Input.GetKeyDown("p"))
        {
            Paused();
        }
        checkDead();
    }

    public void Paused()
    {
        isPaused = !isPaused;
        _player.GetComponent<Rigidbody>();
        _pauseButton.gameObject.SetActive(true);
        if (isPaused)
        {
            float noSpeed=0f;
            HandleAnimatorSpeed(noSpeed);
        }
        else
        {
            float fullSpeed = 1f;
            HandleAnimatorSpeed(fullSpeed);
        }

    }

    private void HandleAnimatorSpeed(float animatopspeed)
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
        float noSpeed = 0f;
        HandleAnimatorSpeed(noSpeed);
        float fullSpeed = 1f;
        HandleAnimatorSpeed(fullSpeed);
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
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SaveScore();
        SceneManager.LoadScene(0);
    }

    public void SaveScore()
    {
        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
        PlayerPrefs.Save();
    }

    public void checkDead()
    {
        if (isDead)
        {
            GameOver();
        }
    }
}