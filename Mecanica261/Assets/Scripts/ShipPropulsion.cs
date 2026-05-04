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

    [Header("Hull Drag")]
    [SerializeField] private float _hullDrag = 0.08f;

    private Rigidbody _rigidbody;
    private float _throttle;
    private float _rudder;
    private float _engineForce;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _engineForce = _rigidbody.mass * (_maxSpeed / _timeToMaxSpeed) * 1.3f;
    }

    private void FixedUpdate()
    {
        
    }


}
