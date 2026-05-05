using System.Runtime.CompilerServices;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class ShipPropulsion : MonoBehaviour
{
    [Header("Force Points")]
    [SerializeField] private Transform _propulsorPoint;
    [SerializeField] private Transform _rudderPoint;

    [Header("Motor")]
    [SerializeField] private float _maxSpeed = 15.95f;
    [SerializeField] private float _timeToMaxSpeed = 25f;
    [SerializeField] private float _throttleDecay = 1f;
    [SerializeField] private float _reverseSpeed = 0.3f;

    [Header("Rudder")]
    [SerializeField] private float _maxRudderForce = 5000f;
    [SerializeField] private float _rudderTimeToMax = 25f;
    [SerializeField] private float _rudderDecay = 1.5f;
    
    private Rigidbody _rigidbody;
    
    private float _rudder;
    private float _throttle;
    private float _engineForce;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _engineForce = _rigidbody.mass * (_maxSpeed / _timeToMaxSpeed) * 1.3f;
    }

    private void FixedUpdate()
    {
        UpdateInputs();
        ApplyPropulsion();
        ApplyRudder();
    }

    private void UpdateInputs()
    {
        float throttleInput = Input.GetKey(KeyCode.W) ? 1f : Input.GetKey(KeyCode.S) ? -_reverseSpeed : 0f;
        float rudderInput = Input.GetKey(KeyCode.D) ? 1f : Input.GetKey(KeyCode.A) ? -1f : 0f;

        float throttleRate=!Mathf.Approximately(throttleInput,0f)?1f/_timeToMaxSpeed:_throttleDecay;
        float rudderRate = !Mathf.Approximately(rudderInput, 0f) ? 1f / _rudderTimeToMax : _rudderDecay;

        _throttle = Mathf.MoveTowards(_throttle, throttleInput, throttleRate * Time.fixedDeltaTime);
        _rudder = Mathf.MoveTowards(_rudder, rudderInput, rudderRate * Time.fixedDeltaTime);
    }

    private void ApplyPropulsion()
    {
        if (Mathf.Approximately(_throttle, 0f) || _propulsorPoint == null) return;

        float speedRatio = Mathf.Clamp01(Mathf.Abs(Vector3.Dot(_rigidbody.linearVelocity, transform.forward)) / _maxSpeed);
        Vector3 force=transform.forward *(_engineForce*_throttle *(1f-speedRatio));

        _rigidbody.AddForceAtPosition(force, _propulsorPoint.position, ForceMode.Force);
    }

    private void ApplyRudder()
    {
        if (Mathf.Approximately(_rudder, 0f) || _rudderPoint == null) return;

        float forwardSpeed = Vector3.Dot(_rigidbody.linearVelocity, transform.forward);
        float effectiveness = Mathf.Clamp01(Mathf.Abs(forwardSpeed) / (_maxSpeed * 0.3f));
        Vector3 force = transform.right * (_maxRudderForce * _rudder * effectiveness);

        _rigidbody.AddForceAtPosition(force, _rudderPoint.position, ForceMode.Force);
    }

    //https://discussions.unity.com/t/sailing-ship-control/374700/20
    //https://www.habrador.com/tutorials/unity-boat-tutorial/
    //https://medium.com/@joshua.wiscaver/movement-in-unity-with-mathf-2aca0f649dc6

}
