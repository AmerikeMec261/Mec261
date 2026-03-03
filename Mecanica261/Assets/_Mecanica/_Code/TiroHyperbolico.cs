using UnityEngine;
using UnityEngine.InputSystem;  

public class TiroHyperbolico : MonoBehaviour
{
    [Header("Datos iniciales")]
    public float velocidadInicial = 20f;   
    public float angulo = 45f;            
    public float gravedad = 9.8f;          
    private float v0x;
    private float v0y;
    private float tiempoVuelo;
    private float tiempo;
    private Vector3 posicionInicial;  
    private bool disparando = false;
    private PlayerInput playerInput;  

    void Awake()
    {   
       var espacioAction = new InputAction("Disparar", InputActionType.Button, "<Keyboard>/space");
       espacioAction.performed += _ => IniciarDisparo();
       espacioAction.Enable();
    }
    void Start()
    {
       posicionInicial = transform.position;

        
       float anguloRad = angulo * Mathf.Deg2Rad;


       v0x = velocidadInicial * Mathf.Cos(anguloRad);
       v0y = velocidadInicial * Mathf.Sin(anguloRad);

        
       tiempoVuelo = (2 * v0y) / gravedad;

       Debug.Log("V0x = " + v0x);
       Debug.Log("V0y = " + v0y);
       Debug.Log("Tiempo = " + tiempoVuelo + " s");
    }

    void Update()
    {
        if (!disparando)
        {
           tiempo = 0f;  
           return;
        }

        tiempo += Time.deltaTime;
       
        float x = v0x * tiempo;
        float y = v0y * tiempo - 0.5f * gravedad * tiempo * tiempo;

        transform.position = posicionInicial + new Vector3(x, y, 0);
 
        if (y < 0)
        {
            disparando = false;
        }
    }   
    private void IniciarDisparo()
    {
        if (!disparando)
        {
            disparando = true;
            tiempo = 0f;
            Debug.Log("¡Disparo iniciado con espacio!");
        }
    }
}
