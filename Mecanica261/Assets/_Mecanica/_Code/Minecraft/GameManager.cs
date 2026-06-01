using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using UnityEngine.SceneManagement;

namespace Minecraft
{
    [AddComponentMenu("Minecraft/Game Manager")]
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Death Screen")]
        [Tooltip("Root object shown when the player dies.")]
        [SerializeField, Required] private GameObject _deathScreen;
        [Tooltip("Text element that displays the final score.")]
        [SerializeField, Required] private TextMeshProUGUI _scoreText;
        [Tooltip("Button that restarts the current scene.")]
        [SerializeField, Required] private Button _retryButton;

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

            _deathScreen.SetActive(false);
            _retryButton.onClick.AddListener(Retry);
        }

        private void OnDestroy()
        {
            if (Instance == this) { Instance = null; }

            _retryButton.onClick.RemoveListener(Retry);
        }

        public void AddScore(int score)
        {
            _score += score;
        }

        public void ShowDeathScreen()
        {
            _scoreText.text = "Score: " + _score;
            _deathScreen.SetActive(true);
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
