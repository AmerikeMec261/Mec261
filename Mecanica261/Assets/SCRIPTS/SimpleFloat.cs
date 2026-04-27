using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

public class SimpleFloat : MonoBehaviour
{
    [SerializeField] private float _waterLevel = 1.0f;
    [SerializeField] private float _waterDensity = 1.0f;
    [SerializeField] private float _waterDrag = 1.0f;

    [SerializeField] private float _shapeFactor = 1.0f;
    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _bottomPoint;
    [SerializeField] private List<Transform> _floatPoints;
    private Rigidbody _rigidbody;

    private float _Area;
    private float _HullHeight;
    private float _HullVolume;
    private float _Draft;

    public float Area { get; }
    public float HullHeight { get; }
    public float HullVolume { get; }
    public float Draft {  get; }



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
       float Gravity = Physics.gravity.magnitude;
        float volumenperpoint = _HullVolume / _floatPoints.Count;
        for (int i = 0;  i < _floatPoints.Count; i++)
        {
            Transform point = _floatPoints[i];
            float submersion = Mathf.Clamp01(_waterDensity - point.position.y / _HullHeight);
            if (submersion <= 0f)
            {
                continue;
            }
            float force = _waterDensity * volumenperpoint * Gravity * submersion;
            _rigidbody.AddForceAtPosition(Vector3.up * force , point.position, ForceMode.Force);
            Vector3 velocity = _rigidbody.GetPointVelocity(point.position);
            _rigidbody.AddForceAtPosition(-velocity * _waterDrag * submersion, point.position, ForceMode.Force);

        }
    }

    private void CalculateHullData()
    {
        _Area = CalculateArea();
        _HullHeight = _topPoint.position.y - _bottomPoint.position.y;
        _HullVolume = _Area * _HullHeight * _shapeFactor;

        float requiredVolume = _rigidbody.mass / _waterDensity;
        _Draft = requiredVolume / (_Area * _shapeFactor);
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
