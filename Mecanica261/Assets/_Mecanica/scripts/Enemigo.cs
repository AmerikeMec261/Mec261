using UnityEngine;

public class Enemigo : MonoBehaviour, IRecibeImpacto
{
    [Header("Stats")]
    [SerializeField] private float vida = 100f;
    [SerializeField] private float escudo = 0f;
    [SerializeField] private float velocidad = 2f;

    [Header("Movimiento")]
    [SerializeField] private Transform puntoA;
    [SerializeField] private Transform puntoB;

    private Transform objetivoActual;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        objetivoActual = puntoB;
    }

    private void FixedUpdate()
    {
        Vector3 destino = new Vector3(
            objetivoActual.position.x,
            rb.position.y,
            objetivoActual.position.z
        );

        Vector3 nuevaPosicion = Vector3.MoveTowards(
            rb.position,
            destino,
            velocidad * Time.fixedDeltaTime
        );

        rb.MovePosition(nuevaPosicion);

        if (Vector3.Distance(rb.position, destino) < 0.2f)
        {
            if (objetivoActual == puntoA)
            {
                objetivoActual = puntoB;
            }
            else
            {
                objetivoActual = puntoA;
            }
        }
    }

    public void RecibirImpacto(float impacto)
    {
        if (escudo > 0f)
        {
            escudo -= impacto;

            if (escudo < 0f)
            {
                vida += escudo;
                escudo = 0f;
            }
        }
        else
        {
            vida -= impacto;
        }

        Debug.Log(gameObject.name + " vida restante: " + vida);

        if (vida <= 0f)
        {
            Destroy(gameObject);
        }
    }
}