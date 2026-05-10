using UnityEngine;

public class ShipMovement : MonoBehaviour //Estandarizar código
{
    [Header("Ship Speed")]    
    [SerializeField] private float motorFroce = 100f;
    [SerializeField] private float multiplier = 100f;
    [SerializeField] private float rudderFroce = 50000f;

    [Header("Ship Configuration")]
    [SerializeField] private Transform motor;
    [SerializeField] private Transform rudder;

    private Rigidbody rigidBody;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    
    void FixedUpdate() //El input se debe manejar en el Update, pero la física se debe manejar en el FixedUpdate, para evitar problemas de rendimiento y de sincronización.
    {
        MotorShip();
        RudderShip();
    }

    private void MotorShip()
    {
        if (Input.GetKeyDown(KeyCode.W)) //Tus fuerzas se aplican solo cuando presionas la tecla, lo que no es ideal para un movimiento suave. Deberías usar Input.GetKey en lugar de Input.GetKeyDown para aplicar la fuerza mientras la tecla esté presionada.
        {
            Vector3 force = motor.forward * motorFroce * multiplier * Time.deltaTime;
            rigidBody.AddForceAtPosition(force, motor.position, ForceMode.Force);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Vector3 force = - motor.forward * motorFroce * multiplier * Time.deltaTime;
            rigidBody.AddForceAtPosition(force, motor.position, ForceMode.Force);
        }
    }

    private void RudderShip()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Vector3 force = rudder.right * rudderFroce * multiplier * Time.deltaTime;
            rigidBody.AddForceAtPosition(force, rudder.position, ForceMode.Force);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Vector3 force = -rudder.right * rudderFroce * multiplier * Time.deltaTime;
            rigidBody.AddForceAtPosition(force, rudder.position, ForceMode.Force);
        }
    }
}
