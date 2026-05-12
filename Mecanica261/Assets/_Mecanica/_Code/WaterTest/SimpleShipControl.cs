using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleShipControl : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _engineForce = 5000f; // la cantidad de fuerza maxima que puede recibir el motor 
    [SerializeField] private float _rudderForce = 1000f; // La cantidad de fuerza maxima que puede recibir el timon

    [Header("Weight Feel")]
    [SerializeField] private float _engineChangeSpeed = 0.5f; // El motor que cambia la velocidad para generar esa sensacion de peso del barco
    [SerializeField] private float _rudderChangeSpeed = 0.4f; // El timon que cambia la velocidad para generar esa sensacion de peso del barco

    [Header("References")]
    [SerializeField] private Transform _propeller; // El transform del propulsor del barco
    [SerializeField] private Transform _rudder; // El transform del timon del barco

    private Rigidbody _rigidbody; // Referencia al rigidbody del barco 

    private float _currentEngineInput; // El valor actual del input del motor, que se va a ir acercando al valor objetivo para generar la sensacion de peso del barco
    private float _currentRudderInput; // El valor actual del input del timon, que se va a ir acercando al valor objetivo para generar la sensacion de peso del barco

    private void Awake() 
    { 
        _rigidbody = GetComponent<Rigidbody>(); // Referencia al rigibody en awake para llamarlo desde que inicie la escena
    }

    private void Update()
    {
        UpdateEngineInput(); // Llama la funcion y actualiza el input del motor constantemente
        UpdateRudderInput(); // Llama la funcion y actualiza el input del timon constantemente
    }

    private void FixedUpdate()
    {
        ApplyEngineForce(); // Llama la funcion de manera que se puedan aplicar las fisicas del motor en todos los intervalos de tiempo que se llame
        ApplyRudderForce(); // Llama la funcion de manera que se puedan aplicar las fisicas del timon en todos los intervalos de tiempo que se llame
    }

    private void UpdateEngineInput() // Se calcula el valor del input del motor para que al presionar foward o backward se almacene en la variable targetEngineInput la velocidad
    {
        float targetEngineInput = 0f; // variable que almacena el valor del input del motor que estara en 0 si no se recibe algun input 

        if (Input.GetKey(KeyCode.W)) { targetEngineInput = 1f; } // Si se presiona W, se almacena 1 de velocidad constante
        else if (Input.GetKey(KeyCode.S)) { targetEngineInput = -1f; } // Si se presiona S, se almacena -1 de velocidad deteniendo el barco

        float engineInputChangePerFrame = _engineChangeSpeed * Time.deltaTime; // suavizado constante del cambio del motor para que el movimiento sea fluido

        if (_currentEngineInput < targetEngineInput) //si _currentEngineInput es menor a targetEngineInput, se sumara engineInputChagePerFrame a _currentEngineInput
        {
            _currentEngineInput += engineInputChangePerFrame;
            if (_currentEngineInput > targetEngineInput) { _currentEngineInput = targetEngineInput; } // Si _currentEngineInput es mayor a targetEngineInput, _currentEngineInput es igual a targetEngineInput
        }
        else if (_currentEngineInput > targetEngineInput) // Si _currentEngineInput es mayor a TargetEngineInput, se le restara engineInputChangePerFrame a _currentEngineInput debido a la velocidad decreciente
        {
            _currentEngineInput -= engineInputChangePerFrame;
            if (_currentEngineInput < targetEngineInput) { _currentEngineInput = targetEngineInput; } // Si _currentEngineInput es menor a targetEngineInput, _currentEngineInput es igual a targetEngineInput


            // Metodo con Mathf.Clamp mas simplificado para realizar lo mismo del if anterior
            //-_currentEngineInput = Mathf.Clamp(_currentEngineInput, -1f, 0f);
        }
    }

    private void UpdateRudderInput() // Funcion que calcula el input constante del timon 
    {
        float targetRudderInput = 0f; // variable que almacena el valor del input del timon

        if (Input.GetKey(KeyCode.A)) { targetRudderInput = 1f; } // Si se presiona A, se almacena 1 de velocidad con el input constante en el timon
        else if (Input.GetKey(KeyCode.D)) { targetRudderInput = -1f; } // Si se presiona D, se almacena -1 de velocidad con el input constante en el timon

        float rudderInputChangePerFrame = _rudderChangeSpeed * Time.deltaTime; // Suavizado constante con el cambio de velocidad del timon con time.deltaTime

        if (_currentRudderInput < targetRudderInput) //Si _currentRudderInput es menor a TargetRudderInput, se sumara rudderInputChangePerFrame a _currentRudderInput de forma gradual
        {
            _currentRudderInput += rudderInputChangePerFrame;
            if (_currentRudderInput > targetRudderInput) { _currentRudderInput = targetRudderInput; } // Si _currentRudderInput es mayor a TargetRudderInput, _currentRudderInput es igual a TargetRudderInput
        }
        else if (_currentRudderInput > targetRudderInput) // Si _currentRudderInput es mayor a TargetRudderInput, se le restara rudderInputChangePerFrame a _currentRudderInput a la velocidad decreciente
        {
            _currentRudderInput -= rudderInputChangePerFrame;
            if (_currentRudderInput < targetRudderInput) { _currentRudderInput = targetRudderInput; } // Si _currentRudderInput es menor a TargetRudderInput, _currentRudderInput es igual a TargetRudderInput
        }
    }

    private void ApplyEngineForce() // Funcion que aplica la fuerza
    {
        Vector3 engineForceDirection = transform.right; // Define la direccion de la fuerza del motor, que es hacia la derecha del barco debido a que esta posicionado en x
        Vector3 engineForce = engineForceDirection * _currentEngineInput * _engineForce; // Calcula la fueza del motor multiplicando la direccion por el input actual del motor y la fuerza del motor
        _rigidbody.AddForceAtPosition(engineForce, _propeller.position, ForceMode.Force); // Aplica fuerza a la posicion del propulsor del barco con la fuerza calculada con ForceMode.Force de manera constante
    }

    private void ApplyRudderForce() // Funcion que aplica la fuerza del timon
    {
        Vector3 rudderForceDirection = -transform.forward; // Define la direccion de la fuerza del timon, que es hacia adelante del barco debido a que esta posicionado en z;
        Vector3 rudderForce = rudderForceDirection * _currentRudderInput * _rudderForce; // Calcula la fuerza del timon multiplicando la direccion por el input actual del timon y la fuerza del timon
        _rigidbody.AddForceAtPosition(rudderForce, _rudder.position, ForceMode.Force); // Aplica fuerza a la posicion del timon del barco con la fuerza calculada con ForceMode.Force de manera constante
    }
}