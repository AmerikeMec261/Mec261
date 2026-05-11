using UnityEngine;
using UnityEngine.InputSystem;

public class ShipControler : MonoBehaviour
{
    [SerializeField] private float _Maxfoward = 0f;
    [SerializeField] private float _Maxreverse = 1f;
    [SerializeField] private float _Forwardmovement;
    [SerializeField] private float _Reversemovement;

    public Transform Motor;
    private Rigidbody _Rigidbody;

    [SerializeField] public float SteerPower = 500f;
    [SerializeField] public float Power = 5f;
    [SerializeField] public float MaxSpeed = 10f;
    [SerializeField] public float Drag = 0.1f;

    [SerializeField] private Quaternion StartRotation;

    public void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody>();
        if (Motor != null) StartRotation = Motor.localRotation;
    }

    void FixedUpdate()
    {
        var keyboard = Keyboard.current;

        Vector3 forwardFix = transform.right;
        Vector3 steerDirection = transform.forward;

        float steer = 0f;

        if (keyboard.aKey.isPressed) steer = 1f;

        if (keyboard.dKey.isPressed) steer = -1f;

        if (Motor != null)
        {
            _Rigidbody.AddForceAtPosition(steer * steerDirection * SteerPower / 100f, Motor.position);
        }
        
        if (keyboard.wKey.isPressed)
        {
            _Rigidbody.AddForce(forwardFix * Power, ForceMode.Acceleration);
        }

        if (keyboard.sKey.isPressed)
        {
            _Rigidbody.AddForce(-forwardFix * Power, ForceMode.Acceleration);
        }
    }
}
