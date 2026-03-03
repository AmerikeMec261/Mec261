using UnityEngine;

public class AroMovimiento : MonoBehaviour
{
    public float velocidad = 2f;
    public float distancia = 3f;

    private Vector3 posicionInicial;
    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        float movimiento = Mathf.Sin(Time.time * velocidad) * distancia;

        transform.position = new Vector3(
            posicionInicial.x + movimiento, posicionInicial.y, posicionInicial.z);
    }
}

