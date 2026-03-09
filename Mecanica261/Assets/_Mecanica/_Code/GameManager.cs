using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Objetos")]
    [SerializeField] public TiroParabolico shoot;
    [SerializeField] public InputField angle;
    [SerializeField] public InputField Velocity;
    [SerializeField] public GameObject RestartsUI;

    [Header("Valores")]
    [SerializeField] public int MaxShoots = 3;
    [SerializeField] public float RotationVelocity = 50f;

    private int WastedShoots = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RestartsUI.SetActive(false);
        angle.text = "45";
        Velocity.text = "20";

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
            shoot.transform.Rotate(Vector3.up * -RotationVelocity * Time.deltaTime);
        if (Input.GetKey(KeyCode.D))
            shoot.transform.Rotate(Vector3.up * RotationVelocity * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Space) && WastedShoots < MaxShoots)
            Shoot();
    }
    void Shoot()
    {
        float ang = float.Parse(angle.text);
        float vel = float.Parse(Velocity.text);

        shoot.Parameters(vel, ang);
        shoot.Shooting();

        WastedShoots++;
        if(WastedShoots >= MaxShoots )
            RestartsUI?.SetActive(true);
    
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        WastedShoots = 0;
        shoot.Reinicio();
        RestartsUI.SetActive(false);
        angle.text = "45";
        Velocity.text = "20";
    }
}
