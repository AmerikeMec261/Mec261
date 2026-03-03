using UnityEngine;
using UnityEngine.InputSystem;

public class MovimientoPlayer : MonoBehaviour
{
    [Header("Configuración del Salto")]
    public float velocidadSalto = 20f;
    public float angulo = 45f;
    public float gravedad = 30f;

    [Header("Movimiento Automático")]
    public float velocidadMovimiento = 10f;

    private float v0x;
    private float v0y;
    private float tiempoSalto;
    private bool estaSaltando = false;
    private bool enSuelo = true;

    private Vector3 posicionSaltoInicio;
    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        if (enSuelo && !estaSaltando)
        {
        transform.Translate(Vector3.right * velocidadMovimiento * Time.deltaTime);
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && !estaSaltando)
        {
            Saltar();
        }

        if (estaSaltando)
        {
            AplicarFisicaSalto();
        }
    }

    void Saltar()
    {
        estaSaltando = true;
        enSuelo = false;
        tiempoSalto = 0f;
        posicionSaltoInicio = transform.position;

        float anguloRad = angulo * Mathf.Deg2Rad;
        v0x = velocidadSalto * Mathf.Cos(anguloRad);
        v0y = velocidadSalto * Mathf.Sin(anguloRad);
    }

    void AplicarFisicaSalto()
    {
        tiempoSalto += Time.deltaTime;

        float x = v0x * tiempoSalto;
        float y = v0y * tiempoSalto - 0.5f * gravedad * (tiempoSalto * tiempoSalto);

        transform.position = new Vector3(posicionSaltoInicio.x + x, posicionSaltoInicio.y + y, transform.position.z);

        if (y <= 0)
        {
            estaSaltando = false;
            transform.position = new Vector3(transform.position.x, posicionSaltoInicio.y, transform.position.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("trampa"))
        {
            estaSaltando = false;
            enSuelo = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("trampa"))
        {
            ReiniciarNivel();
        }
    }

    void ReiniciarNivel()
    {
        transform.position = posicionInicial;
        estaSaltando = false;
        enSuelo = true;
        tiempoSalto = 0f;
    }
}