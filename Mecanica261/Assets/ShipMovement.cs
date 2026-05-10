using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipMovement : MonoBehaviour
{
    #region Variables

    [Header("Dependencies")]
    [SerializeField] private Transform _enginePoint;
    [SerializeField] private Transform _rudderPoint;

    [Header("Engine Settings")]
    [SerializeField] private float _moveForce = 12000f;
    [SerializeField] private float _accelerationTime = 4f;
    [SerializeField] private float _maxSpeed = 10f;

    [Header("Rudder Settings")]
    [SerializeField] private float _turnForce = 6000f;
    [SerializeField] private float _rudderTime = 2f;

    private Rigidbody _rigidbody;

    private float _currentMoveForce;
    private float _currentTurnForce;

    #endregion Variables


    #region Unity Methods

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        Movement();
        Sides();
    }

    #endregion Unity Methods


    #region Methods 

    private void Movement() //El barco está al revés
    {
        if (_enginePoint == null) { return; } //<-- Posible uso de IA. No hay necesidad de revisar si se supone que lo añades en el editor.

        float vertical = Input.GetAxis("Vertical");

        float targetForce = vertical * _moveForce;

        _currentMoveForce = Mathf.MoveTowards(_currentMoveForce, targetForce, (_moveForce / _accelerationTime) * Time.deltaTime);

        if (_rigidbody.linearVelocity.magnitude > _maxSpeed) { return; } // Esto se puede cambiar por un clamp

        Vector3 force = transform.forward * _currentMoveForce;

        _rigidbody.AddForceAtPosition(force, _enginePoint.position);
    }


    private void Sides()
    {
        if (_rudderPoint == null) { return; }  //<-- Posible uso de IA. No hay necesidad de revisar si se supone que lo añades en el editor.

        float horizontal = Input.GetAxis("Horizontal");

        float targetForce = horizontal * _turnForce;

        _currentTurnForce = Mathf.MoveTowards(_currentTurnForce, targetForce, (_turnForce / _rudderTime) * Time.deltaTime);

        Vector3 force = transform.right * _currentTurnForce;

        _rigidbody.AddForceAtPosition(force, _rudderPoint.position);
    }

    #endregion Methods
}