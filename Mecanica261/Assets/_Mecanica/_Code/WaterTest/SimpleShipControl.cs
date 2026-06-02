using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleShipControl : MonoBehaviour
{

    //aca tenemos todas las variables de nuestro motor para que podamos mover el barco.


    [Header("Movement")]
    [SerializeField] private float _engineForce = 5000f;
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
        UpdateEngineInput();  // vamos a estar actualizando los valores de nuestro barco
        UpdateRudderInput();
    }

    private void FixedUpdate()     // aplicamos las fuerzas para que funcione
    {
        ApplyEngineForce();
        ApplyRudderForce();
    }

    private void UpdateEngineInput()  //Vamos a obtener 
    {
        float targetEngineInput = 0f;

        if (Input.GetKey(KeyCode.W)) { targetEngineInput = 1f; }
        else if (Input.GetKey(KeyCode.S)) { targetEngineInput = -1f; } //Estas son solo las teclas de movimiento para moverlo hacia adelante y hacia atras

        float engineInputChangePerFrame = _engineChangeSpeed * Time.deltaTime;        //aca aplicamos las fuerzas gradualmente.

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

        if (Input.GetKey(KeyCode.A)) { targetRudderInput = 1f; }
        else if (Input.GetKey(KeyCode.D)) { targetRudderInput = -1f; }     //lo mismo las teclas pero para girar

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
        Vector3 engineForce = engineForceDirection * _currentEngineInput * _engineForce;    //Hacia donde vamos a aplicar las fuerzas
        _rigidbody.AddForceAtPosition(engineForce, _propeller.position, ForceMode.Force);
    }

    private void ApplyRudderForce()            
    {
        Vector3 rudderForceDirection = -transform.forward;
        Vector3 rudderForce = rudderForceDirection * _currentRudderInput * _rudderForce;     //lo mismo pero en la direccion
        _rigidbody.AddForceAtPosition(rudderForce, _rudder.position, ForceMode.Force);
    }
}