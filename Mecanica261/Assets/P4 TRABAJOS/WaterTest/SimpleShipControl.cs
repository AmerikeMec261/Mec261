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

    private float _currentEngineInput; //Se crea variable interna
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
        float targetEngineInput = 0f; //Se agrega una variable el cual mas adelante es el que decidira si ir hacia adelnta o retroceder

        if (Input.GetKey(KeyCode.W)) { targetEngineInput = 1f; } //Se agrega una condicion donde se checa si presionas W el valor de targetEngineInpunt cambia a 1f
        else if (Input.GetKey(KeyCode.S)) { targetEngineInput = -1f; } //Si no checa otra condicion donde verifica si se presiona S cambia su valor a -1f

        float engineInputChangePerFrame = _engineChangeSpeed * Time.deltaTime; //Se crea una variable de tipo float donde este se ira multiplicando por el tiempo que pase por frame con el objetivo de suavizar la aceleracion

        if (_currentEngineInput < targetEngineInput) //Se crea una condicional donde en este cuadro se verifica si ira hacia adelante o atras/ En esta condicion se checa si el valor de _currentEngineinput(0) es menor al valor que tiene targetEngineInput(1) 
        {
            _currentEngineInput += engineInputChangePerFrame; //Se guarda la sum
            if (_currentEngineInput > targetEngineInput) { _currentEngineInput = targetEngineInput; }//Este limita el numero

        }
        else if (_currentEngineInput > targetEngineInput)
        {
            _currentEngineInput -= engineInputChangePerFrame; //Se guarda la resta sobre si mismo guardando el valor negativo de engineInputChangePerFrame actualizandose por frame simulando la aceleracion que tendra al ir atras
            if (_currentEngineInput < targetEngineInput) { _currentEngineInput = targetEngineInput; }
        }
    }

    private void UpdateRudderInput()
    {
        float targetRudderInput = 0f;

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