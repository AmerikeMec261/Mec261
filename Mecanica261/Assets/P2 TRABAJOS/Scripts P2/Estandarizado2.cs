using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] public TiroParabolico _shoot;
    [SerializeField] public InputField _angle;
    [SerializeField] public InputField _velocity;
    [SerializeField] public GameObject _restartsUI;

    [Header("Settings")]
    [SerializeField] public int _maxShoots = 3;
    [SerializeField] public float _rotationVelocity = 50f;

    private int _wastedShoots = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _restartsUI.SetActive(false);
        _angle.text = "45";
        _velocity.text = "20";

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.A))
        {
            _shoot.transform.Rotate(Vector3.up * -_rotationVelocity * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            _shoot.transform.Rotate(Vector3.up * _rotationVelocity * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space) && _wastedShoots < _maxShoots)
        {
            Shoot();
        }
    }
    void Shoot()
    {
        float ang = float.Parse(_angle.text);
        float vel = float.Parse(_velocity.text);

        _shoot.Parameters(vel, ang);
        _shoot.Shooting();

        _wastedShoots++;

        if(_wastedShoots >= _maxShoots )
        {
            _restartsUI?.SetActive(true);

        } 
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        _wastedShoots = 0;
        _shoot.Reinicio();
        _restartsUI.SetActive(false);
        _angle.text = "45";
        _velocity.text = "20";
    }
}
