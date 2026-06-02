using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class ShipBuoyancy : MonoBehaviour
{
    [Header("Water")]          //marcamos todas las variables del agua con sus valores correspondientes 
    [SerializeField] private float _waterLevel = 0f;
    [SerializeField] private float _waterDensity = 1000f;
    [SerializeField] private float _waterDrag = 0.1f;

    [Header("Hull")]           //lo mismo pero con el hull
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
        _rigidbody = GetComponent<Rigidbody>();
        CalculateHullData();
    }

    private void FixedUpdate()        
    {
        ApplyBuoyancy();
    }

    private void ApplyBuoyancy()      //Aca vamos a obtener las fuerzas para que flote el barco 
    {
        float gravityStrength = Physics.gravity.magnitude;
        float hullVolumePerPoint = _hullVolume / _buoyancyPoints.Count;                  
        
        //Calculamos las fuerzas de gravedad y vamos a dividir el volumen del barcco entre todos los puntos de flotabilidad que existen

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

    private void CalculateHullData()        //sacamos todas las formulas para que nnuestro hull tenga las magnitudes exactas que vamos autilizzar para que el barco tenga las fisicas reales.
    {
        _area = CalculateHullArea();
        _hullHeight = _topPoint.position.y - _bottomPoint.position.y;   //sacamos la altura desde el pinto mas alto menos la parte mas baja de nuestro barco.
        _hullVolume = _area * _hullHeight * _shapeFactor;               //lo mismo pero ahora con el volumen

        float requiredVolume = _rigidbody.mass / _waterDensity;
        _draft = requiredVolume / (_area * _shapeFactor);
    }

    private float CalculateHullArea()     //calculamos toda el area para que nuestro barco avance y se vaya moviendo
    {
        float area = 0f;

        for (int i = 0; i < _buoyancyPoints.Count; i++)
        {
            Vector3 currentPoint = transform.InverseTransformPoint(_buoyancyPoints[i].position);
            Vector3 nextPoint = transform.InverseTransformPoint(_buoyancyPoints[(i + 1) % _buoyancyPoints.Count].position);

            area += (currentPoint.x * nextPoint.z) - (nextPoint.x * currentPoint.z);   //aca pues solo actualizaamos los puntos de posicion para que se mueva.
        }

        return Mathf.Abs(area) * 0.5f;
    }

    private void OnDrawGizmos()  //Esto es solo para ver que nuestra base del barco este bien.
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