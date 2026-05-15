using UnityEngine;

[RequireComponent(typeof(Rigidbody))]//hace que el objeto tenga un rigidbody
public class SimpleShipControl : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _engineForce = 5000f;//fuerza del motor del barco
    [SerializeField] private float _rudderForce = 1000f;//fuerza del timon del barco

    [Header("Weight Feel")]
    [SerializeField] private float _engineChangeSpeed = 0.5f;//velocidad del motor 
    [SerializeField] private float _rudderChangeSpeed = 0.4f;//velocidad del timon

    [Header("References")]
    [SerializeField] private Transform _propeller;// transform del motor del barco
    [SerializeField] private Transform _rudder;//transform del timon del barco

    private Rigidbody _rigidbody;// guarda un rigidbody para el barco

    private float _currentEngineInput;//guarda el motor del barco
    private float _currentRudderInput;//guarda el timon del barco

    private void Awake() //se ejecuta al iniciar el objeto
    { 
        _rigidbody = GetComponent<Rigidbody>();//detecta que el objeto tenga un rigidbody 
    }

    private void Update()//se ejecuta en cada fps
    {
        UpdateEngineInput();//hace que el motor se actualize
        UpdateRudderInput();//hace que el timon se actualize
    }

    private void FixedUpdate()//se ejecuta varias veces por segundo
    {
        ApplyEngineForce();//le da la fuerza al motor
        ApplyRudderForce();//le da la fuerza al timon
    }

    private void UpdateEngineInput()////hace que el motor se actualize
    {
        float targetEngineInput = 0f;//es el valor que se le da al motor

        if (Input.GetKey(KeyCode.W)) { targetEngineInput = 1f; }//pone la tecla w para avanzar 
        else if (Input.GetKey(KeyCode.S)) { targetEngineInput = -1f; }//pone la tecla s para dar para atras

        float engineInputChangePerFrame = _engineChangeSpeed * Time.deltaTime;//calcula cuanto avanza por cada fps

        if (_currentEngineInput < targetEngineInput)//revisa si acelera o no
        {
            _currentEngineInput += engineInputChangePerFrame;//hace que avance poco a poco
            if (_currentEngineInput > targetEngineInput) { _currentEngineInput = targetEngineInput; }//evita que se pase del valor maximo que le dimos
        }
        else if (_currentEngineInput > targetEngineInput)// revisa si hay que ir mas lento
        {
            _currentEngineInput -= engineInputChangePerFrame;//va reduciendo poco a poco la velocidad
            if (_currentEngineInput < targetEngineInput) { _currentEngineInput = targetEngineInput; }//revisa que no baje demaciado la velocidad
        }
    }

    private void UpdateRudderInput()//hace que el timon se actualize
    {
        float targetRudderInput = 0f;//es el valor que se le da al timon

        if (Input.GetKey(KeyCode.A)) { targetRudderInput = 1f; }//hace que gire hacia la izquierda con la tecla a
        else if (Input.GetKey(KeyCode.D)) { targetRudderInput = -1f; }//hace que gire a la derecha con la tecla d

        float rudderInputChangePerFrame = _rudderChangeSpeed * Time.deltaTime;//hace que el timon gire poco a poco

        if (_currentRudderInput < targetRudderInput)
        {
            _currentRudderInput += rudderInputChangePerFrame;//hace que el giro sea poco a poco
            if (_currentRudderInput > targetRudderInput) { _currentRudderInput = targetRudderInput; }
        }
        else if (_currentRudderInput > targetRudderInput)//
        {
            _currentRudderInput -= rudderInputChangePerFrame;//hace que el giro se redusca poco a poco
            if (_currentRudderInput < targetRudderInput) { _currentRudderInput = targetRudderInput; }
        }
    }

    private void ApplyEngineForce()//hace que el barco se mueva
    {
        Vector3 engineForceDirection = transform.right;//le da la direccion hacia enfrente al barco
        Vector3 engineForce = engineForceDirection * _currentEngineInput * _engineForce;//calcula la fuerza que va a agarrar el motor
        _rigidbody.AddForceAtPosition(engineForce, _propeller.position, ForceMode.Force);//le aplica la fuerza al rigidbody del motor
    }

    private void ApplyRudderForce()//hace que el barco gire 
    {
        Vector3 rudderForceDirection = -transform.forward;// le da la direccion hacia los lados al barco
        Vector3 rudderForce = rudderForceDirection * _currentRudderInput * _rudderForce;//calcula la fuerza que va a agarrar el timon
        _rigidbody.AddForceAtPosition(rudderForce, _rudder.position, ForceMode.Force);//le aplica la fuerza al rigidbody del timon
    }
}