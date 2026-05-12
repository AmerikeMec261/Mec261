using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleShipControl : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _engineForce = 5000f; //Engine Force por lo que entiendo es la fuerza que va a impulsar el barco hacía adelante o hacia atrás
    [SerializeField] private float _rudderForce = 1000f; // Rudder Force es la variable que va a indicar la fuerza que va a tener el timón del barco para que giré

    [Header("Weight Feel")]
    [SerializeField] private float _engineChangeSpeed = 0.5f; // Esta sección del codigo llamada Weight Feel determina que tan rápido es la aceleración del barco
    [SerializeField] private float _rudderChangeSpeed = 0.4f;

    [Header("References")]
    [SerializeField] private Transform _propeller; 
    [SerializeField] private Transform _rudder; 

    private Rigidbody _rigidbody; // El rigidbody es el elemento que hace que el barco sea afectado por las fisicas de Unity

    private float _currentEngineInput; // Esta línea indica que la aceleración hacia delante sea gradual
    private float _currentRudderInput; // Esta línea indica que la aceleración de giro del barco sea gradual

    private void Awake() 
    { 
        _rigidbody = GetComponent<Rigidbody>(); // Esta línea es la que al inicio del programa busca el componente Rigidbody del barco
    }

    private void Update()
    {
        UpdateEngineInput();
        UpdateRudderInput(); // En el Update esta llamando ambos metodos referenciados
    }

    private void FixedUpdate()
    {
        ApplyEngineForce();
        ApplyRudderForce(); // De igual manera en el Fixed Update se están llamando ambos metodos encargados de aplicar la fuerza
    }

    private void UpdateEngineInput()
    {
        float targetEngineInput = 0f; //Esta línea por lo que entiendo es la que indica cual va a ser la velocidad inicial del barco al iniciar el programa

        if (Input.GetKey(KeyCode.W)) { targetEngineInput = 1f; }
        else if (Input.GetKey(KeyCode.S)) { targetEngineInput = -1f; } // Estas dos líneas indican con que teclas se va a mover el barco sobre el eje X, en este caso, W para ir hacía adelante y S para ir hacía atrás

        float engineInputChangePerFrame = _engineChangeSpeed * Time.deltaTime; //Esta línea indica el cambio de velocidad que va a tener el barco por cada frame dentro del programa

        if (_currentEngineInput < targetEngineInput) 
        {
            _currentEngineInput += engineInputChangePerFrame;  //Esta línea se esncarga de que la velocidad actual del motor del barco, sea igual a la fuerza del frame actual
            if (_currentEngineInput > targetEngineInput) { _currentEngineInput = targetEngineInput; } // Esta linea hace que la current engine sea igual al target engine
        }
        else if (_currentEngineInput > targetEngineInput)
        {
            _currentEngineInput -= engineInputChangePerFrame; //Esta línea se enacarga de detectar si la velocidad actual es menor o igual a lo que debe tener el frame actual
            if (_currentEngineInput < targetEngineInput) { _currentEngineInput = targetEngineInput; } // Esta línea se encarga de que si el target engine es menor al engine input la iguale
        }
    }

    private void UpdateRudderInput()
    {
        float targetRudderInput = 0f; //Esta línea por lo que entiendo es la que indica cual va a ser la velocidad de giro inicial del barco al iniciar el programa

        if (Input.GetKey(KeyCode.A)) { targetRudderInput = 1f; }
        else if (Input.GetKey(KeyCode.D)) { targetRudderInput = -1f; } //Etsas dos líneas indican con que dos teclas se va a mover el barco cobre el eje Z

        float rudderInputChangePerFrame = _rudderChangeSpeed * Time.deltaTime;  //Esta línea indica el cambio de velocidad de giro que va a tener el barco por cada frame dentro del programa

        if (_currentRudderInput < targetRudderInput)
        {
            _currentRudderInput += rudderInputChangePerFrame; //Esta línea se esncarga de que la velocidad de giro actual del motor del barco, sea igual a la fuerza del frame actual
            if (_currentRudderInput > targetRudderInput) { _currentRudderInput = targetRudderInput; } // Esta linea hace que la current rudder sea igual al target rudder
        }
        else if (_currentRudderInput > targetRudderInput)
        {
            _currentRudderInput -= rudderInputChangePerFrame; //Esta línea se enacarga de detectar si la velocidad de giro actual es menor o igual a lo que debe tener el frame actual
            if (_currentRudderInput < targetRudderInput) { _currentRudderInput = targetRudderInput; } // Esta línea se encarga de que si el target rudder es menor al rudder input la iguale
        }
    }

    private void ApplyEngineForce()
    {
        Vector3 engineForceDirection = transform.right; // Esta linea se encarga de aplicar la fuerza que va a mover al barco hacía delante en eje X
        Vector3 engineForce = engineForceDirection * _currentEngineInput * _engineForce; // Esta línea se encarga de que la fuerza se aplique con la cantidad y dirección correctas 
        _rigidbody.AddForceAtPosition(engineForce, _propeller.position, ForceMode.Force);
    }

    private void ApplyRudderForce()
    {
        Vector3 rudderForceDirection = -transform.forward;
        Vector3 rudderForce = rudderForceDirection * _currentRudderInput * _rudderForce;
        _rigidbody.AddForceAtPosition(rudderForce, _rudder.position, ForceMode.Force);
    }
}