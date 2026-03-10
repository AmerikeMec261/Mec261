using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using static UnityEditor.Experimental.GraphView.GraphView;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static int Score = 0;
    public static bool IsPaused;
    public static bool IsDead;

    public static int mainMenuIndex = 0;
    public static int mainGameplayIndex = 1;

    [SerializeField] private GameObject _Player;
    [SerializeField] private Canvas _TutorialCanvas;
    [SerializeField] private Canvas _GameOverCanvas;
    [SerializeField] private Canvas _ScoreCanvas;
    [SerializeField] private Button _PauseButton;
    [SerializeField] private GameObject _Ground;

    public void Awake()
    {
        startScores();
    }
    void Start()
    {
        Score = 0;
        IsPaused = false;
        IsDead = false;
        Paused();
        _TutorialCanvas.gameObject.SetActive(true);
        _GameOverCanvas.gameObject.SetActive(false);
        _PauseButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            Paused();
        }
        checkDead();
    }

    public void Paused()
    {
        CheckIfGameIsPause();

    }

    private void CheckIfGameIsPause()
    {
        IsPaused = !IsPaused;
        //*Player.GetComponent<Player>().Pause();
        _PauseButton.gameObject.SetActive(true);
        if (IsPaused)
        {
            float noSpeed = 0;
            HandleAnimatorSpeed(noSpeed);
        }
        else
        {
            float fullSpeed = 1;
            HandleAnimatorSpeed(1);
        }
    }
    private void HandleAnimatorSpeed()
    {
        _Ground.GetComponent<Animator>().speed = 1;
        _Player.GetComponent<Animator>().speed = 1;
    }
    private void HandleAnimatorSpeed(float animatorSpeed)
    {
        _Ground.GetComponent<Animator>().speed = 0;
        _Player.GetComponent<Animator>().speed = 0;
    }

    public void StopTutorial()
    {
        _TutorialCanvas.gameObject.SetActive(false);
        Paused();
    }

    public void Tutorial()
    {
        _TutorialCanvas.gameObject.SetActive(false);
        Paused();
    }

    public void GameOver()
    {
        _GameOverCanvas.gameObject.SetActive(true);
        _ScoreCanvas.gameObject.SetActive(false);
        _Ground.GetComponent<Animator>().speed = 0;
        _Player.GetComponent<Animator>().speed = 0;
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