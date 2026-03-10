using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Manager : MonoBehaviour
{
    public static int score = 0;
    public static bool isPaused;
    public static bool isDead;




    [SerializeField] private GameObject _player;
    [SerializeField] private Canvas _canvasTutorial;
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
        _canvasTutorial.gameObject.SetActive(true);
        _gameOverCanvas.gameObject.SetActive(false);
        _pauseButton.gameObject.SetActive(false);
    }

    private void PrepareGame()
    {
        score = 0;
        isPaused = false;
        isDead = false;
        Paused();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIPause();
    }

    private void CheckIPause()
    {
        if (Input.GetKeyDown("P"))
        {
            Paused();
        }
        CheckDead();
    }

    public void Paused()
    {
        isPaused = !isPaused;
        _player.GetComponent<Rigidbody>();
        _pauseButton.gameObject.SetActive(true);
        if (isPaused)
        {
            float noSpeed = 0f;
            HandleAnimatorSpeed();
        }
        else
        {
            float fullSpeed = 0f;
            HandleAnimatorSpeed();
        }

    }

    private void HandleAnimatorSpeed()
    {
        _ground.GetComponent<Animator>().speed = 0;
        _player.GetComponent<Animator>().speed = 0;
    }

    public void StopTutorial()
    {
        _canvasTutorial.gameObject.SetActive(false);
        Paused();
    }

    public void GameOver()
    {
        _gameOverCanvas.gameObject.SetActive(true);
        _scoreCanvas.gameObject.SetActive(false);
        float noSpeed = 0f;
        HandleAnimatorSpeed();
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

    public void CheckDead()
    {
        if (isDead)
        {
            GameOver();
        }
    }

}
