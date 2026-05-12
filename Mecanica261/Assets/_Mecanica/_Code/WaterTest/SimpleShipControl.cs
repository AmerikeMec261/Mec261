using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleShipControl : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _engineForce = 5000f; //valor de fuerza del motor
    [SerializeField] private float _rudderForce = 1000f; //valor de fuerza del giro (timon)

    [Header("Weight Feel")]
    [SerializeField] private float _engineChangeSpeed = 0.5f; //valor de cambio de velocidad del motor
    [SerializeField] private float _rudderChangeSpeed = 0.4f; // valor de cambio de velocidad del giro (timon)

    [Header("References")]
    [SerializeField] private Transform _propeller;   // puntos de los elicces
    [SerializeField] private Transform _rudder; // puntos del giro (timon)

    private Rigidbody _rigidbody;

    private float _currentEngineInput; //punto actual del motor
    private float _currentRudderInput; //punto actual del timon

    private void Awake() 
    { 
        _rigidbody = GetComponent<Rigidbody>(); // llama primero al rigibody pa que sea obligatorio si no no funciona  
    }

    private void Update()
    {
        UpdateEngineInput(); //llama a esa cosa pa que vaya actualizando los valores del motor
        UpdateRudderInput(); //llama a esa cosa pa que vaya actualizando los valores del giro
    }

    private void FixedUpdate()
    {
        ApplyEngineForce(); //llama a esa cosa para que se vaya aplicando los cambios del motort
        ApplyRudderForce(); //llama a esa cosa para que se vaya aplicando los cambios del giro
    }

    private void UpdateEngineInput()
    {
        float targetEngineInput = 0f; //valor de los puntos de las elices

        if (Input.GetKey(KeyCode.W)) { targetEngineInput = 1f; }
        else if (Input.GetKey(KeyCode.S)) { targetEngineInput = -1f; }  //pa que se mueva ,1 pa que se mueva con w pa delante, -1 con S pa que se mueva pa tras

        float engineInputChangePerFrame = _engineChangeSpeed * Time.deltaTime; // pa que vaya cambiando la velocidad y que no acelere aca rapido

        if (_currentEngineInput < targetEngineInput)
        {
            _currentEngineInput += engineInputChangePerFrame;
            if (_currentEngineInput > targetEngineInput) { _currentEngineInput = targetEngineInput; }
        }
        else if (_currentEngineInput > targetEngineInput)
        {
            _currentEngineInput -= engineInputChangePerFrame;
            if (_currentEngineInput < targetEngineInput) { _currentEngineInput = targetEngineInput; }  // hace que no revase la velocidad actual 
        }
    }

    private void UpdateRudderInput()
    {
        float targetRudderInput = 0f; //valor de los puntos de las elices

        if (Input.GetKey(KeyCode.A)) { targetRudderInput = 1f; }
        else if (Input.GetKey(KeyCode.D)) { targetRudderInput = -1f; } // pa que gire, con 1 para que gire con A a la izquierda, -1 con D pa que gire a la derecha

        float rudderInputChangePerFrame = _rudderChangeSpeed * Time.deltaTime; // pa que vaya cambiando la velocidad de giro y que no lo cambie rapido

        if (_currentRudderInput < targetRudderInput)
        {
            _currentRudderInput += rudderInputChangePerFrame;
            if (_currentRudderInput > targetRudderInput) { _currentRudderInput = targetRudderInput; }
        }
        else if (_currentRudderInput > targetRudderInput)
        {
            _currentRudderInput -= rudderInputChangePerFrame;
            if (_currentRudderInput < targetRudderInput) { _currentRudderInput = targetRudderInput; } // hace que no revase la velocidad actual del giro
        }
    }

    private void ApplyEngineForce()
    {
        Vector3 engineForceDirection = transform.right; 
        Vector3 engineForce = engineForceDirection * _currentEngineInput * _engineForce;
        _rigidbody.AddForceAtPosition(engineForce, _propeller.position, ForceMode.Force); // aplica los valores de la fuerza para el motor (los punto s de la elice)
    }

    private void ApplyRudderForce()
    {
        Vector3 rudderForceDirection = -transform.forward;
        Vector3 rudderForce = rudderForceDirection * _currentRudderInput * _rudderForce;
        _rigidbody.AddForceAtPosition(rudderForce, _rudder.position, ForceMode.Force); // aplica los valores de la fuerza para el giro (el timon)
    }
}