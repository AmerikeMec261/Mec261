using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SimpleFloat : MonoBehaviour
{
    [Header("Water Settings")]
    [SerializeField] private float _waterLevel = 0;
    [SerializeField] private float _volume = 1;
    [SerializeField] private float _waterDensity = 1000;
    [SerializeField] private float _waterDrag = 1;

    [Header("Physics Settings")]
    [SerializeField] private float _shapeFactor;
    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _bottomPoint;
    [SerializeField] private List<Transform> _floatPoints;
    private Rigidbody _rigidbody;

    [Header("Debug Settings")]
    [SerializeField] private float _area;
    [SerializeField] private float _hullHeight;
    [SerializeField] private float _hullVolume;
    [SerializeField] private float _draft;
    

    [Header("Area")]
    float Area { get => _area; }
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

    }

    [Header("Altitude")]
    float HullHeight { get => _hullHeight; }

    [Header("Volume")]
    float HullVolume { get => _hullVolume; }

    [Header("Draft")]
    float Draft { get => _draft; }



    private void FixedUpdate()
    {
        Float();
    }

    private void Float()
    {
        float summersion = Mathf.Clamp01((_waterLevel - transform.position.y / 1f));

        if (summersion < 0f) return;

        float force = _waterDensity * summersion * Physics.gravity.magnitude;

        _rigidbody.AddForce(Vector3.up * force * _volume, ForceMode.Force);
    }

    void FloatShip()
    {
        float Gravity = Physics.gravity.magnitude;
        float VolumePerPoints = _hullVolume / _floatPoints.Count;
        for (int i = 0; i < _floatPoints.Count; i++)
        {
            Transform Point = _floatPoints[i];
            float Submersion = Mathf.Clamp01(_waterLevel - Point.position.y) / _hullHeight;
            if (Submersion <= 0)
            {
                continue;
            }

            float Force = _waterDensity * Submersion * VolumePerPoints * Gravity;
            _rigidbody.AddForceAtPosition(Vector3.up * Force, Point.position, ForceMode.Force);
            Vector3 Velocity = _rigidbody.GetPointVelocity(Point.position);
            _rigidbody.AddForceAtPosition(-Velocity * _waterDrag * Submersion, Point.position, ForceMode.Force);
        }


    }

    private void CalculateHullData()
    {
        _area = CalculateAreaXZ();
        _hullHeight = _topPoint.position.y - _bottomPoint.position.y;
        _hullVolume = _area * _hullHeight * _shapeFactor;

        float requiredVolume = _rigidbody.mass / _waterDensity;
        _draft = requiredVolume / (_area * _shapeFactor);
    }

    private float CalculateAreaXZ()
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