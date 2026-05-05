using UnityEngine;

public class barcomoverse : MonoBehaviour
{
    [Header("Puntos")]
    [SerializeField] private Transform[] motorPoints;
    [SerializeField] private Transform[] turnPoints;

    [Header("Fuerzas")]
    [SerializeField] private float motorForce = 50f;
    [SerializeField] private float turnForce = 20f;

    private Rigidbody rb;

    private float velocidadActual = 0f;
    private float giroActual = 0f;

    [SerializeField] private float aceleracion = 2f;
    [SerializeField] private float suavizadoGiro = 2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float inputForward = Input.GetAxis("Vertical");
        float inputTurn = Input.GetAxis("Horizontal");

        velocidadActual = Mathf.Lerp(velocidadActual, inputForward, Time.fixedDeltaTime * aceleracion);
        giroActual = Mathf.Lerp(giroActual, inputTurn, Time.fixedDeltaTime * suavizadoGiro);

        for (int i = 0; i < motorPoints.Length; i++)
        {
            Vector3 fuerza = transform.forward * velocidadActual * motorForce;
            rb.AddForceAtPosition(fuerza, motorPoints[i].position);
        }

        for (int i = 0; i < turnPoints.Length; i++)
        {
            Vector3 fuerza = transform.right * giroActual * turnForce;
            rb.AddForceAtPosition(fuerza, turnPoints[i].position);
        }
    }
}
