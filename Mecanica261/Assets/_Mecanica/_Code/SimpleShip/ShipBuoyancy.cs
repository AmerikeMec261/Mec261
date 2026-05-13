using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class ShipBuoyancy : MonoBehaviour
{
    [Header("Water")]
    [SerializeField] private float _waterLevel = 0f;  //esta es la variable que ve a que altura esta el agua entes caso a 0
    [SerializeField] private float _waterDensity = 1000f;  ///que tan densa es el agua 
    [SerializeField] private float _waterDrag = 0.1f;  //que tanto arrastre tiene el agua 

    [Header("Hull")]
    [SerializeField] private float _shapeFactor = 0.67f;
    [SerializeField] private Transform _topPoint;  //el empty que esta arriba del barco
    [SerializeField] private Transform _bottomPoint;  //el empty que esat en el centro del barco abajo
    [SerializeField] private List<Transform> _buoyancyPoints = new List<Transform>();  //es para asigar cada 1 de los puntos de flote

    private Rigidbody _rigidbody;  //obtener el rigidbodyd el barco

    private float _area;  
    private float _hullHeight;
    private float _hullVolume;
    private float _draft;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();  //llamar el rigidbody del barco para acceder a sus fisicas 
        CalculateHullData();  //llama a la funcion de hull data 
    }

    private void FixedUpdate()  //este se usa ya que el fixed update esta hecho para fisicas o matematica 
    {
        ApplyBuoyancy();  //llama a la funcion de salto del barco
    }

    private void ApplyBuoyancy()
    {
        float gravityStrength = Physics.gravity.magnitude;  //obtiene la gravedad del barco
        float hullVolumePerPoint = _hullVolume / _buoyancyPoints.Count; //divide el volumen entre la cantidad de puntos en el barco 

        for (int i = 0; i < _buoyancyPoints.Count; i++) //es un bulce para ver cada punto de flotabilidad 
        {
            Transform buoyancyPoint = _buoyancyPoints[i];   //guarda los puntos de flotacion

            float submergedAmount = Mathf.Clamp01((_waterLevel - buoyancyPoint.position.y) / _hullHeight);  //calcula que parte del punto esta bajo el agua 
            if (submergedAmount <= 0f) { continue; }    //si el punto no esta sumergido salta al siguiente

            float buoyancyForce = _waterDensity * hullVolumePerPoint * gravityStrength * submergedAmount;   //es la formula de empuje 

            _rigidbody.AddForceAtPosition(Vector3.up * buoyancyForce, buoyancyPoint.position, ForceMode.Force); //aplica una fuerza de empuje hacia arriba

            Vector3 pointVelocity = _rigidbody.GetPointVelocity(buoyancyPoint.position);    //obtiene la velocidad del barco 

            Vector3 waterDragForce = -pointVelocity * pointVelocity.magnitude * _waterDrag * submergedAmount;   //calcula la fuerza de resistencia del barco 

            _rigidbody.AddForceAtPosition(waterDragForce, buoyancyPoint.position, ForceMode.Force); //aplical la fuerza de friccion del barco 
        }
    }

    private void CalculateHullData()
    {
        _area = CalculateHullArea();
        _hullHeight = _topPoint.position.y - _bottomPoint.position.y;   //posicion del punto mas alto - el mas bajo 
        _hullVolume = _area * _hullHeight * _shapeFactor;   //la fornula del area

        float requiredVolume = _rigidbody.mass / _waterDensity; //volumen desplazado al estar en el agua 
        _draft = requiredVolume / (_area * _shapeFactor);   //cuanto se hunde 
    }

    private float CalculateHullArea()  //esta funcion es para 
    {
        float area = 0f;

        for (int i = 0; i < _buoyancyPoints.Count; i++) //mueve los puntos del barco para formar un rectangulo imaginario
        {
            Vector3 currentPoint = transform.InverseTransformPoint(_buoyancyPoints[i].position);    //convierte la posicion del punto en coordenadas locales 
            Vector3 nextPoint = transform.InverseTransformPoint(_buoyancyPoints[(i + 1) % _buoyancyPoints.Count].position); //convierte la posicion del siguiente punto en coordenadas locales 

            area += (currentPoint.x * nextPoint.z) - (nextPoint.x * currentPoint.z);
        }

        return Mathf.Abs(area) * 0.5f;
    }

    private void OnDrawGizmos()
    {
        if (_buoyancyPoints == null || _buoyancyPoints.Count < 2) { return; }

        Gizmos.color = Color.green;  //lo pinta de color verde 

        for (int i = 0; i < _buoyancyPoints.Count; i++)  //es para hacerlo constantes 
        {
            Vector3 currentPoint = _buoyancyPoints[i].position;  //es para acceder a la lista de todos los puntos 
            Vector3 nextPoint = _buoyancyPoints[(i + 1) % _buoyancyPoints.Count].position;  //busca la posicion del siguiente punto 

            Gizmos.DrawLine(currentPoint, nextPoint);  //es para crear la linea que conecta cada 1 de los puntos 
        }
    }
}