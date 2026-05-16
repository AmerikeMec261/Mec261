using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleShipControl : MonoBehaviour
{
    [Header("Movement")]  //Las variables editables para moverle a los parametros de la fuerza .
    [SerializeField] private float _engineForce = 5000f;
    [SerializeField] private float _rudderForce = 1000f;

    [Header("Weight Feel")] //Variabled de altura 
    [SerializeField] private float _engineChangeSpeed = 0.5f;
    [SerializeField] private float _rudderChangeSpeed = 0.4f;

    [Header("References")] // Estos son los emptys en donde se van a plicar las fuerzas del barco para que avaze y para que gire.
    [SerializeField] private Transform _propeller;
    [SerializeField] private Transform _rudder;

    private Rigidbody _rigidbody; //Este es el Rigisbody del barco .

    private float _currentEngineInput; 
    private float _currentRudderInput;

    private void Awake() 
    { 
        _rigidbody = GetComponent<Rigidbody>(); //Aquí es para  
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

    private void UpdateEngineInput() //Aquí en el Update se aplican las físicas del Engine y su movimiento.
    {
        float targetEngineInput = 0f; //Aquí la fuerza o la aceleración empieza en 0.

        if (Input.GetKey(KeyCode.W)) { targetEngineInput = 1f; } // Si se presiona la tecla  W se aumenta 1 de fuerza al motor.
        else if (Input.GetKey(KeyCode.S)) { targetEngineInput = -1f; } // Pero si se presiona S se le resta 1 de fuerza al motor.

        float engineInputChangePerFrame = _engineChangeSpeed * Time.deltaTime; //El input cambia por frame multiplicado por el time.deltatiem.

        if (_currentEngineInput < targetEngineInput) // Si ela velocidad actual es menor al target .... ya no entendí las líneas hasta la 59 pero creo que es de aceleración progresiva ya que se multiplica mucho el input y el time.
        {
            _currentEngineInput += engineInputChangePerFrame;
            if (_currentEngineInput > targetEngineInput) { _currentEngineInput = targetEngineInput; }
        }
        else if (_currentEngineInput > targetEngineInput)
        {
            _currentEngineInput -= engineInputChangePerFrame;
            if (_currentEngineInput < targetEngineInput) { _currentEngineInput = targetEngineInput; }
        }
    }

    private void UpdateRudderInput()
    {
        float targetRudderInput = 0f; // La fuerza del rudder es 0 .

        if (Input.GetKey(KeyCode.A)) { targetRudderInput = 1f; } // Si se presiona la tecla A se le suma 1 a la fuerza del rudder.
        else if (Input.GetKey(KeyCode.D)) { targetRudderInput = -1f; } // Si se presiona la tecla D se le resta 1 de fuerza al rudder.

        float rudderInputChangePerFrame = _rudderChangeSpeed * Time.deltaTime; // Nuevamente no entiendo las líneas siguientes pero de igual manera creo es lo de el giro gradual ya que veo que se multiplican varias veces el tiempo y los frames .

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
    }

    private void ApplyEngineForce() //Aquí se le aplican las fuerzas al motor del barco.
    {
        Vector3 engineForceDirection = transform.right; //Aquí aplica la fuerza del motor a la derecha.
        Vector3 engineForce = engineForceDirection * _currentEngineInput * _engineForce; //Multiplica la fuerza de direccion del motor por el input actual por la fuerza del motor.
        _rigidbody.AddForceAtPosition(engineForce, _propeller.position, ForceMode.Force); //Agrega la fuerza al rigidbody.
    }

    private void ApplyRudderForce() //Aquí se le aplican las fuerzas al rudder del barco.
    {
        Vector3 rudderForceDirection = -transform.forward; //Aplica la fuerza del rudder al lado .
        Vector3 rudderForce = rudderForceDirection * _currentRudderInput * _rudderForce; //Multiplica la fuerza de dirección del rudder´por el input actual por la fuerza del rudder.
        _rigidbody.AddForceAtPosition(rudderForce, _rudder.position, ForceMode.Force); //Aplica la fuerza del rudder al rigidbody.
    }
}