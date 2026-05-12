using UnityEngine;

[RequireComponent(typeof(Rigidbody))]  //agergar un rigid body en dado caso de que no tenga 1
public class SimpleShipControl : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _engineForce = 5000f;  //es la fuerza que va a tener el motor para ir hacia delante 
    [SerializeField] private float _rudderForce = 1000f;  //es la fuerza que va a tener el berco para girar

    [Header("Weight Feel")]
    [SerializeField] private float _engineChangeSpeed = 0.5f;
    [SerializeField] private float _rudderChangeSpeed = 0.4f;

    [Header("References")]
    [SerializeField] private Transform _propeller; //llamar al empty de donde esta el motor
    [SerializeField] private Transform _rudder; //llamar al emty del giro del barco 

    private Rigidbody _rigidbody; //el rigid body del baro para moverlo

    private float _currentEngineInput;
    private float _currentRudderInput;

    private void Awake() 
    { 
        _rigidbody = GetComponent<Rigidbody>(); //acceder al rigidbody para obtener sus fisicas 
    }

    private void Update()  //se agregan estas funciones aqui ya que son funciones que se tienen que actualizar lo mas rapido posible
    {
        UpdateEngineInput();
        UpdateRudderInput();
    }

    private void FixedUpdate()  //se agregaron estas en el fixedupdate porque se actualiza a un ritmo constante y tiene que ver con la fisicas
    {
        ApplyEngineForce();
        ApplyRudderForce();
    }

    private void UpdateEngineInput()
    {
        float targetEngineInput = 0f;  //

        if (Input.GetKey(KeyCode.W)) { targetEngineInput = 1f; }  //si presiona la tecla W agregar 1 par a que se mueva el barco hacia delante
        else if (Input.GetKey(KeyCode.S)) { targetEngineInput = -1f; } //si presiona la tecla S restar 1 par a que se mueva el barco hacia atras 

        float engineInputChangePerFrame = _engineChangeSpeed * Time.deltaTime;

        if (_currentEngineInput < targetEngineInput)
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
        float targetRudderInput = 0f;

        if (Input.GetKey(KeyCode.A)) { targetRudderInput = 1f; } //si presiona la tecla W agregar 1 par a que se mueva el barco hacia la izquierda
        else if (Input.GetKey(KeyCode.D)) { targetRudderInput = -1f; }  //si presiona la tecla W agregar 1 par a que se mueva el barco hacia la derecha

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
    }

    private void ApplyEngineForce()
    {
        Vector3 engineForceDirection = transform.right;
        Vector3 engineForce = engineForceDirection * _currentEngineInput * _engineForce;
        _rigidbody.AddForceAtPosition(engineForce, _propeller.position, ForceMode.Force);
    }

    private void ApplyRudderForce()
    {
        Vector3 rudderForceDirection = -transform.forward;
        Vector3 rudderForce = rudderForceDirection * _currentRudderInput * _rudderForce;
        _rigidbody.AddForceAtPosition(rudderForce, _rudder.position, ForceMode.Force);
    }
}