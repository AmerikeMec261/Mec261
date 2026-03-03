using UnityEngine;
using UnityEngine.InputSystem;


public class bala : MonoBehaviour
{
    [Header("Datos iniciales")]
    public float velocidadInicial = 20f;   // m/s
    public float angulo = 45f;             // grados
    public float gravedad = 9.8f;          // m/s^2

    private float v0x;
    private float v0y;
    private float tiempoVuelo;
    private float velocidadMovimiento = 5f;

    private float tiempo;
    private Vector3 posicionInicial;

    private bool disparado = false;

    void Update()
    {
        if (!disparado)
        {
            float movimiento = 0;
            if (Keyboard.current.aKey.isPressed) movimiento = -1;
            else if (Keyboard.current.dKey.isPressed) movimiento = 1;

            transform.Translate(Vector3.right * movimiento * velocidadMovimiento * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.E) && !disparado)
        {
            posicionInicial = transform.position;
            tiempo = 0;

            float anguloRad = angulo * Mathf.Deg2Rad;

            v0x = velocidadInicial * Mathf.Cos(anguloRad);
            v0y = velocidadInicial * Mathf.Sin(anguloRad);

            disparado = true;
        }



        if (disparado)
        {
            tiempo += Time.deltaTime;

            float x = v0x * tiempo;
            float y = v0y * tiempo - 0.5f * gravedad * tiempo * tiempo;
            transform.position = posicionInicial + new Vector3(x, y, 0);
        }
    }
}
