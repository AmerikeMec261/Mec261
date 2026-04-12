using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int money = 100;
    public int lives = 10;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI livesText;
    public GameObject enemyPrefab;
    public Transform spawnPoint;

    private float spawnTimer = 0f;
    public float spwanInterval = 2f;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
        
    }

    void Start()
    {
        UpdateUI();

    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if(spawnTimer >= spwanInterval)
        {
            SpawnEnemy();
            spawnTimer= 0f;
        }

        if(Keyboard.current!= null && Keyboard.current.mKey.wasPressedThisFrame)
        {
            money += 10;
            UpdateUI();
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateUI();
    }

    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            UpdateUI();
            return true;
        }

        return false;
    }

    public void LoseLife(int amount)
    {
        lives -= amount;
        UpdateUI();
        if (lives <= 0)
        {
            Debug.Log("Ya valiste");
            Time.timeScale = 0f;
        }
    }

    void UpdateUI()
    {
        moneyText.text = "$" + money;
        livesText.text = "Vidas" + lives;
    }
}
