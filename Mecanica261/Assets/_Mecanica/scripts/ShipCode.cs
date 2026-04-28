
using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;

public class SimpleFloat : MonoBehaviour
{
    [SerializeField] private float _waterlevel = 0;
    [SerializeField] private float _volume = 1;
    [SerializeField] private float _waterdensity = 1000;
    [SerializeField] private float _ShapeFactor;
    [SerializeField] private float _waterDrag;
    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _bottomPoint;
    [SerializeField] private List<Transform> _floatPoints;
    private Rigidbody _rigidbody;

    private float _Area;
    private float _HullHeight;
    private float _HullVolume;
    private float _Draft;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        CalculateHullData();
    }


    private void FixedUpdate()
    {
        FloatShip();
    }



    private void Float()
    {
        float summersion = Mathf.Clamp01((_waterlevel - transform.position.y / 1f));

        if (summersion < 0f) return;

        float force = _waterdensity * summersion * _volume * Physics.gravity.magnitude;

        _rigidbody.AddForce(Vector3.up * force, ForceMode.Force);
    }



    private void FloatShip()
    {
        float gravity = Physics.gravity.magnitude;

        float volumenperpoint = _HullVolume / _floatPoints.Count;

        for (int i = 0; i < _floatPoints.Count; i++)
        {
            Transform point = _floatPoints[i];

            float Submersion = Mathf.Clamp01(_waterlevel - point.position.y / _HullHeight);

            if (Submersion <= 0f)
            {
                continue;
            }

            float force = _waterdensity * volumenperpoint * gravity * Submersion;

            _rigidbody.AddForceAtPosition(Vector3.up * force, point.position, ForceMode.Force);

            Vector3 velocity = _rigidbody.GetPointVelocity(point.position);

            _rigidbody.AddForceAtPosition(-velocity * _waterDrag * Submersion, point.position, ForceMode.Force);
        }
    }

    private void CalculateHullData()
    {
        _Area = CalculateArea();

        _HullHeight = _topPoint.position.y - _bottomPoint.position.y;
        _HullVolume = _Area * _HullHeight * _ShapeFactor;

        float RiquiereVolume = _rigidbody.mass / _waterdensity;

        _Draft = RiquiereVolume / (_Area * _ShapeFactor);





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

    
/*
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class ShipFlotability : MonoBehaviour
{
    [Header("Water")]
    [SerializeField] private float _waterLevel = 0f;
    [SerializeField] private float _waterDensity = 1000f;
    [SerializeField] private float _waterDrag = 1f;

    [Header("Hull")]
    [SerializeField] private float _shapeFactor = 0.67f;
    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _bottomPoint;
    [SerializeField] private List<Transform> _floatPoints = new List<Transform>();

    private Rigidbody _rigidbody;

    private float _area;
    private float _hullHeight;
    private float _hullVolume;
    private float _draft;

    public float Area => _area;
    public float HullHeight => _hullHeight;
    public float HullVolume => _hullVolume;
    public float Draft => _draft;

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
        float volumePerPoint = _hullVolume / _floatPoints.Count;

        for (int i = 0; i < _floatPoints.Count; i++)
        {
            Transform point = _floatPoints[i];

            float submersion = Mathf.Clamp01((_waterLevel - point.position.y) / _hullHeight);
            if (submersion <= 0f) { continue; }

            float force = _waterDensity * volumePerPoint * gravity * submersion;
            _rigidbody.AddForceAtPosition(Vector3.up * force, point.position, ForceMode.Force);

            Vector3 velocity = _rigidbody.GetPointVelocity(point.position);
            _rigidbody.AddForceAtPosition(-velocity * _waterDrag * submersion, point.position, ForceMode.Force);
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

    private void OnDrawGizmos()
    {
        if (_floatPoints == null || _floatPoints.Count < 2) { return; }

        Gizmos.color = Color.red;

        for (int i = 0; i < _floatPoints.Count; i++)
        {
            Vector3 current = _floatPoints[i].position;
            Vector3 next = _floatPoints[(i + 1) % _floatPoints.Count].position;

            Gizmos.DrawLine(current, next);
        }
    }
}

*/
