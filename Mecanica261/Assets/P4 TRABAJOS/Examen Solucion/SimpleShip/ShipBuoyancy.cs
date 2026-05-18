using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class ShipBuoyancy : MonoBehaviour
{
    [Header("Water")]
    [SerializeField] private float _waterLevel = 0f; //En esta variable de tipo float se almacena el nivel de agua
    [SerializeField] private float _waterDensity = 1000f; //En esta variable de tipo float se almacena el valor de la densidad del agua
    [SerializeField] private float _waterDrag = 0.1f; //Es la resistencia del agua que tiene cuando el barco esta en movimiento

    [Header("Hull")]
    [SerializeField] private float _shapeFactor = 0.67f; //Es la forma del barco es decir ajusta la forma o el volumen del barco
    [SerializeField] private Transform _topPoint; //Se utiliza este dato de tipo transform para saber cual es el punto mas alto del barco 
    [SerializeField] private Transform _bottomPoint; //Se utiliza este dato de tipo transform para saber cual es el punto mas najo del baroco
    [SerializeField] private List<Transform> _buoyancyPoints = new List<Transform>(); //Almacena todos los puntos del barco para su flotabilidad

    private Rigidbody _rigidbody; //Se almacena el rigidbody del barco o del objeto que tenga rigidbody

    //Estos son los datos que almacenaran un calculo cada uno usadas en las formulas de cada metodos
    private float _area;
    private float _hullHeight;
    private float _hullVolume;
    private float _draft;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();//Al ejecutar primero se lee y almacena el rigidbody 
        CalculateHullData(); //Al ejecutar primero almacena o tiene en cuenta los calculos de los datos del casco
    }

    private void FixedUpdate()
    {
        ApplyBuoyancy();//Aplica constantemente la fuerza en los puntos de flotabilidad de acuerdo a los calculos actualizandose por cada frame
    }

    private void ApplyBuoyancy()
    {
        float gravityStrength = Physics.gravity.magnitude; //Se almacena la magnitud de la fuerza de gravedad que tiene Unity por default
        //Link de referencia de .magnitude: https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Vector3-magnitude.html
        //Link de referencia de Physic.gravity: https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Physics-gravity.html 
        //Duda: Physics.gravity se que almacena la fuerza de gravedad que tiene por default unity qu ees -9.81 y magnitude se que te devuelve la longitud del vector pero en este caso porque se utiliza magnitude y no por ejemplo Mathf.Abs(Physics.gravity.y); como se utilizo en BasicTurretAim 
        float hullVolumePerPoint = _hullVolume / _buoyancyPoints.Count; //Se aplica la formula vista en clase de Volumenporpunto (Vi = Vcasco/Numero de puntos) en este caso seria (hullVolumePerPoint = _hullVolume / _buoyancyPoints.Count) el volumen de casco entre los puntos de flotabilidad

        for (int i = 0; i < _buoyancyPoints.Count; i++) //Se crea un for donde recorre cada punto mientras que buoyancyPoints sea mayor que "i" ira avanzando al siguiente punto "i++"
        //Link de referencia uso de loops: https://learn.unity.com/tutorial/loops-z2b
        {
            Transform buoyancyPoint = _buoyancyPoints[i]; //Dentro de esta variable buoyancyPoint se guarda la lista de puntos de flotabilidad _buoyancyPoints[i] 

            float submergedAmount = Mathf.Clamp01((_waterLevel - buoyancyPoint.position.y) / _hullHeight); //Se utiliza la formula vista en clase que es MathfClamp(Hagua - Yi / Hcasco)(0,1) se resta el nivel del agua - la posicion de los puntos en el eje Y entre la altura del casco (_waterLevel - buoyancyPoint.position.y) / _hullHeight) y por ultimo se utiliza Mathf.Clamp01 que limita los valores entre 0 y 1
            if (submergedAmount <= 0f) { continue; } //Se pone una condicion donde si submergedAmount es igual o menor a 0 este continua con las siguientes lineas. Duda: porque ponemos esa condicion si con MathfClamp ya limita el numero a 0 puede haber un resultado negativo? y porque solo se toma en cuenta el 0  
            //Link de referencia de Mathf.Clamp01 : https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Mathf.Clamp01.html

            float buoyancyForce = _waterDensity * hullVolumePerPoint * gravityStrength * submergedAmount;//Se aplica la formula de fuerza por punto (Fi = p * Vi * g * Si) se toma en cuenta la densidad del agua multiplicada por el volumen por punto multiplicada por la fuerza de gravedad y multiplicada por la sumersiom (_waterDensity * hullVolumePerPoint * gravityStrength * submergedAmount) y se guarda en buoyancyForce

            _rigidbody.AddForceAtPosition(Vector3.up * buoyancyForce, buoyancyPoint.position, ForceMode.Force); //En el rigidbody se aplica la fuerza hacia la posicion que es el eje Y se aplica la fuerza en los puntos de flotabilidad hacia el eje Y
            //Link de referencia de ForceMode.Force: https://docs.unity3d.com/6000.4/Documentation/ScriptReference/ForceMode.Force.html
            //Link de referencia Vector3.up: https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Vector3-up.html

            Vector3 pointVelocity = _rigidbody.GetPointVelocity(buoyancyPoint.position); //Se crea una varibale de tipo Vecotr3 llamada pointVelocity que almacenara o tendra en cuenta la velocidad angular de los puntos de flotabilidad
            //Link de referencia de GetPointVelocity: https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Rigidbody.GetPointVelocity.html

            Vector3 waterDragForce = -pointVelocity * pointVelocity.magnitude * _waterDrag * submergedAmount; //Se crea otra variable de tipo Vector3 llamada waterDragForce que guardara o aplicara la formula de Drag del agua el cual es (Fdragi = -Vi * c * Si) el valor negativo de pointVelocity se multiplica por su propia magnitud o longitud por el valor de drag del agua por la sumersion ( -pointVelocity * pointVelocity.magnitude * _waterDrag * submergedAmount;)
            //Duda: No entiendo porque en esta formula se agrega la misma magnitud de pointVelocity porque se hace esto?(pointVelocity.magnitude)
            _rigidbody.AddForceAtPosition(waterDragForce, buoyancyPoint.position, ForceMode.Force); //Por ultimo se aplica la fuerza de waterDragForce en los puntos de flotabilidad
        }
    }

    private void CalculateHullData() //Almacena todos los calculos para el barco
    {
        _area = CalculateHullArea();//Se llama el metodo porque aqui se almacena todo el calculo del area y almacena ese calculo porque es lo que regresa ese metodo (return Mathf.Abs(area) * 0.5f;)
        _hullHeight = _topPoint.position.y - _bottomPoint.position.y; //Se guarda la altura de los puntos para saber que tan sumergido debe de estar el casco con una resta que es la altura con su eje Y de _topPoint.position menos el eje Y de bottomPoint
        _hullVolume = _area * _hullHeight * _shapeFactor; //Para saber el volumen se aplica tambien una formula vista en clase (A * H * K) en este caso la area(_area) por(*) la altura del casco (_hullheight) por (*) la forma del barco  (shape factor) 

        float requiredVolume = _rigidbody.mass / _waterDensity;//Para saber el volumen requerido para flotrar se aplica la formula vista en clase que es (V para flotar = m/p) en este caso la masa que tiene el barco su rigidbody (_rigidbody.mass) entre la densidad del agua (_waterDensity)
        _draft = requiredVolume / (_area * _shapeFactor);//Para sacar el draft de igual manera se aplica la formula vista en clase (d=m/P * A * K) como la densidad del agua y masa ya se toma en cuenta en requiredvolumen namas este se divide(/) por el area (_area) por (*) la forma del barco (_shapeFactor)
    }

    private float CalculateHullArea() //De este metodo sale el calculo de la area del barco basandose en la posicion de los puntos de flotabilidad
    {
        float area = 0f; //Se crea una variable base de tipo float para que almacene el area por ahoirta asignado con el numero 0

        for (int i = 0; i < _buoyancyPoints.Count; i++) //Se crea un for donde recorre cada punto mientras que buoyancyPoints sea mayor que "i" ira avanzando al siguiente punto "i++"
        //Link de referencia uso de loops: https://learn.unity.com/tutorial/loops-z2b
        {
            Vector3 currentPoint = transform.InverseTransformPoint(_buoyancyPoints[i].position); //Guarda el transform inverso del punto de flotabilidad actual de la lista
            Vector3 nextPoint = transform.InverseTransformPoint(_buoyancyPoints[(i + 1) % _buoyancyPoints.Count].position); //En una variable de tipo vector3 llamada nextpoint se guarda la posicion inversa de los siguientes puntos sumando uno indicalo el siguiente punto de la lista
            //Link de referencia InverseTransformPoint: https://docs.unity3d.com/ScriptReference/Transform.InverseTransformPoint.html
            //Duda: No entiendo el objetivo de invertir su transform porque se hace? y tambien la misma duda en Gizmos como funciona o cual es el proposito de "%" en esa linea

            area += (currentPoint.x * nextPoint.z) - (nextPoint.x * currentPoint.z); //Se aplica el calculo visto en clase (Xi Zi+1 - Xi+1 Zi)
        }

        return Mathf.Abs(area) * 0.5f; //Como resultado se regresa el calculo de area sin signo y multiplicado por un medio que equivale a 0.5 (1/2)(Xi Zi+1 - Xi+1 Zi)
    }

    private void OnDrawGizmos() //Este metodo se hizo con el fin de verificar como estan conectado lo puntos de flotabilidad del barco y como estan ordenados usando Gizmos hacemos que dibujo una linea de color verde que sigue el orden que tiene la lista de transform sobre lo spuntos de flotabilidad del barco
    {
        if (_buoyancyPoints == null || _buoyancyPoints.Count < 2) { return; } //Se crea una condicion donde si no hay punto de flotacion O hay menos de dos puntos de flotacion no regresa nada

        Gizmos.color = Color.green; //Se le indica el color con el cual dibujara la linea
        //Link de referencia uso de loops: https://learn.unity.com/tutorial/loops-z2b
        for (int i = 0; i < _buoyancyPoints.Count; i++) //Se crea un for donde recorre cada punto mientras que buoyancyPoints sea mayor que "i" ira avanzando al siguiente punto "i++"
        //Link de referencia .count : https://docs.unity3d.com/es/530/ScriptReference/Hashtable.Count.html
        {
            Vector3 currentPoint = _buoyancyPoints[i].position; //Guarda la posicion del punto actual de la lista 
            Vector3 nextPoint = _buoyancyPoints[(i + 1) % _buoyancyPoints.Count].position; //En una variable de tipo vector3 llamada nextpoint se guarda la posicion de los siguientes puntos sumando uno indicando el siguiente punto de la lista
            //Duda: Cual es la razon de poner ese % en esa linea? Se que funciona para residuos entre la division de dos numeros pero aqui cual es su proposito?
           
            Gizmos.DrawLine(currentPoint, nextPoint); //Dibuja la linea apartir del punto actual y endo al siguiente punto de acuerdo al orden
        }
        //Link de referencia de Gizmos en general: https://docs.unity3d.com/es/530/ScriptReference/Gizmos.html 
    }
}