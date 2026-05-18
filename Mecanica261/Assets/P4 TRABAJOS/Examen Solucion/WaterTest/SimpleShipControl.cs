using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleShipControl : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _engineForce = 5000f; //Este variable es la fuerza que movera el barco hacia adlente o atras aplicada en la formula de ApplyEngineForce()
    [SerializeField] private float _rudderForce = 1000f; //Este variable es la fuerza que movera el barco hacia la derecha o izquierda aplicada en la formula de ApplyRudderForce()

    [Header("Weight Feel")]
    [SerializeField] private float _engineChangeSpeed = 0.5f;  //Este variable es la velocidad que en la que cambiara el numero de input por frame decidiendo que tecla se esta presionando para que el barco vaya para enfrente o atras con w o s y que el cambio sea gradual
    [SerializeField] private float _rudderChangeSpeed = 0.4f;  //Este variable es la velocidad que en la que cambiara el numero de input por frame decidiendo que tecla se esta presionando para que el barco vaya para la izquierda o derecha con A o D y que el cambio sea gradual

    [Header("References")]
    [SerializeField] private Transform _propeller; //Para aplicar la fuerza hacia adelnte o atras en el eje X se necesita un empty un punto en el cual aplicarla 
    [SerializeField] private Transform _rudder; //Para aplicar la fuerza hacia la derecha o izquierda en el eje Z se necesita un empty un punto en el cual aplicarla 

    private Rigidbody _rigidbody;

    private float _currentEngineInput; //Se crea variable interna para decidir posteriormente si va ir hacia adelante o atras utilizado en UpdateEngineInput y ApplyEngineForce
    private float _currentRudderInput; //Se crea variable interna para decidir posteriormente si va ir hacia adelante o atras utilizado en UpdateRudderInput y ApplyRudderForce

    private void Awake() //Link de referecia Awake: https://docs.unity3d.com/es/530/ScriptReference/MonoBehaviour.Awake.html
    { 
        _rigidbody = GetComponent<Rigidbody>();  //De una vez se pone una variable el cual almacene el rigidbody del barco para aplicarlo en las formulas ya que este se aplica 
    }

    private void Update()
    {
        UpdateEngineInput(); //Se actualiza el valor de currentEngineInput de acuerdo a la tecla que se presione W o S para almacenar hacia donde ira el barco tanto adelnta como atras
        UpdateRudderInput(); //Se actualiza el valor de currentRudderInput de acuerdo a la tecla que se presione A o D para almacenar hacia donde ira el barco de derecha a izquierda
    }

    private void FixedUpdate() //Hace que los calculos sean consistentes / Link de referencia FiexUpdate: https://docs.unity3d.com/6000.4/Documentation/ScriptReference/MonoBehaviour.FixedUpdate.html
    {
        ApplyEngineForce(); //Se actualiza y se aplica la fuerza de acuerdo al input dado y aplicando la formula dada en el respectivo metodo hacia el eje X
        ApplyRudderForce(); //Se actualiza y se aplica la fuerza de acuerdo al input dado y aplicando la formula dada en el respectivo metodo hacia el eje Z
    }

    private void UpdateEngineInput()
    {
        float targetEngineInput = 0f; //Se agrega una variable el cual mas adelante es el que decidira si ir hacia adelnte o retroceder

        if (Input.GetKey(KeyCode.W)) { targetEngineInput = 1f; }// Se agrega una condicion donde se checa si presionas W el valor de targetEngineInpunt cambia a 1f
        else if (Input.GetKey(KeyCode.S)) { targetEngineInput = -1f; } //Si no checa otra condicion donde verifica si se presiona S cambia su valor a -1f

        float engineInputChangePerFrame = _engineChangeSpeed * Time.deltaTime; //Se crea una variable de tipo float donde el valor de _engineChangeSpeed se ira multiplicando por el tiempo que pase por frame con el objetivo de suavizar al avance o retrocesos gradualmente

        if (_currentEngineInput < targetEngineInput) //En esta condicion se checa si el valor de _currentEngineInput(0) es menor al valor que tiene targetEngineInput(1)
        {
            _currentEngineInput += engineInputChangePerFrame; //Se guarda el valor del engineInputChangePerFrame en _currentEngineInput actualizandose por frame
            if (_currentEngineInput > targetEngineInput) { _currentEngineInput = targetEngineInput; }//La condicion verifica si el _currentEngineInput si es mayor a targetEngineInpút es decir si se pasa del valor asignado (1), este lo limitara al valor asignado de acuerdo al input (1)

        }
        else if (_currentEngineInput > targetEngineInput) //Si no, se checa otra condicion donde se verifica si el _currentEngineInput (0) si es mayor a targetEngineInput(-1) 
        {
            _currentEngineInput -= engineInputChangePerFrame; //Se guarda el valor negativo del engineInputChangePerFrame en _currentEngineInput actualizando por frame 
            if (_currentEngineInput < targetEngineInput) { _currentEngineInput = targetEngineInput; }//La condicion verifica si el _currentEngineInput si es menor a targetEngineInpút es decir si se pasa del valor negativo asignado (-1), este lo limitara al valor asignado de acuerdo al input (-1)
        }
    }

    private void UpdateRudderInput()
    {
        float targetRudderInput = 0f; //Se agrega una variable el cual mas adelante es el que decidira si ir hacia izquierda o derecha

        if (Input.GetKey(KeyCode.A)) { targetRudderInput = 1f; } // Se agrega una condicion donde se checa si presionas A el valor de targetEngineInpunt cambia a 1f
        else if (Input.GetKey(KeyCode.D)) { targetRudderInput = -1f; }//Si no checa otra condicion donde verifica si se presiona D cambia su valor a -1f

        float rudderInputChangePerFrame = _rudderChangeSpeed * Time.deltaTime; //Se crea una variable de tipo float donde el valor de _rudderChangeSpeed se ira multiplicando por el tiempo que pase por frame con el objetivo de suavizar al avance o retrocesos gradualmente

        if (_currentRudderInput < targetRudderInput) //En esta condicion se checa si el valor de _currentRudderInput(0) es menor al valor que tiene targetRudderInput(1)
        {
            _currentRudderInput += rudderInputChangePerFrame; //Se guarda el valor del rudderInputChangePerFrame en _currentRudderInput actualizandose por frame
            if (_currentRudderInput > targetRudderInput) { _currentRudderInput = targetRudderInput; }//La condicion verifica si el _currentRudderInput si es mayor a targetEngineInpút es decir si se pasa del valor asignado (1), este lo limitara al valor asignado de acuerdo al input (1)
        }
        else if (_currentRudderInput > targetRudderInput) //Si no, se checa otra condicion donde se verifica si el _currentRudderInput (0) si es mayor a targetRudderInput(-1) 
        {
            _currentRudderInput -= rudderInputChangePerFrame; //Se guarda el valor del rudderInputChangePerFrame en _currentRudderInput actualizandose por frame
            if (_currentRudderInput < targetRudderInput) { _currentRudderInput = targetRudderInput; }//La condicion verifica si el _currentRudderInput si es mayor a targetEngineInpút es decir si se pasa del valor asignado (-1), este lo limitara al valor asignado de acuerdo al input (-1) 
        }
    }

    //Se utiliza right y forward inversos por la orientacion global de unity el barco para avanzar esta orientado en X y para la rotacion este utiliza Z
    private void ApplyEngineForce()
    {
        //Link Referencia a transform.right: https://docs.unity3d.com/ScriptReference/Transform-right.html 
        Vector3 engineForceDirection = transform.right; //Se crea una variable de tipo Vector3 donde esta almacenando el tranform de la eje x a la derecha es decir (1,0,0)
        Vector3 engineForce = engineForceDirection * _currentEngineInput * _engineForce; //Se crea otra variable de tipo vector que almacenara un nuevo vector en este caso la x se multiplica de el nuevo vector de engineForceDirection (1,0,0) * la variable de _currentEngineInput (-1 a 1) * la fuerza asignada en la variable engineForce (5000f)
        _rigidbody.AddForceAtPosition(engineForce, _propeller.position, ForceMode.Force);//Se aplica la fuerza en el rigidbody, se le da el nuevo Vector o mas bien la nueva direccion engineforce junto a la posicion del empty donde se gener la fuerza es decir _propeller.position para luego hacer que la fuerza sea continua con ForceMode.Force.
        //Link de referencia ForceMode.Force: https://docs.unity3d.com/es/530/ScriptReference/ForceMode.html
        //Link de referencia AddForceAtPosition: https://docs.unity3d.com/es/530/ScriptReference/Rigidbody.AddForceAtPosition.html

        //Duda en los dos metodos: Porque ponemos ForceMode.Force entiendo que hace una fuerza continua usando la masa del rigidbody pero AddForceAtPosition no hace ya eso utiizando el rigidbody?
    }

    private void ApplyRudderForce()
    {
        //Referencia a transform.forward: https://docs.unity3d.com/ScriptReference/Transform-forward.html
        Vector3 rudderForceDirection = -transform.forward; //Se crea una variable de tipo Vector3 donde esta almacenando el tranform negativo de la eje z hacia enfrente es decir (-1,0,0)
        Vector3 rudderForce = rudderForceDirection * _currentRudderInput * _rudderForce; //Se crea otra variable de tipo vector que almacenara un nuevo vector en este caso la z se multiplica de el nuevo vector negativo de rudderForceDirection (0,0,1) * la variable de _currentEngineInput (-1 a 1) * la fuerza asignada en la variable rudderForce (1000f)
        _rigidbody.AddForceAtPosition(rudderForce, _rudder.position, ForceMode.Force); //Se aplica la fuerza en el rigidbody, se le da el nuevo Vector o mas bien la nueva direccion rudderforce, junto a la posicion del empty donde se genera la fuerza es decir _rudder.position para luego hacer que la fuerza sea continua con ForceMode.Force.
    }
}