using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleShipControl : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _engineForce = 5000f; //variable de la fuerza del motor   
    [SerializeField] private float _rudderForce = 1000f; //variable de la fuerza del timón 

    [Header("Weight Feel")]
    [SerializeField] private float _engineChangeSpeed = 0.5f; //variable la cual hace que el barco vaya aumentando de velocidad gradualmente
    [SerializeField] private float _rudderChangeSpeed = 0.4f; //variable la cual hacer que al girar sea de forma gradual 

    [Header("References")]
    [SerializeField] private Transform _propeller; //variable la cual hace la referencia a las helices del barco
    [SerializeField] private Transform _rudder; //variable la cual hace referencia al timón

    private Rigidbody _rigidbody;

    private float _currentEngineInput;//variable la cual hace que llame a lo que esta haciendo el motor al momento
    private float _currentRudderInput;//variable la cual llama a lo que esta haciendo el timón al momento

    private void Awake() 
    { 
        _rigidbody = GetComponent<Rigidbody>();//llama al rigidbody 
    }

    private void Update()
    {
        UpdateEngineInput();//llama al método que actualiza el motor 
        UpdateRudderInput();//llama al método que actualiza el timón 
    }

    private void FixedUpdate()
    {
        ApplyEngineForce();//llama el método que aplica la fuerza al motor 
        ApplyRudderForce();//llama al método que aplica la fuerza al timón 
    }

    private void UpdateEngineInput()
    {
        float targetEngineInput = 0f;//variable la cual llama a los puntos del motor 

        if (Input.GetKey(KeyCode.W)) { targetEngineInput = 1f; }
        else if (Input.GetKey(KeyCode.S)) { targetEngineInput = -1f; }//metodo los cuales preguntamos que si estamos presionando ya sea la tecla W/S haga que nos de un resultado 1f/-1f

        float engineInputChangePerFrame = _engineChangeSpeed * Time.deltaTime;//hacemos que la velocidad gradual se multiplique por cada frame

        if (_currentEngineInput < targetEngineInput)
        {
            _currentEngineInput += engineInputChangePerFrame;
            if (_currentEngineInput > targetEngineInput) { _currentEngineInput = targetEngineInput; }
        }
        else if (_currentEngineInput > targetEngineInput)
        {
            _currentEngineInput -= engineInputChangePerFrame;
            if (_currentEngineInput < targetEngineInput) { _currentEngineInput = targetEngineInput; } //hacemos que el barco no acelere de más y se sienta pesado al avanzar y que avance de forma gradual
        }
    }

    private void UpdateRudderInput()
    {
        float targetRudderInput = 0f;//variable la cual llama al timon del barco

        if (Input.GetKey(KeyCode.A)) { targetRudderInput = 1f; }
        else if (Input.GetKey(KeyCode.D)) { targetRudderInput = -1f; }//metodo el cual pregunta que tecla estamos precionando A/D haga que nos de un resultado 1f/-1f

        float rudderInputChangePerFrame = _rudderChangeSpeed * Time.deltaTime;//hacemos que el cambio de velocidad del timon se multiplique con cada frame

        if (_currentRudderInput < targetRudderInput)
        {
            _currentRudderInput += rudderInputChangePerFrame;
            if (_currentRudderInput > targetRudderInput) { _currentRudderInput = targetRudderInput; }
        }
        else if (_currentRudderInput > targetRudderInput)
        {
            _currentRudderInput -= rudderInputChangePerFrame;
            if (_currentRudderInput < targetRudderInput) { _currentRudderInput = targetRudderInput; }//hacemos que el barco gire de forma gradual y gire de manera lenta 
        }
    }

    private void ApplyEngineForce()//hacemos que la direccion del motor este sobre el eje x, y hacemos que la fuerza total del motor se aplique al rigidbody que en este caso seria al propeller y avace
    {
        Vector3 engineForceDirection = transform.right;
        Vector3 engineForce = engineForceDirection * _currentEngineInput * _engineForce;
        _rigidbody.AddForceAtPosition(engineForce, _propeller.position, ForceMode.Force);
    }

    private void ApplyRudderForce()//hacemos que el timon este viendo siempre al frente, y hacemos que la fuerza total del timon se aplique al rigidbody del timon y haga ese giro
    {
        Vector3 rudderForceDirection = -transform.forward;
        Vector3 rudderForce = rudderForceDirection * _currentRudderInput * _rudderForce;
        _rigidbody.AddForceAtPosition(rudderForce, _rudder.position, ForceMode.Force);
    }
}