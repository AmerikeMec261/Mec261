using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleShipControl : MonoBehaviour
{
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
        float targetEngineInput = 0f;//Esta varable nos ayudara para saber en que posicion va estar el barco 

        if (Input.GetKey(KeyCode.W)) { targetEngineInput = 1f; }//Agregamos una condicion que si presiones la tecla W se sumara 1 al flotante targetEngineInput
        else if (Input.GetKey(KeyCode.S)) { targetEngineInput = -1f; }//Caso contrario que si presionas la tecla S se restara 1 al flotante targetEngineInput

        float engineInputChangePerFrame = _engineChangeSpeed * Time.deltaTime;//Variable para cambiar la velocidad del barco

        if (_currentEngineInput < targetEngineInput)//Cada vez que se toca la tecla w esto genera que el barco avance y tambien la otra if con la condicion
        {//de los dos sean mayores o menores que el _currentEngiineInput
            _currentEngineInput += engineInputChangePerFrame;//Si _currentEngineInput es menor que TargetEngineInput se le sumara 1 a engineInputChangePerFrame que hara que avance
            if (_currentEngineInput > targetEngineInput) { _currentEngineInput = targetEngineInput; }
        }
        else if (_currentEngineInput > targetEngineInput)
        {
            _currentEngineInput -= engineInputChangePerFrame;//Y si es caso contrario _curretEngineInput es mayor que TargetEngineInput este restara 1 a _currentEngioneInput y hara que retroseda
            if (_currentEngineInput < targetEngineInput) { _currentEngineInput = targetEngineInput; }
        }
    }

    private void UpdateRudderInput()
    {
        float targetRudderInput = 0f;//Agregamos una variable de tipo flotante para el movimiento

        if (Input.GetKey(KeyCode.A)) { targetRudderInput = 1f; }//Hacemos la indicacion que cuando presiones la tecla A se sumara 1 a nuestro flotante targetRudderInput
        else if (Input.GetKey(KeyCode.D)) { targetRudderInput = -1f; }//Caso contrario si es que presionamos la tecla D se restara 1 al flotantes TargetRudderInput

        float rudderInputChangePerFrame = _rudderChangeSpeed * Time.deltaTime;//Agregamos otra variable para asignarle la velocidad del barco

        if (_currentRudderInput < targetRudderInput)//Aqui ponemos otra condicion que si _currentRudderInput es menor que targetRudderInput
        {
            _currentRudderInput += rudderInputChangePerFrame;
            if (_currentRudderInput > targetRudderInput) { _currentRudderInput = targetRudderInput; }
        }
        else if (_currentRudderInput > targetRudderInput)
        {
            _currentRudderInput -= rudderInputChangePerFrame;//Aqui ponemos otra condicion que si _currentRudderInput es mayor que targetRudderInput
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