using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{

    [Header("Running")]
    public static KeyCode RunningKey = KeyCode.LeftShift;
    public bool CanRun = true;
    public bool IsRunning { get; private set; }
    public float RunSpeed = 9;

    [SerializeField] private float _speed = 5;
    private Rigidbody _rigidBodyComponent;

    public List<System.Func<float>> SpeedOverrides = new List<System.Func<float>>();

    void Awake()
    {
        _rigidBodyComponent = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float targetMovingSpeed = HandleMovement();
        targetMovingSpeed = CheckSpeedOverrides(targetMovingSpeed);

        HandleVelocity(targetMovingSpeed);
    }

    private float CheckSpeedOverrides(float targetMovingSpeed)
    {
        if (SpeedOverrides.Count > 0)
        {
            targetMovingSpeed = SpeedOverrides[SpeedOverrides.Count - 1]();
        }

        return targetMovingSpeed;
    }

    private void HandleVelocity(float targetMovingSpeed)
    {
        Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

        _rigidBodyComponent.linearVelocity = transform.rotation * new Vector3(targetVelocity.x, _rigidBodyComponent.linearVelocity.y, targetVelocity.y);
    }

    private float HandleMovement()
    {
        IsRunning = CanRun && Input.GetKey(RunningKey);

        float targetMovingSpeed = IsRunning ? RunSpeed : _speed;
        return targetMovingSpeed;
    }
}
