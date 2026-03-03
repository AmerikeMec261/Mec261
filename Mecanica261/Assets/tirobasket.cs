using UnityEngine;
using UnityEngine.InputSystem;

public class tirobasket : MonoBehaviour
{
    [Header("Datos iniciales")]
    public float velocidadInicial = 12f;
    public float angulo = 45f;    

    [Header("Configuraci�n Flotado")]
    public float amplitud = 0.5f;
    public float velocidadflotado = 2f;

    private Rigidbody rb;
    private Vector3 posicionInicial;
    private bool lanzado = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        posicionInicial = transform.position;
        
        rb.isKinematic = true;
    }

    void Update()
    {
        if (!lanzado)
        {            
            float nuevoY = posicionInicial.y + Mathf.Sin(Time.time * velocidadflotado) * amplitud;
            transform.position = new Vector3(posicionInicial.x, nuevoY, posicionInicial.z);
            
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Lanzar();
            }
        }
        else
        {            
            if (transform.position.y < -5f)
            {
                Reiniciar();
            }
        }
    }

    void Lanzar()
    {
        lanzado = true;
        
        rb.isKinematic = false;
        rb.useGravity = true;
       
        float anguloRad = angulo * Mathf.Deg2Rad;
        float v0x = 0; 
        float v0y = velocidadInicial * Mathf.Sin(anguloRad);
        float v0z = velocidadInicial * Mathf.Cos(anguloRad);
       
        rb.linearVelocity = new Vector3(v0x, v0y, v0z);
    }

    void Reiniciar()
    {
        lanzado = false;
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = posicionInicial;
        transform.rotation = Quaternion.identity;
    }
}
