using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.UIElements; //<- Posible uso de IA por librerias fantasma


//Muchas gracias por las referencias
//Referencias
//https://filehostingbucket.web.app/Unity/Instructables/index.html
//https://github.com/ditzel/UnityOceanWavesAndShip/blob/master/Waves/Assets/WaterBoat.cs 
//https://www.youtube.com/watch?v=gdW_rXFE1Gk&t=1s
//https://www.youtube.com/watch?v=eL_zHQEju8s&t=6s
//https://codefinity.com/es/courses/v2/9284ee3b-35da-4063-b243-6f8e8cbd4412/0838fd67-05d4-4253-a981-5fb1b74c885e/823bce7d-7f39-43d1-aa07-c3d05bf3e61d
public class CalculateArea : MonoBehaviour

//Codigos y variables exactas reutilizadas de los codigos de los primeros links o referencias
/*
/////////////// Script - WaterBoat ////////////////////

* Variables:

public float SteerPower = 500f; Linea 12

* Lineas de codigo exactas y Reutilizadas

var forceDirection = transform.forward; Linea 38
        var steer = 0; Linea 39

        //steer direction [-1,0,1]
        if (Input.GetKey(KeyCode.A)) Linea 42
            steer = 1; Linea 43
        if (Input.GetKey(KeyCode.D)) Linea 44
            steer = -1; Linea 45
 Rigidbody.AddForceAtPosition(steer * transform.right * SteerPower / 100f, Motor.position); Linea 49

/////////////// Script - ShipMove ////////////////////

* Variables

public float maxThrustForce = 50f; Linea 17
public float thrustAcceleration = 5f; Linea 18

public float targetThrust = 0f; Linea 26 
public float currentThrust = 0f; Linea 27

* Lineas de Codigo exactas y Reutilizadas

void OnForward(InputValue value) { Linea 84
        if (value.isPressed) { Linea 85
            targetThrust = Mathf.Clamp(targetThrust + throttleStep, -maxThrustForce * 0.5f, maxThrustForce); Linea 86

void OnBackward(InputValue value) { Linea 90
        if (value.isPressed) { Linea 91
            targetThrust = Mathf.Clamp(targetThrust - throttleStep, -maxThrustForce * 0.5f, maxThrustForce); Linea 86
        }

 currentThrust = Mathf.MoveTowards(currentThrust, targetThrust, thrustAcceleration * Time.fixedDeltaTime); Linea 92
       
    rb.AddForce(transform.forward * currentThrust); Linea 58
 */
{

    [SerializeField] private float _waterLevel;
    [SerializeField] private float _waterDensity;
    [SerializeField] private float _waterDrag;

    [SerializeField] private float _shapeFactor;
    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _bottomPoint;
    [SerializeField] private List<Transform> _floatPoints;
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private float _area;
    [SerializeField] private float _hullHeight;
    [SerializeField] private float _hullVolume;
    [SerializeField] private float _draft;

    [SerializeField] private Transform _motorPoint;//Primero se necesita un vacio como motor un punto donde se aplique la fuerza para hacer el efecto de rotacion
    [SerializeField] private float _steerPower = 500f;//Esta variable se reutilizo para definir la fuerza con el cual rota el barco

    [SerializeField] private float _maxThrustForce = 50700f; //Esta variable es necesaria para definir el maimo empuje que este tendra ya que la mass del rigidbody es grande para simular el eso del barco se necesita grandes cantidades para que este funcione poniendo un limite
    [SerializeField] private float _thrustAcceleration = 4225f;//Esta valor sirve para definiri que tan rapido cambiara el valor hacia su objetivo
    [SerializeField] private float _targetThrust = 0f; //Guarda el empuje deseado o el valor objetivo a llegar
    [SerializeField] private float _currentThrust = 0f; // Esta variable se guarda gradualmente el empuje que tendra en el momento siempre actualizandose cada vez que este aumente o descienda

    float Area { get => _area; }
    float HullHeight { get => _hullHeight; }
    float HullVolume { get => _hullVolume; }
    float Draft { get => _draft; }

    //hullheight = toppoint.position.y - bottomPoint.poisition.y
    //VOLUMNE ARE * HULLhEIGHT
    //FLOAT REQUIERED VOLUMEN = _RIGIDBODY.MASS/ WATERDENSITY
    //_DRAFT = VOLUMEN REQUERIDO/ AREA * shapefactor
    void Awake()
    {
        
        _rigidbody = GetComponent<Rigidbody>();
        CalculateHullData();


    }
    void FixedUpdate()
    {
        FloatShip();
        ShipController(); //Se actualiza el control del barco, aplicando  el giro y el empuje gradualmente 
    }
    private void FloatShip()
    {
        float gravity = Physics.gravity.magnitude;
        float volumePerPoint = _hullVolume / _floatPoints.Count;

        for (int i = 0; i < _floatPoints.Count; i++)
        {
            Transform point = _floatPoints[i];

            float submersion = Mathf.Clamp01((_waterLevel - point.position.y) / _hullHeight);
            if (submersion <= 0f) { continue; }

            float force = _waterDensity * volumePerPoint * gravity * submersion;
            _rigidbody.AddForceAtPosition(Vector3.up * force, point.position, ForceMode.Force);

            Vector3 velocity = _rigidbody.GetPointVelocity(point.position);
            _rigidbody.AddForceAtPosition(-velocity * _waterDrag * submersion, point.position, ForceMode.Force);
        }
    }

    private void ShipController() //Explicacion de codigos y el porque se escogieron especificamente esos junto con correcionesdel examen
    {
        /*
        Se inicio primero con la rotacion gracias al script respectivo esas lineas las agarre especifimanete ya que solo necesitaba la rotacion porque a pesar de este ya ser controlador, aqui no estamos implementando
        ningun sistema de particulas, mas aparte que presentaba metodos y funciones el cual no tengo ningun conocimiento a comparacion de las lineas puestas aqui, asi que de ese script lo que se rescato fue el controlador basico con respecto a la rotacion 
        */
        int steer = 0; //Se determina una variable con la cantidad 0 llamada steer donde este representa una rotacion nula y de var se cambio a int porque el tipo de dato var no estoy seguro de como funciona y quiero aplicar lo que tengo de conocimientos con algo seguro como int sin embargo esto no modifica nada porque el objetivo es que este sea una variable de tipo entera donde se decide para donde girar con -1 o 1 que se explicara en breve 

        if (Input.GetKey(KeyCode.A)) //Iniciamos con las condiciones basicas, si al presionar la tecla asignada A esta cambiara su numero a 1 como se indicia en la siguiente linea sin embargo esto solo es para indicar aun no hace alguna accion hasta el momento, pero esto representa la direccion .
            steer = 1;
        if (Input.GetKey(KeyCode.D))//La siguiente condicion es al presionar la tecla D el numero se cambia -1 representando la izquierda misma funcion pero en viceversa. 
            steer = -1;

        if (_motorPoint != null)//Se asigna una condicion extra donde se comprueba que el empty este asignado con el punto de que no cause un error 
            _rigidbody.AddForceAtPosition(steer * transform.right * _steerPower / 100f, _motorPoint.position); //Ahora al ver esta linea en las instrucciones se necesitaba aplicar AddForceAtPosition y cumple con la funcion o efecto de rotacion de manera basica 
        //Para el calculo se utiliza el _rigidbody para aplicarle la fuerza en una posicion aqui se decide la direccion hacia la que va steer se multiplica con el verctor que ya nos da transform.right se multiplica con la fuerza que se tiene es _steerpower que se divide por 100 para controlar la intensidad del giro esto no se movio del codio para no romper su funcion sin embargo esto me facilita para bajar fuerzas intensas mas grandes como 350000f ya que el barco tiene una masa grande puesto en el editor y por ultimo se aplica la fuerza en _motorPoint.position este empuja el empty hacia la derecha o izquierda de acuerdo a la variable que tenga steer  
        //El _motorPoint es conveniente que quede fuera o en la esquina de atras del rigidbody 

        //Aqui se empieza con la siguiente implementacion con la aceleracion gradualmente  
        //Se necesita establecer una condicion para designar una tecla 
        if (Input.GetKey(KeyCode.W)) //En _targetThrust se aplica una formula par ahacer un empuje gradual simulando la aceleracion hasta llegar al maximo pero esta vez gracias a la fuerza
            _targetThrust = Mathf.Clamp(_targetThrust + _thrustAcceleration * Time.fixedDeltaTime, 0f, _maxThrustForce); //Se aplica Mathf.Clamp para limitar un rango de minimo a maximo en esta de 0 a hasta el valor maximo en _maxThurstForce a _targetThrust se le va sumando la cantidad de _thrustAcceleration por Time.deltaTime en vez de que se actualice por frame se actualiza cada segundo por el tiempo transcurrido logrando que el barco en cierta cantidad llegue a la maxima cantidad en ciertos segundos. 
        else                                                                                                        //En este caso seria de esta manera en 4225 por segundo llegara a su maximo empuje 50700 simulando la velocidad donde en 12 segundos llega a la maxima velocidad sin exagerar infinitamente por eso el limite
            _targetThrust = 0f; //Si no se presiona la tecla W se regresa a 0 para que el barco deje de aumentar la fuerta hacia adelante.

        if (Input.GetKey(KeyCode.S)) //Correcion: Añado la tecla S con la misma logica pero invirtiendo los signos y poniendo un nuevo limite negativo para que retroceda y cambio a una mejor estabilidad en cuanto Time.deltaTime (retroalimentacion tomada en cuenta en clase)
            _targetThrust = Mathf.Clamp(_targetThrust - _thrustAcceleration * Time.fixedDeltaTime, -_maxThrustForce, _maxThrustForce);//Es una solcuion cercana, si retrocede pero con muy poco valor a maximo a -84

        _currentThrust = Mathf.MoveTowards(_currentThrust, _targetThrust, _thrustAcceleration * Time.fixedDeltaTime); //_currentThust es el empuje que esta usando en el momenot que va cambiando conforme acelere el barco
        //Mathf.MoveTowards es el que acerca un valora otro de manera progresiva es decir que este evita que cambie de golpe o que baje gradualmente suave
        //La formula que esta contiene es que con el valor actual que tenga currentThrust, se va actualizando con la multiplicacion previa hecha para aceleracion donde el objetivo de este es el valor que tenga _targetThrust en ese momento.
        //Esta linea es la responsables de la aceleracion progresiva que tiene y la desaceleracion al dejar de presionar la W cumpliendo una de las instrucciones del examen que ni acelere de golpe y que no pare de golpe
        _rigidbody.AddForceAtPosition(transform.forward * _currentThrust, _motorPoint.position); //Correcion: Por ultimo se aplica la fuerza hacia adelante y esta fuerza se multiplica por el valor de _currenttrust permitiendo una acelracion correcta y desaceleracion

        //Conclusion:
        /*
        Todas estas lineas fueron reutilizadas y no se escogieron al azar solo necesitaba pequeños comportamientos que cumplieran con las indicaciones del profesor
        solo necesitaba 3 comportamientos los cuales eran girar, acelerar y desacelerar gradualmente estas lineas son las que mejor encajaban en el codigo adaptandolas ligeramente 
        pero con elmismo funcionamiento y sin que entre en conflicto con lo ya calculado puedo decir que esto fue un complemento con conceptos los cuales si tengo un
        conocimiento basico.
        */
 }
    private void CalculateHullData()
    {
        _area = CalculatedArea();
        _hullHeight = _topPoint.position.y - _bottomPoint.position.y;
        _hullVolume = _area * _hullHeight * _shapeFactor;

        float requiredVolume = _rigidbody.mass / _waterDensity;
        _draft = requiredVolume / (_area * _shapeFactor);


    }
    private float CalculatedArea()
    {
        float area = 0f;

        for (int i = 0; i < _floatPoints.Count; i++)
        {
            Vector3 current = transform.InverseTransformPoint(_floatPoints[i].position);
            Vector3 next = transform.InverseTransformPoint(_floatPoints[(i + 1) % _floatPoints.Count].position);

            area += (current.x * next.z) - (next.x * current.z);
        }

        return Mathf.Abs(area) * 0.5f;
    }
    private void OnDrawGizmos()
    {
        if (_floatPoints == null || _floatPoints.Count < 2) { return; }

        Gizmos.color = Color.green;

        for (int i = 0; i < _floatPoints.Count; i++)
        {
            Vector3 current = _floatPoints[i].position;
            Vector3 next = _floatPoints[(i + 1) % _floatPoints.Count].position;

            Gizmos.DrawLine(current, next);
        }
    }

    /*
    [Header("Water")]
    [SerializeField] private float _waterLevel = 0f;
    [SerializeField] private float _waterDensity = 1000f;
    [SerializeField] private float _waterDrag = 1f;

    [Header("Hull")]
    [SerializeField] private float _shapeFactor = 0.67f;
    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _bottomPoint;
    [SerializeField] private List<Transform> _floatPoints = new List<Transform>();

    private Rigidbody _rigidbody;

    private float _area;
    private float _hullHeight;
    private float _hullVolume;
    private float _draft;

    float Area { get => _area; }
    float HullHeight { get => _hullHeight; }
    float HullVolume { get => _hullVolume; }
    float Draft { get => _draft; }

    private void Awake()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    private void FloatShip()
    {
        
    }

    private void CalculateHullData()
    {
        
    }

    private float CalculateAreaXZ()
    {
        float area = 0f;

        for (int i = 0; i < _floatPoints.Count; i++)
        {
            Vector3 current = transform.InverseTransformPoint(_floatPoints[i].position);
            Vector3 next = transform.InverseTransformPoint(_floatPoints[(i + 1) % _floatPoints.Count].position);

            area += (current.x * next.z) - (next.x * current.z);
        }

        return Mathf.Abs(area) * 0.5f;
    }

    
     */
}
