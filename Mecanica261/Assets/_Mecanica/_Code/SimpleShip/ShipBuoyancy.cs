using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody))]//aqui se le dice a unity que este script requiere que el gameobject tenga un rigidbody
public class ShipBuoyancy : MonoBehaviour
{
    [Header("Water")]
    [SerializeField] private float _waterLevel = 0f;//la altura del agua en el eje y para saber si esta encima del agua, tocando e lagua o sumergido
    [SerializeField] private float _waterDensity = 1000f;//la densidad del agua, cuanto mayor la densidad mas fuerza de lfotacion y mas facil flota el objeto
    [SerializeField] private float _waterDrag = 0.1f;// la resistencia del agua para que frene al barco

    [Header("Hull")]
    [SerializeField] private float _shapeFactor = 0.67f;//es el que corrige el volumen
    [SerializeField] private Transform _topPoint;//parte alta del mdelo
    [SerializeField] private Transform _bottomPoint;//parte baja del modelo
    [SerializeField] private List<Transform> _buoyancyPoints = new List<Transform>();//la lista de los puntos de flotacion

    private Rigidbody _rigidbody;// guarda la referencia del rigidbody del barco

    private float _area;
    private float _hullHeight;
    private float _hullVolume;
    private float _draft;//cuanto se hunde el barco

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();//obtiene el rigidbody
        CalculateHullData();//llama al metodo que calcula todos los datos, el area,altura.volumen y el draft
    }

    private void FixedUpdate()
    {
        ApplyBuoyancy();//las fuerxas fisicas se aplican aqui
    }

    private void ApplyBuoyancy()
    {
        float gravityStrength = Physics.gravity.magnitude;//physics.gravity.magnitude obtiene la intensudad de la gravedad
        float hullVolumePerPoint = _hullVolume / _buoyancyPoints.Count;// reparte el volumen total de el barco entre todos los puntos de flotacion

        for (int i = 0; i < _buoyancyPoints.Count; i++)//recorre todos los puntos de flotacion es un bucle
        {
            Transform buoyancyPoint = _buoyancyPoints[i];//obtiene el elemento actual de la lista

            float submergedAmount = Mathf.Clamp01((_waterLevel - buoyancyPoint.position.y) / _hullHeight);//calcula cuanto esta sumergido ese punto, compara el nivel del aguacon la altura del punto, con el mathclamp se limita el valor y el hullheighjt normaliza esa profundidad
            if (submergedAmount <= 0f) { continue; }//si el punto no esta bajo el agua ps siguiente ciclo q haya

            float buoyancyForce = _waterDensity * hullVolumePerPoint * gravityStrength * submergedAmount;//aqui es para que si un punto se hunde mas recibe mas empuje, es para que el empuje dependa del fluido

            _rigidbody.AddForceAtPosition(Vector3.up * buoyancyForce, buoyancyPoint.position, ForceMode.Force);//aplica una fuerza en una posicion concreta,lleva la fuerza hacia arriba, se aplica en la posocion del punto y al final fuerza continua normal. Con el addforceatposition hace que la flotacion sea mas realista porque no solo empuja el barco hacia arriba

            Vector3 pointVelocity = _rigidbody.GetPointVelocity(buoyancyPoint.position);//el getpointvelocity devuelve la velocidad rreal de ese punto especifico del rigidbody, considera la velocidad lineal y la rotacion

            Vector3 waterDragForce = -pointVelocity * pointVelocity.magnitude * _waterDrag * submergedAmount;//calcula una fuerza de resistencia del agua, primero la fuerza en direccion opuesta almovimiento, despues usa la rapidez del punto, multiplicar por la magnitud otra vez, controlar la densidad del drag y su esta un poco sumergido recibe menos drag

            _rigidbody.AddForceAtPosition(waterDragForce, buoyancyPoint.position, ForceMode.Force);//aplica esa resistencia en el mismo punto del barco para que el agua frene el movimiento del barco
        }
    }

    private void CalculateHullData()
    {
        _area = CalculateHullArea();//calcula el area del barco
        _hullHeight = _topPoint.position.y - _bottomPoint.position.y;//calcula la altura del barco
        _hullVolume = _area * _hullHeight * _shapeFactor;// calcula el volumen del barco

        float requiredVolume = _rigidbody.mass / _waterDensity;//calcula cuanto volumen de agua necesita desplazar el barco para flotar
        _draft = requiredVolume / (_area * _shapeFactor);//calcula que profundiad necesita hundirse para poder desplazar el volumen y poder quedar lfotando
    }

    private float CalculateHullArea()
    {
        float area = 0f;

        for (int i = 0; i < _buoyancyPoints.Count; i++)// recorre los puntos del barco
        {
            Vector3 currentPoint = transform.InverseTransformPoint(_buoyancyPoints[i].position);
            Vector3 nextPoint = transform.InverseTransformPoint(_buoyancyPoints[(i + 1) % _buoyancyPoints.Count].position);//hace que al llegar al ultimo punto el siguiente vuelva a ser el primero

            area += (currentPoint.x * nextPoint.z) - (nextPoint.x * currentPoint.z);//calcula el area del poligono
        }

        return Mathf.Abs(area) * 0.5f;//math.abs asegura que el area salga positiva
    }

    private void OnDrawGizmos()// el gizmos es para las ayudas visuales en la escena de unity
    {
        if (_buoyancyPoints == null || _buoyancyPoints.Count < 2) { return; }

        Gizmos.color = Color.green;

        for (int i = 0; i < _buoyancyPoints.Count; i++)
        {
            Vector3 currentPoint = _buoyancyPoints[i].position;//dibuja lineas entre cada punto de flotacion y el siguiento eso es para que visualmente te ayude en la escena
            Vector3 nextPoint = _buoyancyPoints[(i + 1) % _buoyancyPoints.Count].position;

            Gizmos.DrawLine(currentPoint, nextPoint);
        }
    }
}