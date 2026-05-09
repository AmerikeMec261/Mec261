using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoatMovement : MonoBehaviour
{
    [SerializeField] private Transform _engine;
    [SerializeField] private Transform _rudder;
    [SerializeField] private float _moveForce = 2000f;
    [SerializeField] private float _turnForce = 1500f;
    [SerializeField] private float _maxSpeed = 10f;

    private Rigidbody _rigidbody;

    void Start()
    {
       _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Move();
        Turn();
    }

    private void Move()
    {
        if (_engine == null) return; //<- Posible uso de IA. Verificar que el componente esté asignado en el inspector para evitar errores en tiempo de ejecución.
        float vertical = Input.GetAxis("Vertical");
        if (_rigidbody.linearVelocity.magnitude > _maxSpeed) return; //Debes limitar la velocidad con una correcta aplicación de fuerza
        Vector3 force =_engine.forward * vertical * _moveForce * Time.deltaTime; 
        _rigidbody.AddForceAtPosition(force, _engine.position);
    }

    private void Turn()
    {
        if (_rudder == null) return;  //<- Posible uso de IA. Verificar que el componente esté asignado en el inspector para evitar errores en tiempo de ejecución.
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 force = transform.right * horizontal * _turnForce * Time.deltaTime; //Buena aplicación de fuerza
        _rigidbody.AddForceAtPosition(force, _rudder.position);
    }
}
