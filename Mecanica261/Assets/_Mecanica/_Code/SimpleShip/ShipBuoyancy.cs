using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class ShipBuoyancy : MonoBehaviour
{
    [Header("Water")]
    [SerializeField] private float _waterLevel = 0f;
    [SerializeField] private float _waterDensity = 1000f;
    [SerializeField] private float _waterDrag = 0.1f;

    [Header("Hull")]
    [SerializeField] private float _shapeFactor = 0.67f;
    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _bottomPoint;
    [SerializeField] private List<Transform> _buoyancyPoints = new List<Transform>();

    private Rigidbody _rigidbody;

    private float _area;
    private float _hullHeight;
    private float _hullVolume;
    private float _draft;

    private void Awake()
    {
        //Primero definimos el rigidbody que usaremos de base para la flotación del barco
        // Y despúes hacemos el cálculo del area de nuesto barco y almacenamos los resultados en una nuevo void
        _rigidbody = GetComponent<Rigidbody>();
        CalculateHullData();
    }

    private void FixedUpdate()
    {
        //Y por último, ya que tenemos nuestros resultados, aplicamos el efecto de nuestro barco flotando sobre el agua
        ApplyBuoyancy();
    }

    //Primero antes que nada conseguimos el area de nuestro barco y almacenamos todos nuestros resultados en nuevos flotantes
    //Y por último hacemos ese efecto de nuestro barco que este flotando sobre el agua.
    private void ApplyBuoyancy()
    {
        float gravityStrength = Physics.gravity.magnitude;
        float hullVolumePerPoint = _hullVolume / _buoyancyPoints.Count;

        for (int i = 0; i < _buoyancyPoints.Count; i++)
        {
            Transform buoyancyPoint = _buoyancyPoints[i];

            float submergedAmount = Mathf.Clamp01((_waterLevel - buoyancyPoint.position.y) / _hullHeight);
            if (submergedAmount <= 0f) { continue; }

            float buoyancyForce = _waterDensity * hullVolumePerPoint * gravityStrength * submergedAmount;

            _rigidbody.AddForceAtPosition(Vector3.up * buoyancyForce, buoyancyPoint.position, ForceMode.Force);

            Vector3 pointVelocity = _rigidbody.GetPointVelocity(buoyancyPoint.position);

            Vector3 waterDragForce = -pointVelocity * pointVelocity.magnitude * _waterDrag * submergedAmount;

            _rigidbody.AddForceAtPosition(waterDragForce, buoyancyPoint.position, ForceMode.Force);
        }
    }

    //En este void lo que estamos haciendo es almacenar todos nuestros calculos de area de nuestro barco en nuevas variables, para así poder conseguir
    //el efecto de nuestro barco flotando
    private void CalculateHullData() 
    {
        //Almacena el resultado del caculo del area de nuestro barco
        _area = CalculateHullArea();
        //Calcular altura del top y button point
        _hullHeight = _topPoint.position.y - _bottomPoint.position.y;
        //Calcular volumen 
        _hullVolume = _area * _hullHeight * _shapeFactor;
        // calcular el volumen requerido para flotar
        float requiredVolume = _rigidbody.mass / _waterDensity;
        _draft = requiredVolume / (_area * _shapeFactor);
    }

    //Esta float lo que hace es calcular el area de nuestro barco mediante los empties que colocamos en los vertices de nuestro barco y 
    // en los lados del centro de nuestro barco
    private float CalculateHullArea()
    {
        float area = 0f;

        for (int i = 0; i < _buoyancyPoints.Count; i++)
        {
            Vector3 currentPoint = transform.InverseTransformPoint(_buoyancyPoints[i].position);
            Vector3 nextPoint = transform.InverseTransformPoint(_buoyancyPoints[(i + 1) % _buoyancyPoints.Count].position);

            area += (currentPoint.x * nextPoint.z) - (nextPoint.x * currentPoint.z);
        }

        return Mathf.Abs(area) * 0.5f;
    }

    //Este es solo un extra para poder ver dentro del Unity el area de nuestro cubo y poder verificar si los empties de nestra lista de vertices estan bien colocados en lista.
    private void OnDrawGizmos()
    {
        if (_buoyancyPoints == null || _buoyancyPoints.Count < 2) { return; }

        Gizmos.color = Color.green;

        for (int i = 0; i < _buoyancyPoints.Count; i++)
        {
            Vector3 currentPoint = _buoyancyPoints[i].position;
            Vector3 nextPoint = _buoyancyPoints[(i + 1) % _buoyancyPoints.Count].position;

            Gizmos.DrawLine(currentPoint, nextPoint);
        }
    }
}