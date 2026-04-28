using UnityEngine;
using System.Collections.Generic;



[RequireComponent(typeof(Rigidbody))]
public class SimpleFloat : MonoBehaviour
{
    [Header("Dependencias")]
    [SerializeField] private float _waterLevel = 0;
    [SerializeField] private float _waterDensity = 1000f;
    [SerializeField] private float _waterDrag = 10f;

    [Header("Settings")]
    [SerializeField] private float _shapeFactor;
    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _bottomPoint;
    [SerializeField] private List<Transform> _floatPoints;
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private float _area;
    [SerializeField] private float hulllHeight;
    [SerializeField] private float hullVolume;
    [SerializeField] private float draft;

    private float Area => _area;
    private float HullHeight => hulllHeight;
    private float HullVolume  => hullVolume;
    private float Draft => draft;

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
        float gravity = Physics.gravity.magnitude;
        float volumePerPoint = hullVolume / _floatPoints.Count;

        for (int i = 0; i < _floatPoints.Count; i++)
        {
            Transform point = _floatPoints[i];

            float submersion = Mathf.Clamp01((_waterLevel - point.position.y) / HullHeight);
            if (submersion <= 0f) { continue; }

            float force = _waterDensity * volumePerPoint * gravity * submersion;
            _rigidbody.AddForceAtPosition(Vector3.up * force, point.position, ForceMode.Force);

            //ADD FORCE POSITION IMPORTANTE PARA QUE EL AVANCE EL BOTE

            Vector3 velocity = _rigidbody.GetPointVelocity(point.position);
            _rigidbody.AddForceAtPosition(-velocity * _waterDrag * submersion, point.position, ForceMode.Force);
        }
    }

    private void CalculateHullData()
    {
        _area = CalculateArea();
        //Calcular altura del top y button point
        hulllHeight = (_topPoint.position.y - _bottomPoint.position.y);
        //Calcular volumen 
        hullVolume = _area * hulllHeight * _shapeFactor;
        // calcular el volumen requerido para flotar
        float requiredVolume = _rigidbody.mass / _waterDensity;
        draft = requiredVolume / (_area * _shapeFactor);

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
}