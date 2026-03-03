using UnityEngine;

public class CestaController : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;     // velocidad lenta y constante
    [SerializeField] private float range = 2.0f;     // cuánto se mueve (de derecha a izquierda)

    private Vector3 startPosition;
    private bool goingRight = true;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float step = speed * Time.deltaTime;

        if (goingRight)
        {
            // se mueve de izquierda a derecha
            transform.position += Vector3.right * step;

            // cuando llegue a la derecha del rango, empieza a volver un poco
            if (transform.position.x >= startPosition.x + range)
            {
                goingRight = false;
            }
        }
        else
        {
            // se mueve un poco hacia la izquierda (solo un pequeño pedazo)
            transform.position += Vector3.left * step;

            // si se ha movido lo suficiente hacia la izquierda, vuelve a ir a la derecha
            float minX = startPosition.x; // solo vuelve hasta la posición inicial
            if (transform.position.x <= minX)
            {
                goingRight = true;
            }
        }
    }
}

