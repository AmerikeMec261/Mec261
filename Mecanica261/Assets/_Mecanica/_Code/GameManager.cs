using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int Score = 0;
    public static bool isPaused;
    public static bool isDead;

    public static int mainMenuIndex = 0;
    public static int mainGameplayIndex = 1;

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
        Score = 0;
        isPaused = false;
        isDead = false;
        Paused();
        _canvasTutorial.gameObject.SetActive(true);
        _gameOverCanvas.gameObject.SetActive(false);
        _pauseButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            Paused();
        }
        CheckDead();
    }

    public void Paused()
    {
        CheckIfGameIsPause();

    }

    private void CheckIfGameIsPause()
    {
        isPaused = !isPaused;
        //*Player.GetComponent<Player>().Pause();
        _pauseButton.gameObject.SetActive(true);
        if (isPaused)
        {
            //* float noSpeed = 0;
            HandleAnimatorSpeed(0);
        }
        else
        {
            //* float fullSpeed = 1;
            HandleAnimatorSpeed(1);
        }
    }

    private void HandleAnimatorSpeed()
    {
        _ground.GetComponent<Animator>().speed = 1;
        _player.GetComponent<Animator>().speed = 1;
    }

    private void HandleAnimatorSpeed(float animatorSpeed)
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
        HandleAnimatorSpeed(0);
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
        if (Score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", Score);
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
