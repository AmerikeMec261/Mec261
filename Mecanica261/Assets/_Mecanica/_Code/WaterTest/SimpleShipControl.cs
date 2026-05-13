using UnityEngine;

[RequireComponent(typeof(Rigidbody))]  //agergar un rigid body en dado caso de que no tenga 1
public class SimpleShipControl : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _engineForce = 5000f;  //es la fuerza que va a tener el motor para ir hacia delante 
    [SerializeField] private float _rudderForce = 1000f;  //es la fuerza que va a tener el berco para girar

    [Header("Weight Feel")]
    [SerializeField] private float _engineChangeSpeed = 0.5f;   //rapidez del motor en arrancar
    [SerializeField] private float _rudderChangeSpeed = 0.4f; //rapides de giro del timon

    [Header("References")]
    [SerializeField] private Transform _propeller; //llamar al empty de donde esta el motor
    [SerializeField] private Transform _rudder; //llamar al emty del giro del barco 

    private Rigidbody _rigidbody; //el rigid body del baro para moverlo

    private float _currentEngineInput;  //guarda el valor de la aceleracion del motor 
    private float _currentRudderInput;  //guarda el valor de la fuerza de giro

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>(); //acceder al rigidbody para obtener sus fisicas 
    }

    private void Update()  //se agregan estas funciones aqui ya que son funciones que se tienen que actualizar lo mas rapido posible
    {
        UpdateEngineInput();    //funcion  de las teclas W y S
        UpdateRudderInput();    //funcion  de las teclas A y D
    }

    private void FixedUpdate()  //se agregaron estas en el fixedupdate porque se actualiza a un ritmo constante y tiene que ver con la fisicas
    {
        ApplyEngineForce(); //funcion del barco delante atras
        ApplyRudderForce(); //funcion giro
    }

    private void UpdateEngineInput()
    {
        float targetEngineInput = 0f;

        if (Input.GetKey(KeyCode.W)) { targetEngineInput = 1f; }  //si presiona la tecla W agregar 1 par a que se mueva el barco hacia delante
        else if (Input.GetKey(KeyCode.S)) { targetEngineInput = -1f; } //si presiona la tecla S restar 1 par a que se mueva el barco hacia atras 

        float engineInputChangePerFrame = _engineChangeSpeed * Time.deltaTime;  //cantidad maxima que puede avanzar el barco en en este frame 

        if (_currentEngineInput < targetEngineInput)    //aumenta la velocidad del barco si estamos lejos del enemy de manera lenta
        {
            _currentEngineInput += engineInputChangePerFrame;   //calera el motor poco a poco
            if (_currentEngineInput > targetEngineInput) { _currentEngineInput = targetEngineInput; }   //evita pasarse del valor que se ocupa 
        }
        else if (_currentEngineInput > targetEngineInput)   //si va muy rapido baja la velocidad 
        {
            _currentEngineInput -= engineInputChangePerFrame;   //reduce la velocidad 
            if (_currentEngineInput < targetEngineInput) { _currentEngineInput = targetEngineInput; }
        }
    }

    private void UpdateRudderInput()
    {
        float targetRudderInput = 0f;

        if (Input.GetKey(KeyCode.A)) { targetRudderInput = 1f; } //si presiona la tecla A agregar 1 par a que se mueva el barco hacia la izquierda
        else if (Input.GetKey(KeyCode.D)) { targetRudderInput = -1f; }  //si presiona la tecla D agregar 1 par a que se mueva el barco hacia la derecha

        float rudderInputChangePerFrame = _rudderChangeSpeed * Time.deltaTime;  //cantidad maxima de girto del barco en en este frame

        if (_currentRudderInput < targetRudderInput)
        {
            _currentRudderInput += rudderInputChangePerFrame;
            if (_currentRudderInput > targetRudderInput) { _currentRudderInput = targetRudderInput; }
        }
        else if (_currentRudderInput > targetRudderInput)                                                   //toda esta parte tiene la misma logica solo que este es para el giro
        {
            _currentRudderInput -= rudderInputChangePerFrame;
            if (_currentRudderInput < targetRudderInput) { _currentRudderInput = targetRudderInput; }
        }
    }

    private void ApplyEngineForce()
    {
        Vector3 engineForceDirection = transform.right; //define la direccion del barco
        Vector3 engineForce = engineForceDirection * _currentEngineInput * _engineForce;    //multiplica la direccion por el imput de avance por la fuerza de fuerza total 
        _rigidbody.AddForceAtPosition(engineForce, _propeller.position, ForceMode.Force);   //empuja todo el barco desde el empty del motor
    }

    private void ApplyRudderForce()
    {
        Vector3 rudderForceDirection = -transform.forward;  //define la direccion de giro
        Vector3 rudderForce = rudderForceDirection * _currentRudderInput * _rudderForce;    //calcula la fuerza del giro  
        _rigidbody.AddForceAtPosition(rudderForce, _rudder.position, ForceMode.Force);  //empuja todo el barco desde el empty del rudder

    }
}