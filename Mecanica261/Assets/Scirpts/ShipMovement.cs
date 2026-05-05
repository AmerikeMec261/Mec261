using UnityEngine;

public class ShipMovement : MonoBehaviour
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
    
    void FixedUpdate()
    {
        MotorShip();
        RudderShip();
    }

    private void MotorShip()
    {
        if (Input.GetKeyDown(KeyCode.W))
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
