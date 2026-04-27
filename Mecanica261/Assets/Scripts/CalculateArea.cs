using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[RequireComponent(typeof(Rigidbody))]
public class ShipBuoyancy : MonoBehaviour
{
    [Header("Configuración del Agua")]
    [SerializeField] private float _waterLevel = 0f;
    [SerializeField] private float _waterDensity = 1000f; 
    [SerializeField] private float _waterDrag = 10f;      

    [Header("Configuración del Casco (Hull)")]
    [SerializeField, Range(0.5f, 0.8f)]
    private float _shapeFactor = 0.65f; 

    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _bottomPoint;
    [SerializeField] private List<Transform> _floatPoints;

    private Rigidbody _rigidbody;
    private float _area;
    private float _hullHigh;
    private float _HullVolume;
    private float _draft;

    
    public float Area { get; private set; }
    public float HullHeight { get; private set; }
    public float HullVolume { get; private set; }
    public float Draft { get; private set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        CalculateHullData();
      
    }

    private void FixedUpdate()
    {
        FloatShip();
    }

    private void FloatShip()
    {
        float gravity= Physics.gravity.magnitude;
        float volumePerPoint= _HullVolume/_floatPoints.Count;
        for(int i = 0; i < _floatPoints.Count;i++) 
        {
            Transform point= _floatPoints[i];
            float submersion = Mathf.Clamp01(_waterLevel - point.position.y) / _hullHigh;

            if (submersion <= 0)
            {
                continue;
            }

            float force = _waterDensity * volumePerPoint * gravity * submersion;
            _rigidbody.AddForceAtPosition(Vector3.up * force, point.position, ForceMode.Force);
            Vector3 velocity= _rigidbody.GetPointVelocity(point.position);
            _rigidbody.AddForceAtPosition(-velocity * _waterDrag * submersion, point.position, ForceMode.Force);
        }
        
    }

    private void CalculateHullData()
    {
        _area=CalculateArea();
        _hullHigh = _topPoint.position.y - _bottomPoint.position.y;
        _HullVolume = _area * _hullHigh * _shapeFactor;

        float requireVolume = _rigidbody.mass / _waterDensity;
        _draft = requireVolume / (_area * _shapeFactor);
    }

    private float CalculateArea()
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
}