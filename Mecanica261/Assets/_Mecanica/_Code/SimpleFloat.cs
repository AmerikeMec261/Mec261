using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SimpleFloat : MonoBehaviour
{
    [Header("Water Settings")]
    [SerializeField] private float _waterLevel = 0;
    [SerializeField] private float _volume = 1;
    [SerializeField] private float _waterDensity = 1000;

    [Header("Physics Settings")]
    [SerializeField] private float _shapeFactor;
    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _bottomPoint;
    [SerializeField] private List<Transform> _floatPoints;
    [SerializeField] private Rigidbody _rigidbody;

    [Header("Debug Settings")]
    [SerializeField] private float area;
    [SerializeField] private float hullHeight;
    [SerializeField] private float hullVolume;
    [SerializeField] private float draft;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

    }


    private void awake()
    {

    }

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

    private void FloatShip()
    {

    }

    private void CalculateHullData()
    {

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