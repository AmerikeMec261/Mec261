using UnityEngine;
using UnityEngine.InputSystem;

public class ShipControler : MonoBehaviour
{
    public Transform Motor;
    public float SteerPower = 500f;
    public float Power = 5f;
    public float MaxSpeed = 10f;
    public float Drag = 0.1f;

    protected Rigidbody Rigidbody;
    protected Quaternion StartRotation;

    public void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        if (Motor != null) StartRotation = Motor.localRotation;
    }

    void FixedUpdate()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        var steer = 0f;

        if (keyboard.aKey.isPressed)
        {
            steer = 1;
        }

        if (keyboard.dKey.isPressed)
        {
            steer = -1;
        }

        Rigidbody.AddForceAtPosition(steer * transform.right * SteerPower / 100f, Motor.position);

        var forward = Vector3.Scale(new Vector3(1,0,1), transform.forward);
        
        if (keyboard.wKey.isPressed)
        {
            Rigidbody.AddForce(forward * Power, ForceMode.Acceleration);
        }

        if (keyboard.sKey.isPressed)
        {
            Rigidbody.AddForce(-forward * Power, ForceMode.Acceleration);
        }

        if (Rigidbody.linearVelocity.magnitude > MaxSpeed)
        {
            Rigidbody.linearVelocity = Rigidbody.linearVelocity.normalized * MaxSpeed;
        }
    }
}
