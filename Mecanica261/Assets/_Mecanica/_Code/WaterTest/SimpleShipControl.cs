using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleShipControl : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _engineForce = 5000f;   // Se crean las variables privadas con serializafield para poder manipularlo dentro de unity 
    [SerializeField] private float _rudderForce = 1000f;

    [Header("Weight Feel")]
    [SerializeField] private float _engineChangeSpeed = 0.5f;
    [SerializeField] private float _rudderChangeSpeed = 0.4f;

    [Header("References")]
    [SerializeField] private Transform _propeller;
    [SerializeField] private Transform _rudder;

    private Rigidbody _rigidbody;

    private float _currentEngineInput;
    private float _currentRudderInput;

    private void Awake() 
    { 
        _rigidbody = GetComponent<Rigidbody>(); 
    }

    private void Update()
    {
        UpdateEngineInput();
        UpdateRudderInput();
    }

    private void FixedUpdate()
    {
        ApplyEngineForce();
        ApplyRudderForce();
    }

    private void UpdateEngineInput()
    {
        float targetEngineInput = 0f; //  variable flotante publica para poder manipularla libremente

        if (Input.GetKey(KeyCode.W)) { targetEngineInput = 1f; }   // Se crea un if para el input del movimiento con un keycode y la variable creada anteriormente con un float 
        else if (Input.GetKey(KeyCode.S)) { targetEngineInput = -1f; } // if else como otra alternativa si es que presionas la letra S como otra opcion para que esta vaya hacia atras

        float engineInputChangePerFrame = _engineChangeSpeed * Time.deltaTime; // Se crea la variable y se pone la formula de la variable * el tiempo entre cada frame 

        if (_currentEngineInput < targetEngineInput) // Se pone un if si current es menor que target pasan diferentes situaciónes
        {
            _currentEngineInput += engineInputChangePerFrame; // se hace la operacion dw current += con engine para que el resultado de almacene en current 
            if (_currentEngineInput > targetEngineInput) { _currentEngineInput = targetEngineInput; } // Se hace la operacion de que si current es mayor que target engine input  para que la variable no se pase de lo debido (lo limita)
        }
        else if (_currentEngineInput > targetEngineInput) // y aqui es la otra opcion de else, si current es menor que target 
        {
            _currentEngineInput -= engineInputChangePerFrame;  //y aqui es la resta para que detecte la S osea en reversa pero en resta y se almacene en current 
            if (_currentEngineInput < targetEngineInput) { _currentEngineInput = targetEngineInput; } // Aqui es si el current es menor que target igual hace que no se sobre pase y le pone un limite 
        }
    }

    private void UpdateRudderInput()
    {
        float targetRudderInput = 0f; //se crea la variable floar de targetrudderinput

        if (Input.GetKey(KeyCode.A)) { targetRudderInput = 1f; } 
        else if (Input.GetKey(KeyCode.D)) { targetRudderInput = -1f; }

        float rudderInputChangePerFrame = _rudderChangeSpeed * Time.deltaTime; 

        if (_currentRudderInput < targetRudderInput)
        {
            _currentRudderInput += rudderInputChangePerFrame;
            if (_currentRudderInput > targetRudderInput) { _currentRudderInput = targetRudderInput; }
        }
        else if (_currentRudderInput > targetRudderInput)
        {
            _currentRudderInput -= rudderInputChangePerFrame;
            if (_currentRudderInput < targetRudderInput) { _currentRudderInput = targetRudderInput; }
        }
    }  // aqui es la misma lofica que la anterior para adelante y atras pero hacia los lados 

    private void ApplyEngineForce()
    {
        Vector3 engineForceDirection = transform.right; // Aqui usamos el vector3 con el engine force, para darle direccion al barco 
        Vector3 engineForce = engineForceDirection * _currentEngineInput * _engineForce; // Aqui al igualmente usamos el vector3 por el engine force para darle potencia al motor 
        _rigidbody.AddForceAtPosition(engineForce, _propeller.position, ForceMode.Force); // Aqui usamos el addforceatposition para con la formula para darle potencia al barco
    }

    private void ApplyRudderForce()
    {
        Vector3 rudderForceDirection = -transform.forward;
        Vector3 rudderForce = rudderForceDirection * _currentRudderInput * _rudderForce;
        _rigidbody.AddForceAtPosition(rudderForce, _rudder.position, ForceMode.Force);
    } // Aqui usamos la misma logica que arriba para darle direccion hacia adelante
}