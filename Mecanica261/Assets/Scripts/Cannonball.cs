using UnityEngine;

public class Cannonball : MonoBehaviour
{
    [Header("Datos iniciales")]
    public float velocidadInicial = 20f;   // m/s
    public float angulo = 45f;             // grados
    public float gravedad = 9.8f;          // m/s^2

    private float v0x;
    private float v0y;
    private float tiempoVuelo;

    private float tiempo;
    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;

        // Convertir ángulo a radianes
        float anguloRad = angulo * Mathf.Deg2Rad;

        // Calcular velocidades
        v0x = velocidadInicial * Mathf.Cos(anguloRad);
        v0y = velocidadInicial * Mathf.Sin(anguloRad);

        // Calcular tiempo total de vuelo
        tiempoVuelo = (2 * v0y) / gravedad;

        Debug.Log("V0x = " + v0x);
        Debug.Log("V0y = " + v0y);
        Debug.Log("Tiempo total = " + tiempoVuelo + " s");
    }

    void Update()
    {
        tiempo += Time.deltaTime;

        // Ecuaciones del movimiento
        float x = v0x * tiempo;
        float y = v0y * tiempo - 0.5f * gravedad * tiempo * tiempo;

        transform.position = posicionInicial + new Vector3(x, y, 0);

        // Si cae al suelo, detener
        if (y < 0)
        {
            enabled = false;
        }
    }
}
