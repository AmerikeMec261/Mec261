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


    [SerializeField] private GameObject _Player;
    [SerializeField] private Canvas _TutorialCanvas;
    [SerializeField] private Canvas _GameOverCanvas;
    [SerializeField] private Canvas _ScoreCanvas;
    [SerializeField] private Button _PauseButton;
    [SerializeField] private GameObject _Ground;

    public void Awake()
    {
        StartScores();
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

    void Update()
    {
        Pause();
    }

    public void Pause()
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
        //_Player.GetComponent<Rigidbody>;
        _PauseButton.gameObject.SetActive(true);


        if (IsPaused)
        {
            _Ground.GetComponent<Animator>().speed = 0;
            _Player.GetComponent<Animator>().speed = 0;
        }
        else
        {
            _Ground.GetComponent<Animator>().speed = 1;
            _Player.GetComponent<Animator>().speed = 1;
        }

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
        MagicNumbers();
        SaveScore();
    }

    private void MagicNumbers()
    {
        _Ground.GetComponent<Animator>().speed = 0;
        _Player.GetComponent<Animator>().speed = 0;
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
        if (IsDead)
        {
            GameOver();
        }
    }
}