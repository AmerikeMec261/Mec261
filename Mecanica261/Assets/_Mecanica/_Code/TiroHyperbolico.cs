using UnityEngine;
using UnityEngine.InputSystem;  

public class TiroHyperbolico : MonoBehaviour
{
    [Header("Datos iniciales")]
    public float initialVelocity = 20f;   
    public float angle = 45f;            
    public float gravity = 9.8f;          
    private float v0x;
    private float v0y;
    private float _flyingTime;
    private float _time;
    private Vector3 _startPosition;  
    private bool _isShooting = false;
    private PlayerInput _playerInput;  

    void Awake()
    {   
       var espacioAction = new InputAction("Disparar", InputActionType.Button, "<Keyboard>/space");
       espacioAction.performed += _ => IniciarDisparo();
       espacioAction.Enable();
    }
    void Start()
    {
       _startPosition = transform.position;

        
       float anguloRad = angle * Mathf.Deg2Rad;


       v0x = initialVelocity * Mathf.Cos(anguloRad);
       v0y = initialVelocity * Mathf.Sin(anguloRad);

        
       _flyingTime = (2 * v0y) / gravity;

       Debug.Log("V0x = " + v0x);
       Debug.Log("V0y = " + v0y);
       Debug.Log("Tiempo = " + _flyingTime + " s");
    }

    void Update()
    {
        if (!_isShooting)
        {
           _time = 0f;  
           return;
        }

        _time += Time.deltaTime;
       
        float x = v0x * _time;
        float y = v0y * _time - 0.5f * gravity * _time * _time;

        transform.position = _startPosition + new Vector3(x, y, 0);
 
        if (y < 0)
        {
            _isShooting = false;
        }
    }   
    private void IniciarDisparo()
    {
        if (!_isShooting)
        {
            _isShooting = true;
            _time = 0f;
            Debug.Log("¡Disparo iniciado con espacio!");
        }
    }
}
