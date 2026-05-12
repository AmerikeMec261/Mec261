using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
//Obliga al objeto a tener un RigidBody
public class SimpleShipControl : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _engineForce = 5000f; //Fuerza del motor
    [SerializeField] private float _rudderForce = 1000f; //Fuerza del timon

    [Header("Weight Feel")]
    [SerializeField] private float _engineChangeSpeed = 0.5f; //Velocidad a la que acelera
    [SerializeField] private float _rudderChangeSpeed = 0.4f; //Velocidad a la que gira

    [Header("References")]
    [SerializeField] private Transform _propeller; //Empty referente del motor 
    [SerializeField] private Transform _rudder;    //Empty referente al timon

    private Rigidbody _rigidbody; //RigidBody del barco

    private float _currentEngineInput; //Valor actual de aceleracion
    private float _currentRudderInput; //Valor actual del giro

    private void Awake() 
    { 
        _rigidbody = GetComponent<Rigidbody>(); //Obtiene el rigidBody de nuestro cubo
    }

    private void Update()
    {
        UpdateEngineInput(); //Actualiza el movimiento del motor 
        UpdateRudderInput(); //Actualiza el movimiento del timon
    }

    private void FixedUpdate()
    {
        ApplyEngineForce(); //Aplica lo mas rapido posible la fuerza de motor con fisicas
        ApplyRudderForce(); //Aplica lo mas rapido posible la fuerza del timon con fisicas
    }

    private void UpdateEngineInput()
    {
        float targetEngineInput = 0f; //Valor de la aceleracion

        if (Input.GetKey(KeyCode.W)) { targetEngineInput = 1f; } //Si se presiona W acelera hacia delante
        else if (Input.GetKey(KeyCode.S)) { targetEngineInput = -1f; } //Si se presiona s acelera hacia atras

        float engineInputChangePerFrame = _engineChangeSpeed * Time.deltaTime; //Calcula cuanto cambia por frame

        if (_currentEngineInput < targetEngineInput) //Si el valor actual es menor al del objetivo
        {
            _currentEngineInput += engineInputChangePerFrame; //Aumenta poco a poco
            if (_currentEngineInput > targetEngineInput) { _currentEngineInput = targetEngineInput; } //Este if evita que se pase del objetivo
        }
        else if (_currentEngineInput > targetEngineInput) // Si el valor actual es mayor al objetivo
        {
            _currentEngineInput -= engineInputChangePerFrame; //El valor disminuye poco a poco
            if (_currentEngineInput < targetEngineInput) { _currentEngineInput = targetEngineInput; } //Este if evita que baje mas del valor objetivo
        }
    }

    private void UpdateRudderInput()
    {
        float targetRudderInput = 0f; //Valor del giro

        if (Input.GetKey(KeyCode.A)) { targetRudderInput = 1f; } // Si presionas "A" gira a la izquierda
        else if (Input.GetKey(KeyCode.D)) { targetRudderInput = -1f; } // Si presionas "D" gira a la derecha

        float rudderInputChangePerFrame = _rudderChangeSpeed * Time.deltaTime; //Calcula cuanto tiempo cambia por frame

        if (_currentRudderInput < targetRudderInput) //Este if calcula si el giro es menor al objetivo
        {
            _currentRudderInput += rudderInputChangePerFrame; // Aumenta poco a poco 
            if (_currentRudderInput > targetRudderInput) { _currentRudderInput = targetRudderInput; } //Evita que se pase
        }
        else if (_currentRudderInput > targetRudderInput) //Este if Si el giro actual es mayor al objetivo
        {
            _currentRudderInput -= rudderInputChangePerFrame; //Disminuye poco a poco 
            if (_currentRudderInput < targetRudderInput) { _currentRudderInput = targetRudderInput; } //Evita que baje demasiado
        }
    }

    private void ApplyEngineForce()
    {
        Vector3 engineForceDirection = transform.right; //La direccion hacia donde avanzara el motor 
        Vector3 engineForce = engineForceDirection * _currentEngineInput * _engineForce; // Se usa para calcular la fuerza final del motor 
        _rigidbody.AddForceAtPosition(engineForce, _propeller.position, ForceMode.Force); // Aplica la fuerza en la posicion del empty
    }

    private void ApplyRudderForce()
    {
        Vector3 rudderForceDirection = -transform.forward; //Usa direccion contraria para girar
        Vector3 rudderForce = rudderForceDirection * _currentRudderInput * _rudderForce; //Se usa para calcular la fuerza final del timon
        _rigidbody.AddForceAtPosition(rudderForce, _rudder.position, ForceMode.Force); // Aplica la fuerza en la posicion del timon 
    }
}