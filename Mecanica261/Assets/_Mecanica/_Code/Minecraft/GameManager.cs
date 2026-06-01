using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Minecraft
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Death Screen")]
        [SerializeField] private GameObject _deathScreen;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Button _retryButton;

        private int _score;

        public int Score { get { return _score; } }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            if (_deathScreen != null) { _deathScreen.SetActive(false); }
            if (_retryButton != null) { _retryButton.onClick.AddListener(Retry); }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }

            if (_retryButton != null)
            {
                _retryButton.onClick.RemoveListener(Retry);
            }
        }

        public void AddScore(int score)
        {
            _score += score;
        }

        public void ShowDeathScreen()
        {
            if (_scoreText != null)
            {
                _scoreText.text = "Score: " + _score;
            }

            if (_deathScreen != null)
            {
                _deathScreen.SetActive(true);
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void Retry()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }
    }
}
