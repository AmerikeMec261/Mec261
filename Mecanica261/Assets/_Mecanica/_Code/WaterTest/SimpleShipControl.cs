using UnityEngine;

[RequireComponent(typeof(Rigidbody))]//el objeto tiene que tener un rigidbody
public class SimpleShipControl : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _engineForce = 5000f;// la fuerza maxima del motor
    [SerializeField] private float _rudderForce = 1000f;// la fuerza maxima del timon

    [Header("Weight Feel")]
    [SerializeField] private float _engineChangeSpeed = 0.5f;//que tan rapido responden
    [SerializeField] private float _rudderChangeSpeed = 0.4f;

    [Header("References")]
    [SerializeField] private Transform _propeller;// donde se aplica la fuerza del motor
    [SerializeField] private Transform _rudder;// donde se aplica la fuerza del timon

    private Rigidbody _rigidbody;//guarda la referencia del rigidbody del barco

    private float _currentEngineInput;
    private float _currentRudderInput;

    private void Awake() 
    { 
        _rigidbody = GetComponent<Rigidbody>(); //guarda la referencia al rigidbodypara poder usarla despues
    }

    private void Update()
    {
        UpdateEngineInput();//actualiza el motor una vez por frame
        UpdateRudderInput();// acrtualiza el timon una vez por frame
    }

    private void FixedUpdate()
    {
        ApplyEngineForce();//aplica la fuerza del motor
        ApplyRudderForce();// aplica la fuerza del timon
    }

    private void UpdateEngineInput()//no aplica la fuerza aun pero hace que los cambios sean suaves que sean poco a poco
    {
        float targetEngineInput = 0f;//se crea la variable local targetEngineInput

        if (Input.GetKey(KeyCode.W)) { targetEngineInput = 1f; }//es para detectar las teclas pulsadas en el teclado y uno suma y el otro resta
        else if (Input.GetKey(KeyCode.S)) { targetEngineInput = -1f; }

        float engineInputChangePerFrame = _engineChangeSpeed * Time.deltaTime;//calcula cuanto puede cambiar el input de el motor en frame

        if (_currentEngineInput < targetEngineInput)
        {
            _currentEngineInput += engineInputChangePerFrame;//si el input actual es menor que el que se quiere lo sube poco a poco
            if (_currentEngineInput > targetEngineInput) { _currentEngineInput = targetEngineInput; }//es para evitar pasarse del objetivo
        }
        else if (_currentEngineInput > targetEngineInput)//aqui al reves si el input actual es mayor al que se quiere lo baja poco a poco
        {
            _currentEngineInput -= engineInputChangePerFrame;
            if (_currentEngineInput < targetEngineInput) { _currentEngineInput = targetEngineInput; }
        }
    }

    private void UpdateRudderInput()
    {
        float targetRudderInput = 0f;//suponemos que emepezamos con el timon en 0

        if (Input.GetKey(KeyCode.A)) { targetRudderInput = 1f; }//aqui igual es para las letras que pulses en el teclado uno resta y uno suma
        else if (Input.GetKey(KeyCode.D)) { targetRudderInput = -1f; }

        float rudderInputChangePerFrame = _rudderChangeSpeed * Time.deltaTime;//igua calcula cuanto puede cambiar el input del timon en frame

        if (_currentRudderInput < targetRudderInput)
        {
            _currentRudderInput += rudderInputChangePerFrame;
            if (_currentRudderInput > targetRudderInput) { _currentRudderInput = targetRudderInput; }
        }
        else if (_currentRudderInput > targetRudderInput)
        {
            _currentRudderInput -= rudderInputChangePerFrame;
            if (_currentRudderInput < targetRudderInput) { _currentRudderInput = targetRudderInput; }
        }//hace los mismo que el del motor pero en el timon, hace suavemente los cambios 
    }

    private void ApplyEngineForce()
    {
        Vector3 engineForceDirection = transform.right;//se define la direccion en la que empuja el motor
        Vector3 engineForce = engineForceDirection * _currentEngineInput * _engineForce;//se calcula la fuerza final del motor, se multiplica la direccion con el input actual y la potencia maxima
        _rigidbody.AddForceAtPosition(engineForce, _propeller.position, ForceMode.Force);//aplica una fuerza en una posicion concreta del rigidbody
    }

    private void ApplyRudderForce()
    {
        Vector3 rudderForceDirection = -transform.forward;//lo mismo que en el motor se degine la direccion de la fuerza pero ahora del timon
        Vector3 rudderForce = rudderForceDirection * _currentRudderInput * _rudderForce;
        _rigidbody.AddForceAtPosition(rudderForce, _rudder.position, ForceMode.Force);
    }//se calculan las cosas igual que en el motor pero aplicadas al timon
}