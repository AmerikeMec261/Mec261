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

    //Creamos las variables son serializefield para dejarlas en privado 

    private Rigidbody _rigidbody;

    private float _area;
    private float _hullHeight;
    private float _hullVolume;
    private float _draft;

    // Y aqui los private para hacer las formulas

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        CalculateHullData();
    }

    //Agregamos aqui el calculatehulldata para que se apliquen a la hora de la ejecución del codigo
    private void FixedUpdate()
    {
        ApplyBuoyancy();
    }
    // Y aqui el appplybouyancy igualmente para que se aplique 
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
    //Aqui ponemos el void de applybuoyancy junto con toda la formula de la flotabilidad, para que este flote 
    private void CalculateHullData()
    {
        _area = CalculateHullArea();
        _hullHeight = _topPoint.position.y - _bottomPoint.position.y;
        _hullVolume = _area * _hullHeight * _shapeFactor;

        float requiredVolume = _rigidbody.mass / _waterDensity;
        _draft = requiredVolume / (_area * _shapeFactor);
    }
    // Aqui ponemos la formula para calcular el punto mas alto y mas bajo del barco 
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

    // Calculamos los  vectores para que con el inversetransformpoint convierta el espacio local a mundial 
    //https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Transform.InverseTransformPoint.html


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
    } // El gizmos es mas que nada algo visual de unas lineas para poder ver
}