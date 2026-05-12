using UnityEngine;
using UnityEngine.Rendering;

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
    [SerializeField] private Transform _propeller; //Motor para adelante, motor para atrás
    [SerializeField] private Transform _rudder; //Timón para ir a los lados con el barco

    private Rigidbody _rigidbody;

    //Suaviza el movimiento del barco 
    private float _currentEngineInput;
    private float _currentRudderInput;

    private void Awake() 
    { 
        //Aqui definimos el rigidbody que usaremos de base para el control del barco
        _rigidbody = GetComponent<Rigidbody>(); 
    }

    private void Update()
    {
        //En este Update estamos agregando el trabajo de UpdateEngineInput y de UpdateRudderInput
        //Estos lo que hacen es manejar las direcciones en donde avanzará el barco y el giro del barco
        UpdateEngineInput();
        UpdateRudderInput();
    }

    private void FixedUpdate()
    {
        //Y por último agregamos un FixedUpdate para el barco pueda moverse y girarse mediante la información que le esta dando 
        //en el Update
        ApplyEngineForce();
        ApplyRudderForce();
    }

    private void UpdateEngineInput()
    {
        //Primero agregamos un float para determinar la direccion hacia adelante y hacia atrás
        float targetEngineInput = 0f; 
        // Después ponemos un  if que si el jugador presiona W, el targetEngine Input se incremena 1 y el barco inmediatamente va ir hacia adelante.
        //Pero si presiona S, el taretEngineInput se vuelve negativo y el barco va irse de reversa. 
        if (Input.GetKey(KeyCode.W)) { targetEngineInput = 1f; } 
        else if (Input.GetKey(KeyCode.S)) { targetEngineInput = -1f; }

        //Ahora el engineInputChangePerFrame lo que estamos haciendo es que a la hora de cambiar de dirección de derecha a izquierda haya un peso en el giro
        //gracias al timedeltaTime, que esta multiplicando el tiempo de cambio de dirección.
        float engineInputChangePerFrame = _engineChangeSpeed * Time.deltaTime;

        // Ahora debemos aplicar ese efecto de suavizado 

        //Ahora para mejorar el codigo y no agregar muchos if, podemos agregar un Mathf.Clamp
        //En si lo que estamos haciendo es lo mismo 
        // Lo que estamos estamos diciendo es que el currentEngineImput le estamos dando la información del engineInputChangePerFrame
        // Cuando vaya a moverse de adelante a atrás,
        // Y ya solo dentro de Unity por ejeplo: si el bote va hacia adelante y si de repente presiona S, 
        //El Mathf.Clanp hace un llamado a engineInputChangePerFrame y hace su trabjo de que el cambio de dirección sea pesado y no directo.
       _currentEngineInput = Mathf.Clamp(engineInputChangePerFrame, -1f, 1f);
        /*
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
        */
    }

    private void UpdateRudderInput()
    {
        //Primero agregamos un float para determinar la direccion derecha e izquierda
        float targetRudderInput = 0f;

        // Después ponemos un  if que si el jugador presiona A, el targetEngine Input se incremena 1 y el barco inmediatamente va girar a la derecha
        //Pero si presiona D, el taretEngineInput se vuelve negativo y el barco a la Izquierda
        if (Input.GetKey(KeyCode.A)) { targetRudderInput = 1f; }
        else if (Input.GetKey(KeyCode.D)) { targetRudderInput = -1f; }

        //Ahora el rudderInputChangePerFrame lo que estamos haciendo es que a la hora de cambiar de dirección de derecha a izquierda haya un peso en el giro
        //gracias al timedeltaTime, que esta multiplicando el tiempo de cambio de dirección.
        float rudderInputChangePerFrame = _rudderChangeSpeed * Time.deltaTime;


        //Ahora para mejorar el codigo y no agregar muchos if, podemos agregar un Mathf.Clamp
        // En si lo que estamos haciendo es lo mismo 
        // Lo que estamos estamos diciendo es que el currentRudderImput le estamos dando la información del rudderInputChangePerFrame
        // Cuando vaya a moverse de derecha a izquierda,
        // Y ya solo dentro de Unity por ejemplo: si el bote gire a la derecha y de repente presiona D para ir a la izquierda,
        //El Mathf.Clanp hace un llamado a rudderInputChangePerFrame y hace su trabjo de que el cambio de dirección sea pesado y no directo.
        _currentRudderInput = Mathf.Clamp(rudderInputChangePerFrame, -1f, 1f);

        /*
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
        */
    }

    private void ApplyEngineForce()
    {
        //En "UpdateEngineInput()" solo estamos calculando el valor de _currentEngineInput, pero todavia no agregamos una fueza para que avance el barco
        //Y por eso ocupamos el "ApplyEngineForce()" para agregar esa fuerza que necesita para moverse el barco

        //Primero agregamos un vector que diga el trabajo de "UpdateEngineInput()" sea en eje x "transform.right"
        // Después un calculo final de la fuerza del motor tomando en cuenta: La direccion que va ir dirigido, en que direccion esta mirando (adelante o atrás) y la fuerza de motor
        // Y por último solo agregamos esa fuerza para que el bote se mueva con un AddForceAtPosition tomando en cuenta la furza del motor, en donde estan colocados los motores del barco

        Vector3 engineForceDirection = transform.right;
        Vector3 engineForce = engineForceDirection * _currentEngineInput * _engineForce;
        _rigidbody.AddForceAtPosition(engineForce, _propeller.position, ForceMode.Force);
    }

    private void ApplyRudderForce()
    {
        //En "UpdateRudderInput()" solo estamos calculando el valor de _currentRudderInput, pero todavia no agregamos una fueza para que gire el barco
        //Y por eso ocupamos el "ApplyRudderForce()" para agregar esa fuerza que necesita para que gire el barco

        //Primero agregamos un vector que diga el trabajo de "UpdateRudderInput()" sea en eje z "transform.forward"
        // Después un calculo final de la fuerza del motor tomando en cuenta: La direccion que va ir dirigido, en que direccion esta girando (derecha o izquierda) y la fuerza de motor
        // Y por último solo agregamos esa fuerza para que el bote se mueva con un AddForceAtPosition tomando en cuenta la furza del motor, en donde estan colocados los motores del barco

        Vector3 rudderForceDirection = -transform.forward;
        Vector3 rudderForce = rudderForceDirection * _currentRudderInput * _rudderForce;
        _rigidbody.AddForceAtPosition(rudderForce, _rudder.position, ForceMode.Force);
    }
}