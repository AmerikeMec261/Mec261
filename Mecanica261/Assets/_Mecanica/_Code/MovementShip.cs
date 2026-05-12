using UnityEngine;

public class MovementShip : MonoBehaviour
{
    [Header("movimiento")]
    [SerializeField] private Transform _motor; // Donde se generara la potencia
    [SerializeField] private float _steerPower = 500f; // el giro con 500 de fuerza definida
    [SerializeField] private float _power = 5f; // La fuerza del Forward (adelante y atras)
    [SerializeField] private float _maxSpeed = 10f; // La velocidad maxima a la que se puede llegar
    [SerializeField] private float _drag = 0.1f; // La resistencia al movimiento

    protected Rigidbody _rigidbody;
    protected Quaternion _startRotation;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _startRotation =_motor.localRotation; // mantenemos la rotacion inicial del motor
    }

    // Usa FixedUpdate paa suavizar el movimiento entre frames
    private void FixedUpdate()
    {
        var forceDirection = transform.forward;
        var _steer = 0;


        // Sistema de giro que hace de Rudder
        if (Input.GetKey(KeyCode.A))
        {
            _steer = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _steer = -1;
        }

        // Aplica una fuerza al motor de forma lateral
        _rigidbody.AddForceAtPosition(_steer * transform.right * _steerPower / 100f, _motor.position);

        var _forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);
        var _targetVel = Vector3.zero;

        // Aceleracion y freno del barco, (Se llama a otro script en particular)
        if (Input.GetKey(KeyCode.W))        
            PhysicsHelper.ApplyForceToReachVelocity(_rigidbody, _forward * _maxSpeed, _power);
        if (Input.GetKey(KeyCode.S))
            PhysicsHelper.ApplyForceToReachVelocity(_rigidbody, _forward * -_maxSpeed, _power);

    }



    //Gracias por poner la fuente, ahora se de dónde tomaste el ApplyForceToReachVelocity
    // https://www.youtube.com/watch?v=gdW_rXFE1Gk
}
