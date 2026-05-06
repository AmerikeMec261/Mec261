using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [Header("Propultion")]
    [Tooltip("Fuerza de empuje hacía delante y atras en Newtons 'N'")]
    public float thrustForce = 5000f;

    [Tooltip("Velocidad maxima en m/s")]
    public float maxSpeed = 10f;

    [Header("Turn")]
    [Tooltip("Torque para girar el barco")]
    // Multiplica la masa del RB x 5
    public float turnTorque = 3000f;

    [Tooltip("El barco puede girar si se encuentra sobre esta velocidad" + "Un barco no puede girar en una velocidad 0")]
    public float minSpeedTurn = 0.5f;

    [Header("Water Configuration")]
    [Tooltip("El barco solo se podra mover si esta en el agua")]
    public bool requireWater = true;
    public float waterLevel = 0;

    private Rigidbody rb;

    private float thrustInput;
    private float turnInput;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        thrustInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        Thrust();
        Turn();
    }

    void Thrust()
    {
        // rb.linearVelocity.magnitude es la rapidez actual en m/s
        if (rb.linearVelocity.magnitude < maxSpeed)
        {
            Vector3 force = transform.forward * thrustInput * thrustForce;
            rb.AddForce(force, ForceMode.Force);
        }
    }

    void Turn()
    {
        // El barco solo gira si se mueve
        float currentSpeed = rb.linearVelocity.magnitude;

        // El torque se aplica sobre el eje Y (arriba) para girar horizontalmente.
        // SpeedFactor hace que el giro sea más pronunciado, cuanto más rápido va el barco
        float speedFactor = Mathf.Clamp01(currentSpeed / 3f);
        rb.AddTorque(Vector3.up * turnInput * 2f, ForceMode.VelocityChange);
    }
}
